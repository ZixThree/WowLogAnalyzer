using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Collections.Concurrent;

namespace WowLogAnalyzer.Wow.Logs
{
    public class LogReader : IDisposable
    {
        private const int BlockSize = 32;
        private const int MaxProcessingBlocksMultiplier = 6;

        private readonly DateTime _now = DateTime.Now;
        private readonly int _maxProcessingBlocks;
        private StreamReader _reader;
        int _line = 0;
        bool _endOfFile = false;
        Thread _readerThread;
        ManualResetEventSlim _newDataAvailable = new ManualResetEventSlim(false);
        ManualResetEventSlim _dataNeeded = new ManualResetEventSlim(true);
        ManualResetEvent _canClose = new ManualResetEvent(false);
        CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        int _currentBlock;
        int _nextBlock;
        Queue<ProcessingBlock> _processedBlock = new Queue<ProcessingBlock>();
        ConcurrentDictionary<int, ProcessingBlock> _readyBlocks = new ConcurrentDictionary<int, ProcessingBlock>();
        Queue<LogEvent> _resultEvents = new Queue<LogEvent>();
        ConcurrentQueue<ProcessingBlock> _recycling = new ConcurrentQueue<ProcessingBlock>();

        private class ProcessingBlock
        {
            public ManualResetEventSlim Ready = new ManualResetEventSlim(false);
            public int BlockNumber;
            public string[] InputData = new String[BlockSize];
            public Queue<LogEvent> OutputData = new Queue<LogEvent>(BlockSize + 1);
        }

        public LogReader(Stream stream)
        {
            _maxProcessingBlocks = Environment.ProcessorCount * MaxProcessingBlocksMultiplier;
            _reader = new StreamReader(stream);
            _readerThread = new Thread(StartReading);
            _readerThread.Start();
        }

        private void StartReading()
        {
            CancellationToken token = _cancellationToken.Token;
            while ( true ) {
                if ( token.IsCancellationRequested )
                    break;

                bool isFull = (_nextBlock - _currentBlock) >= _maxProcessingBlocks;
                if ( !isFull ) {
                    ProcessingBlock block = null;
                    TryRecyclingBlock(out block, _nextBlock++);

                    for ( int i = 0; i < block.InputData.Length; i++ ) {
                        _line++;
                        String nextEventLine = _reader.ReadLine();
                        block.InputData[i] = nextEventLine;

                        if ( nextEventLine == null ) {
                            _endOfFile = true;
                            break;
                        }
                    }

                    // Special case verification where block would be empty (end of file on first record)
                    if ( block.InputData[0] != null ) {
                        _readyBlocks[block.BlockNumber] = block;
                    }

                    ThreadPool.QueueUserWorkItem(ProcessBlock, block);

                    _newDataAvailable.Set();

                    if ( _endOfFile )
                        break;

                } else {
                    //_newDataAvailable.Set();
                    _dataNeeded.Wait(token);
                }
            }
            _canClose.Set();
        }

        private void TryRecyclingBlock(out ProcessingBlock block, int blockNumber)
        {
            if ( _recycling.TryDequeue(out block) ) {
                for ( int i = 0; i < block.InputData.Length; i++ ) {
                    block.InputData[i] = null;
                    block.OutputData.Clear();
                    block.Ready.Reset();
                }
            } else {
                block = new ProcessingBlock();
            }
            block.BlockNumber = blockNumber;
        }

        private void ProcessBlock(object blockObject)
        {
            ProcessingBlock block = blockObject as ProcessingBlock;

            for ( int i = 0; i < block.InputData.Length; i++ ) {
                string input = block.InputData[i];
                if ( input != null ) {
                    block.OutputData.Enqueue(ProcessEventLine(input));
                } else {
                    break;
                }
            }

            block.Ready.Set();
        }

