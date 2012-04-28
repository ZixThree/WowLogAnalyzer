using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class DamageShield : CombatLogEventDamage
	{
		private CombatLogSpell spell;

		public override string Name { get { return "DAMAGE_SHIELD"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateDamageEvent(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitDamageShield(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
