using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WowLogAnalyzer.Wow.Logs
{
    public class CombatLog
    {
        const int InitialCapacity = 100000;
        const double InitialLoadFactor = 1.10;
        const double AverageCharacterPerLine = 155;
        List<LogEvent> _events;
        int totalCount;

        public CombatLog(Stream stream)
            : this(new LogReader(stream))
        {
        }

        public CombatLog(LogReader reader)
        {
            Stream baseStream = reader.BaseStream;
            int initialListSize = InitialCapacity;
            if ( baseStream.CanSeek ) {
                long length = baseStream.Length;
                initialListSize = (int)((length / AverageCharacterPerLine) * InitialLoadFactor);
            }
            _events = new List<LogEvent>(initialListSize);
            Console.WriteLine("\t\tInitial List Size: {0}", initialListSize);
            LogEvent next;
            totalCount = 0;
            while ( (next = reader.ReadEvent()) != null ) {
                if ( (next.Source.Flags & CombatLogUnitFlags.Outsider) == 0 &&
                    (next.Destination.Flags & CombatLogUnitFlags.Outsider) == 0) {
                    _events.Add(next);
                }
                totalCount++;
            }
            //_events.TrimExcess();
            reader.Close();
        }

        public IList<LogEvent> Events { get { return _events; } }

        public int TotalCount { get { return totalCount; } }
    }
}
