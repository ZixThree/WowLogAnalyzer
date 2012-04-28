using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public abstract class CombatLogEventDamage : CombatLogEvent
	{
		int amount;
		int overkill;
		CombatLogSpellSchool damageSchool;
		int resisted;
		int blocked;
		int absorbed;
		bool critical;
		bool glancing;
		bool crushing;

		protected void PopulateDamageEvent( EventReader reader )
		{
			amount = reader.ReadInt32();
			overkill = reader.ReadInt32();
			damageSchool = (CombatLogSpellSchool)reader.ReadUInt32();
			resisted = reader.ReadInt32();
			blocked = reader.ReadInt32();
			absorbed = reader.ReadInt32();
			critical = reader.ReadString() != null;
			glancing = reader.ReadString() != null;
			crushing = reader.ReadString() != null;
		}

		public int Amount { get { return amount; } }
		public int Overkill { get { return overkill; } }
		public CombatLogSpellSchool DamageSchool { get { return damageSchool; } }
		public int Resisted { get { return resisted; } }
		public int Blocked { get { return blocked; } }
		public int Absorbed { get { return absorbed; } }
		public bool Critical { get { return critical; } }
		public bool Glancing { get { return glancing; } }
		public bool Crushing { get { return crushing; } }
	}
}
