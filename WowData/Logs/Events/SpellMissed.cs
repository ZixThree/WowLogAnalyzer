using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellMissed : CombatLogEventMissed
	{
		private CombatLogSpell spell;

		public override string Name { get { return "SPELL_MISSED"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateMissed(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellMissed(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
