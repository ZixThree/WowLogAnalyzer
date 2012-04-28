using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using WowLogAnalyzer.Wow.Logs.Events;

namespace WowLogAnalyzer.Wow.Logs
{
    public enum LogEventName
    {
        Unknown,

        SwingDamage,
        SwingMissed,

        RangeDamage,
        RangeMissed,

        SpellDamage,
        SpellHeal,
        SpellMissed,

        SpellSummon,
        SpellDispel,
        SpellDrain,
        SpellEnergize,
        SpellCreate,
        SpellInstaKill,
        SpellExtraAttacks,
        SpellResurrect,
        SpellInterrupt,
        SpellStolen,

        SpellCastSuccess,
        SpellCastStart,
        SpellCastFailed,

        SpellAuraApplied,
        SpellAuraRemoved,
        SpellAuraRefresh,
        SpellAuraAppliedDose,
        SpellAuraRemovedDose,

        SpellPeriodicDamage,
		SpellPeriodicDrain,
        SpellPeriodicHeal,
        SpellPeriodicEnergize,
        SpellPeriodicMissed,
        SpellPeriodicLeech,

        SpellAuraBrokenSpell,

        PartyKill,

        UnitDied,
        UnitDestroyed,

        DamageShield,
        DamageSplit,
        DamageShieldMissed,

        EnvironmentalDamage,

        EnchantApplied,
        EnchantRemoved
    }

    public class LogEvent
    {
		private string originalLine;
		private CombatLogEvent e;

        public LogEvent(string originalLine, DateTime timestamp, string eventName, IEnumerator<string> parameters)
        {
            string name = eventName.Replace("_", "").Trim();
			this.originalLine = originalLine;

			//foreach(Type type in Assembly.GetExecutingAssembly().GetTypes()) {
			//    if( type.Namespace == "WowLogAnalyzer.Wow.Logs.Events" ) {
			//        if( StringComparer.InvariantCultureIgnoreCase.Compare(type.Name, name) == 0) {
			//            e = (CombatLogEvent)Activator.CreateInstance(type);
			//            e.Populate(timestamp, new EventReader(parameters));
			//            break;
			//        }
			//    }
			//}
			e = CombatLogEventFactory.CreateEvent(eventName.Trim());
			e.Populate(timestamp, new EventReader(parameters));

            if ( parameters.MoveNext() )
                throw new ApplicationException("Additional unknown parameters for " + name + ".");
        }


        public DateTime Timestamp { get { return e.TimeStamp; } }
        public string Name { get { return e.Name; } }
        public CombatLogUnit Source { get { return e.Source; } }
        public CombatLogUnit Destination { get { return e.Destination; } }
		public CombatLogEvent Event { get { return e; } }
		public string Line { get { return originalLine; } }
    }
}
