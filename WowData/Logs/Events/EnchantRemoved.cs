using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class EnchantRemoved : CombatLogEvent
	{
		string enchant;
		int itemId;
		string itemName;

		public override string Name { get { return "ENCHANT_REMOVED"; } }

		protected override void InternalPopulate( EventReader reader )
		{
			base.InternalPopulate(reader);
			enchant = reader.ReadString();
			itemId = reader.ReadInt32();
			itemName = reader.ReadString();
		}

		public override void Accept( ICombatLogEventVisitor visitor )
		{
			visitor.VisitEnchantRemoved(this);
		}

		public string Enchant { get { return enchant; } }
		public int ItemId { get { return itemId; } }
		public string ItemName { get { return itemName; } }
	}
}
