using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellInterrupt : CombatLogEvent
	{
		private CombatLogSpell spell;
		private CombatLogSpell extraSpell;

		public override string Name { get { return "SPELL_INTERRUPT"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			extraSpell = reader.ReadSpell();
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellInterrupt(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
		public CombatLogSpell ExtraSpell { get { return extraSpell; } }
	}
}
