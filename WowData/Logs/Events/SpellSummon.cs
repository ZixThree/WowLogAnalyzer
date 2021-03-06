﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellSummon : CombatLogEvent
	{
		private CombatLogSpell spell;

		public override string Name { get { return "SPELL_SUMMON"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellSummon(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
	}
}
