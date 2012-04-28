using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellInstakill : CombatLogEvent
	{
		CombatLogSpell spell;

		public override string Name { get { return "SPELL_INSTAKILL"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellInstakill(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
