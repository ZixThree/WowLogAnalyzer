using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellCastFailed : CombatLogEvent
	{
		private CombatLogSpell spell;
		private string failMessage;

		public override string Name { get { return "SPELL_CAST_FAILED"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			failMessage = reader.ReadString();
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellCastFailed(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
		public string Reason { get { return failMessage; } }
	}
}
