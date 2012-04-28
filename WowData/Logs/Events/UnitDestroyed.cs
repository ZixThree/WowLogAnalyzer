using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class UnitDestroyed : CombatLogEvent
	{
		public override string Name { get { return "UNIT_DESTROYED"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitUnitDestroyed(this);
		}
	}
}
