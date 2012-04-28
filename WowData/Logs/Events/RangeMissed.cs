using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class RangeMissed : CombatLogEventMissed
	{
		private CombatLogSpell spell;

		public override string Name { get { return "RANGE_MISSED"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateMissed(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitRangeMissed(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
