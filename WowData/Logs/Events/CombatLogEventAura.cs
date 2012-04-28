using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public abstract class CombatLogEventAura : CombatLogEvent
	{
		CombatLogAuraKind auraKind;
		int amount;

		protected void PopulateAuraEvent( EventReader reader )
		{
			auraKind = reader.ReadEnum<CombatLogAuraKind>();
			if( reader.HasNextValue ) {
				// Note: seems to be used for absorb effects.
				amount = reader.ReadInt32();
			}
		}

		public CombatLogAuraKind AuraKind { get { return auraKind; } }
		public int Amount { get { return amount; } }
	}
}
