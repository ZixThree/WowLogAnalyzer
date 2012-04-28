using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellCreate : CombatLogEvent
	{
		private CombatLogSpell spell;

		public override string Name { get { return "SPELL_CREATE"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellCreate(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
