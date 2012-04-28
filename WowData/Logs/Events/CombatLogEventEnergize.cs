using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public abstract class CombatLogEventEnergize : CombatLogEvent
	{
		int amount;
		int powerType;

		protected void PopulateEnergizeEvent( EventReader reader )
		{
			amount = reader.ReadInt32();
			powerType = reader.ReadInt32();
		}

		public int Amount { get { return amount; } }
		public int PowerType { get { return powerType; } }
	}
}
