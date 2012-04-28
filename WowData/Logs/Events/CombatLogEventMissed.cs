using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public abstract class CombatLogEventMissed : CombatLogEvent
	{
		private CombatLogMissKind missKind;
		private int amount;

		protected void PopulateMissed( EventReader reader )
		{
			missKind = reader.ReadEnum<CombatLogMissKind>();
			if( missKind == CombatLogMissKind.Block ||
				missKind == CombatLogMissKind.Absorb ||
				missKind == CombatLogMissKind.Resist
			   ) {
				amount = reader.ReadInt32();
			}
		}

		public CombatLogMissKind MissKind { get { return missKind; } }
		public int MissAmount { get { return amount; } }
	}
}
