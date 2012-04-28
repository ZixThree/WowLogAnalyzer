using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public class EventReader
	{
		private IEnumerator<string> parameters;
		private string nextValue = null;

		public EventReader( IEnumerator<string> parameters )
		{
			this.parameters = parameters;
		}

		public bool HasNextValue
		{
			get
			{
				if( nextValue == null && parameters.MoveNext() ) {
					nextValue = parameters.Current;
				}
				return nextValue != null;
			}
		}

		private string GetNext( )
		{
			string result;
			if( nextValue != null ) {
				result = nextValue;
				nextValue = null;
			} else {
				parameters.MoveNext();
				result = parameters.Current;
			}
			return result;
		}

		public T ReadEnum<T>( ) where T : struct, IConvertible
		{
			if( !typeof(T).IsEnum ) {
				throw new ArgumentException("T must be an enumerated type");
			}

			return (T)Enum.Parse(typeof(T), GetNext(), true);
		}

		public CombatLogSpell ReadSpell( )
		{
			int spellId = ReadInt32();
			string spellName = GetNext();
			CombatLogSpellSchool spellSchool = (CombatLogSpellSchool)ReadUInt32();

			return new CombatLogSpell(spellId, spellName, spellSchool);
		}

		public CombatLogUnit ReadUnit( )
		{
			long guid = ReadInt64();
			string name = GetNext();
			CombatLogUnitFlags flags = (CombatLogUnitFlags)ReadUInt32();
			CombatLogRaidFlags raidFlags = (CombatLogRaidFlags)ReadUInt32();

			return new CombatLogUnit(name, guid, flags, raidFlags);
		}

		public string ReadString( )
		{
			string next = GetNext();
			if( next.Equals("nil") )
				return null;
			return next;
		}

		public int ReadInt32( )
		{
			string value = GetNext();
			if( value.StartsWith("0x") ) {
				return Int32.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
			} else {
				return Int32.Parse(value);
			}
		}

		//private int ParseInt32( )
		//{
		//    string value = parameters.Current;
		//    if( value.StartsWith("0x") ) {
		//        return Int32.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
		//    } else {
		//        return Int32.Parse(value);
		//    }
		//}

		public uint ReadUInt32( )
		{
			string value = GetNext();
			if( value.StartsWith("0x") ) {
				return UInt32.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
			} else {
				return UInt32.Parse(value);
			}
		}

		public long ReadInt64( )
		{
			string value = GetNext();
			if( value.StartsWith("0x") ) {
				return Int64.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
			} else {
				return Int64.Parse(value);
			}
		}

	}
}
