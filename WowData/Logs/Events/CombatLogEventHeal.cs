using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public abstract class CombatLogEventHeal : CombatLogEvent
	{
		private int amount;
		private int overkill;
		private int absorbed;
		private bool critical;

		protected void PopulateHeal( EventReader reader )
		{
			amount = reader.ReadInt32();
			overkill = reader.ReadInt32();
			absorbed = reader.ReadInt32();
			critical = reader.ReadString() != null;
		}

		public int Amount { get { return amount; } }
		public int Overkill { get { return overkill; } }
		public int Absorbed { get { return absorbed; } }
		public bool Critical { get { return critical; } }
	}
}
