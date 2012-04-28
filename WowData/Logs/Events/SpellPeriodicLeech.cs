using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellPeriodicLeech : CombatLogEventDrain
	{
		CombatLogSpell spell;

		public override string Name { get { return "SPELL_PERIODIC_LEECH"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateDrain(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellPeriodicLeech(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
