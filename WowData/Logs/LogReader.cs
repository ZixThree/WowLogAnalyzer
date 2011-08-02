using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace WowLogAnalyzer.Wow.Logs
{
    public class LogReader : IDisposable
    {
        private StreamReader _reader;
        int line = 0;

        public LogReader(Stream stream)
        {
            _reader = new StreamReader(stream);
        }

        public void Close()
        {
            _reader.Close();
        }

        void IDisposable.Dispose()
        {
            Close();
        }

        public LogEvent ReadEvent()
        {
            LogEvent result = null;
            line++;
            //try {
                String nextEventLine = _reader.ReadLine();
                if ( nextEventLine == null )
                    return null;

                int start = 0;
                int end = nextEventLine.IndexOf(' '); // Date part
                end = nextEventLine.IndexOf(' ', end + 1); // Time part

                DateTime dateTime =
                   DateTime.ParseExact(nextEventLine.Substring(start, end - start), "M/d HH:mm:ss.fff", CultureInfo.InvariantCulture);

                IEnumerator<String> enumeration = Tokenize(nextEventLine.Substring(end + 2)).GetEnumerator();

                enumeration.MoveNext();
                string eventName = enumeration.Current;

                result = new LogEvent(dateTime, eventName, enumeration);
            //}
            //catch ( Exception e ) {
            //    throw new ApplicationException(String.Concat("Error at line number: ", line, "."), e);
            //}
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
