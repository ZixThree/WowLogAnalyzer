using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellPeriodicDamage : CombatLogEventDamage
	{
		private CombatLogSpell spell;

		public override string Name { get { return "SPELL_PERIODIC_DAMAGE"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateDamageEvent(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellPeriodicDamage(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
