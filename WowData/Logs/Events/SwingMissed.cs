using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SwingMissed : CombatLogEventMissed
	{
		public override string Name { get { return "SWING_MISSED"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			PopulateMissed(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSwingMissed(this);
		}
	}
}
