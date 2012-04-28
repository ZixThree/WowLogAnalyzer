using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs
{
    public class EventRepository : List<LogEvent>
    {
        //CombatLog log;

        private EventRepository(CombatLog log)
        {
        }
    }
}
