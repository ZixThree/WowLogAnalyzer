using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs
{
    public class CombatLogUnit
    {
        private readonly string _name;
        private readonly long _id;
        private readonly CombatLogUnitFlags _flags;
        private readonly CombatLogRaidFlags _raidFlags;

        public CombatLogUnit(string name, long id, CombatLogUnitFlags flags, CombatLogRaidFlags raidFlags)
        {
            _name = name;
            _id = id;
            _flags = flags;
            _raidFlags = raidFlags;
        }

        public string Name { get { return _name; } }
        public long Id { get { return _id; } }
        public CombatLogUnitFlags Flags { get { return _flags; } }
        public CombatLogRaidFlags RaidFlags { get { return _raidFlags; } }

    }
}
