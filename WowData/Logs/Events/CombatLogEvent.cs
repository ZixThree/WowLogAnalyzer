using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public abstract class CombatLogEvent
	{
		protected DateTime timeStamp;
		protected CombatLogUnit source;
		protected CombatLogUnit destination;

		protected CombatLogEvent( ) {
		}

		public void Populate( DateTime timeStamp, EventReader reader )
		{
			this.timeStamp = timeStamp;
			this.source = reader.ReadUnit();
			this.destination = reader.ReadUnit();
			InternalPopulate(reader);
		}

		protected virtual void InternalPopulate( EventReader reader )
		{
		}

		public abstract void Accept( ICombatLogEventVisitor visitor );

		public abstract string Name { get; }
		public DateTime TimeStamp { get { return timeStamp; } }
		public CombatLogUnit Source { get { return source; } }
		public CombatLogUnit Destination { get { return destination; } }
	}
}
