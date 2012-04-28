using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellExtraAttacks : CombatLogEvent
	{
		CombatLogSpell spell;
		int amount;

		public override string Name { get { return "SPELL_EXTRA_ATTACKS"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			amount = reader.ReadInt32();
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellExtraAttacks(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
		public int Amount { get { return amount; } }
	}
}
