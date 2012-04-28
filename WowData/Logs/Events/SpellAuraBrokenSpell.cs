using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellAuraBrokenSpell : CombatLogEventAura
	{
		CombatLogSpell spell;
		CombatLogSpell extraSpell;

		public override string Name { get { return "SPELL_AURA_BROKEN_SPELL"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			extraSpell = reader.ReadSpell();
			PopulateAuraEvent(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellAuraBrokenSpell(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
		public CombatLogSpell ExtraSpell { get { return extraSpell; } }
	}
}
