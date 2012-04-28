using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellPeriodicHeal : CombatLogEventHeal
	{
		private CombatLogSpell spell;

		public override string Name { get { return "SPELL_PERIODIC_HEAL"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateHeal(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellPeriodicHeal(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
