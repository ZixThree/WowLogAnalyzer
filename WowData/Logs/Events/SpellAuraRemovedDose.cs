using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellAuraRemovedDose : CombatLogEventAura
	{
		CombatLogSpell spell;

		public override string Name { get { return "SPELL_AURA_REMOVED_DOSE"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateAuraEvent(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellAuraRemovedDose(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
