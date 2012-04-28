using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class EnvironmentalDamage : CombatLogEventDamage
	{
		private CombatLogEnvironmental environmentalKind;

		public override string Name { get { return "ENVIRONMENTAL_DAMAGE"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			environmentalKind = reader.ReadEnum<CombatLogEnvironmental>();
			PopulateDamageEvent(reader);
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitEnvironmentalDamage(this);
		}

		public CombatLogEnvironmental EnvironmentKind { get { return environmentalKind; } }
	}
}
