using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs
{
    public enum CombatLogUnitFlags : uint
    {
        // Object Type
        IsObject = 0x00004000,
        IsGuardian = 0x00002000,
        IsPet = 0x00001000,
        IsNpc = 0x00000800,
        IsPlayer = 0x00000400,

        // Object Control
        ControlledByNpc = 0x00000200,
        ControlledByPlayer = 0x00000100,

        // Object Reaction
        Hostile = 0x00000040,
        Neutral = 0x00000020,
        Friendly = 0x00000010,

        // Object Affiliation
        Outsider = 0x00000008,
        Raid = 0x00000004,
        Party = 0x00000002,
        Mine = 0x00000001,

        // Special Cases
        Unknown1 = 0x80000000, // None
        MainAssist = 0x00080000,
        MainTank = 0x00040000,
        Focus = 0x00020000,
        Target = 0x00010000
    }
}
