using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellAuraAppliedDose : CombatLogEventAura
	{
		CombatLogSpell spell;

		public override string Name { get { return "SPELL_AURA_APPLIED_DOSE"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateAuraEvent(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellAuraAppliedDose(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
