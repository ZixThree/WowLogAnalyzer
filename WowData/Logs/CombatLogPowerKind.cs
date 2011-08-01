using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs
{
    public enum CombatLogPowerKind
    {
        Health = -2,
        Mana = 0,
        Rage = 1,
        Focus = 2,
        Energy = 3,
        PetHappiness = 4,
        Runes = 5,
        RunicPower = 6,
        SoulShard = 7,
        EclipseEnergy = 8,
        HolyPower = 9,
        Sound = 10, // Atramedes.
    }
}
