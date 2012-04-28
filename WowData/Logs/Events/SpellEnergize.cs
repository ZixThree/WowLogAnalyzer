using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellEnergize : CombatLogEventEnergize
	{
		private CombatLogSpell spell;

		public override string Name { get { return "SPELL_ENERGIZE"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateEnergizeEvent(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellEnergize(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
