using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs
{
    public interface IEventChooser
    {
        bool IsEventNeeded(LogEvent e);
    }
}
