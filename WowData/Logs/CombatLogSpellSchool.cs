using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WowLogAnalyzer.Wow.Logs
{
    [Flags]
    public enum CombatLogSpellSchool
    {
        // Color.FromArgb(255, 255, 0)
        Physical = 0x01,
        // Color.FromArgb(255,230,128)
        Holy = 0x02,
        // Color.FromArgb(255,128,0)
        Fire = 0x04,
        // Color.FromArgb(77,255,77)
        Nature = 0x08,
        // Color.FromArgb(128,255,255)
        Frost = 0x010,
        // Color.FromArgb(128,128,255)
        Shadow = 0x020,
        // Color.FromArgb(255,128,255)
        Arcane = 0x040,

        Holystrike = Holy | Physical,
        Flamestrike = Fire | Physical,
        Holyfire = Fire | Holy,
        Stormstrike = Nature | Physical,
        Holystorm = Nature | Holy,
        Firestorm = Nature | Fire,
        Froststrike = Frost | Physical,
        Holyfrost = Frost | Holy,
        Frostfire = Frost | Fire,
        Froststorm = Frost | Nature,
        Shadowstrike = Shadow | Physical,
        Shadowlight = Shadow | Holy,
        Twilight = Shadow | Holy,
        Shadowflame = Shadow | Fire,
        Shadowstorm = Shadow | Nature,
        Plague = Shadow | Nature,
        ShadowFrost = Shadow | Frost,
        Spellstrike = Arcane | Physical,
        Divine = Arcane | Holy,
        Spellfire = Arcane | Fire,
        Spellstorm = Arcane | Nature,
        Spellfrost = Arcane | Frost,
        Spellshadow = Arcane | Shadow,

        Elemental = Frost | Nature | Fire,
        Chromatic = Arcane | Shadow | Frost | Nature | Fire,
        Magic = Arcane | Shadow | Frost | Nature | Fire | Holy,
        Chaos = Arcane | Shadow | Frost | Nature | Fire | Holy | Physical
    }
}
