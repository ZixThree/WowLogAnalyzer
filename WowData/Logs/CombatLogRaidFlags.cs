using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs
{
    [Flags]
    public enum CombatLogRaidFlags
    {
        RaidTarget8 = 0x80,
        RaidTarget7 = 0x40,
        RaidTarget6 = 0x20,
        RaidTarget5 = 0x10,
        RaidTarget4 = 0x08,
        RaidTarget3 = 0x04,
        RaidTarget2 = 0x02,
        RaidTarget1 = 0x01
    }
}
