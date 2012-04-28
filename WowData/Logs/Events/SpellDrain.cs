using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellDrain : CombatLogEventDrain
	{
		private CombatLogSpell spell;

		public override string Name { get { return "SPELL_DRAIN"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateDrain(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellDrain(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