        private LogEvent ProcessEventLine(string nextEventLine)
        {
            DateTime dateTime;
            int index = 0;
            ReadTimestamp(nextEventLine, out dateTime, ref index);

            IEnumerator<String> enumeration = Tokenize(nextEventLine.Substring(index + 1)).GetEnumerator();

            enumeration.MoveNext();
            string eventName = enumeration.Current;

            return new LogEvent(dateTime, eventName, enumeration);
        }

        private void ReadTimestamp(string nextEventLine, out DateTime dateTime, ref int index)
        {
            // dateTime = DateTime.ParseExact(nextEventLine.Substring(start, end - start), "M/d HH:mm:ss.fff", invariantCulture);
            int year = _now.Year;
            int month;
            int day;
            int hour;
            int minute;
            int second;
            int millis;
            month = (nextEventLine[index++] - '0');
            if ( nextEventLine[index] != '/' ) {
                month = (month * 10) + (nextEventLine[index++] - '0');
            }
            index++;

            day = (nextEventLine[index++] - '0');
            if ( nextEventLine[index] != ' ' ) {
                day = (day * 10) + (nextEventLine[index++] - '0');
            }
            index++;

            hour = (nextEventLine[index++] - '0') * 10;
            hour += nextEventLine[index++] - '0';
            index++;
            minute = (nextEventLine[index++] - '0') * 10;
            minute += nextEventLine[index++] - '0';
            index++;
            second = (nextEventLine[index++] - '0') * 10;
            second += nextEventLine[index++] - '0';
            index++;
            millis = (nextEventLine[index++] - '0') * 100;
            millis += (nextEventLine[index++] - '0') * 10;
            millis += (nextEventLine[index++] - '0');

            dateTime = new DateTime(year, month, day, hour, minute, second, millis, DateTimeKind.Local);
        }

        public void Close()
        {
            _cancellationToken.Cancel();
            _canClose.WaitOne();
            _reader.Close();
        }

        void IDisposable.Dispose()
        {
            Close();
        }

        private static readonly CultureInfo invariantCulture = CultureInfo.InvariantCulture;
        public LogEvent ReadEvent()
        {
            ProcessingBlock current = null;
            while ( current == null ) {
                bool dataFound = false;
                dataFound = _readyBlocks.TryGetValue(_currentBlock, out current);
                if ( !dataFound ) {
                    if ( _endOfFile ) {
                        return null;
                    }
                    current = null;
                    _newDataAvailable.Reset();
                    _dataNeeded.Set();
                    _newDataAvailable.Wait(_cancellationToken.Token);
                    if ( _cancellationToken.IsCancellationRequested )
                        throw new OperationCanceledException("ReadEvent was canceled!", _cancellationToken.Token);
                    _dataNeeded.Reset();
                }
            }

            current.Ready.Wait(_cancellationToken.Token);
            if ( _cancellationToken.IsCancellationRequested ) {
                throw new OperationCanceledException("ReadEvent was canceled!", _cancellationToken.Token);
            }
            LogEvent result = current.OutputData.Dequeue();
            if ( current.OutputData.Count == 0 ) {
                _currentBlock++;
                _recycling.Enqueue(current);
            }
            return result;
        }

        private static IEnumerable<String> Tokenize(String line)
        {
            int start = 0;
            bool quoted = false;
            for ( int index = 0; index < line.Length; index++ ) {
                if ( !quoted && line[index] == ',' ) {
                    yield return line.Substring(start, index - start);
                    start = index + 1;
                } else {
                    if ( line[index] == '"' ) {
                        if ( quoted ) {
                            if ( index + 1 >= line.Length || line[index + 1] == ',' ) {
                                yield return line.Substring(start, index - start);
                                index++;
                                start = index + 1;
                                quoted = false;
                            }
                        } else {
                            if ( index <= 0 || line[index - 1] == ',' ) {
                                quoted = true;
                                start = index + 1;
                            }
                        }
                    }
                }
            }

            if ( line.Length - start > 0 ) {
                yield return line.Substring(start);
            }
        }
    }
}
