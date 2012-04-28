using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellStolen : CombatLogEvent
	{
		private CombatLogSpell spell;
		private CombatLogSpell extraSpell;
		private CombatLogAuraKind auraKind;

		public override string Name { get { return "SPELL_STOLEN"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			extraSpell = reader.ReadSpell();
			auraKind = reader.ReadEnum<CombatLogAuraKind>();
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellStolen(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
		public CombatLogSpell ExtraSpell { get { return extraSpell; } }
		public CombatLogAuraKind AuraKind { get { return auraKind; } }
	}
}
