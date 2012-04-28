using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public abstract class CombatLogEventDrain : CombatLogEvent
	{
		private int amount;
		private int powerType;
		private int extraAmount;

		protected void PopulateDrain( EventReader reader )
		{
			amount = reader.ReadInt32();
			powerType = reader.ReadInt32();
			extraAmount = reader.ReadInt32();
		}

		public int Amount { get { return amount; } }
		public int PowerType { get { return powerType; } }
		public int ExtraAmount { get { return extraAmount; } }
	}
}
