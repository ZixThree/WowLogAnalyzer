using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class SpellAuraApplied : CombatLogEventAura
	{
		CombatLogSpell spell;
		string unknown1;
		string unknown2;

		public override string Name { get { return "SPELL_AURA_APPLIED"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			spell = reader.ReadSpell();
			PopulateAuraEvent(reader);
			if( reader.HasNextValue ) {
				unknown1 = reader.ReadString();
				unknown2 = reader.ReadString();
			}
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitSpellAuraApplied(this);
		}

		public CombatLogSpell Spell { get { return spell; } }
		public string Unknown1 { get { return unknown1; } }
		public string Unknown2 { get { return unknown2; } }
	}
}
