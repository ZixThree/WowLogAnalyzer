using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs
{
    public enum LogEventName
    {
        Unknown,

        SwingDamage,
        SwingMissed,

        RangeDamage,
        RangeMissed,

        SpellDamage,
        SpellHeal,
        SpellMissed,

        SpellSummon,
        SpellDispel,
        SpellEnergize,
        SpellCreate,
        SpellInstaKill,
        SpellExtraAttacks,
        SpellResurrect,
        SpellInterrupt,
        SpellStolen,

        SpellCastSuccess,
        SpellCastStart,
        SpellCastFailed,

        SpellAuraApplied,
        SpellAuraRemoved,
        SpellAuraRefresh,
        SpellAuraAppliedDose,
        SpellAuraRemovedDose,

        SpellPeriodicDamage,
        SpellPeriodicHeal,
        SpellPeriodicEnergize,
        SpellPeriodicMissed,
        SpellPeriodicLeech,

        SpellAuraBrokenSpell,

        PartyKill,

        UnitDied,
        UnitDestroyed,

        DamageShield,
        DamageSplit,
        DamageShieldMissed,

        EnvironmentalDamage,

        EnchantApplied,
        EnchantRemoved
    }

    public class LogEvent
    {
        private IEnumerator<string> _parameters;
        private DateTime _timestamp;
        private LogEventName _name;
        private CombatLogUnit _source;
        private CombatLogUnit _destination;
        private CombatLogSpell _spell;
        private CombatLogEnvironmental _environmentalKind;
        private int _amount;
        private CombatLogSpellSchool _damageSchool;
        private int _extraAmount;
        private int _overkill;
        private int _resisted;
        private int _blocked;
        private int _absorbed;
        private bool _critical;
        private bool _glancing;
        private bool _crushing;
        private CombatLogMissKind _missKind;
        // NOTE: amountMissed -> see amount.
        // NOTE: overhealing -> see overkill
        private int _powerType;
        private CombatLogSpell _extraSpell;
        private CombatLogAuraKind _auraKind;
        private string _failMessage;
        private int _itemId;
        private string _itemName;

        public LogEvent(DateTime timestamp, string eventName, IEnumerator<string> parameters)
        {
            _parameters = parameters;
            _timestamp = timestamp;
            _name = (LogEventName)Enum.Parse(typeof(LogEventName), eventName.Replace("_", ""), true);
            _source = ReadUnit();
            _destination = ReadUnit();

            // Prefix
            switch ( _name ) {
                case LogEventName.RangeDamage: case LogEventName.RangeMissed:
                case LogEventName.SpellDamage: case LogEventName.SpellHeal:
                case LogEventName.SpellMissed:
                case LogEventName.SpellSummon: case LogEventName.SpellDispel:
                case LogEventName.SpellEnergize: case LogEventName.SpellResurrect:
                case LogEventName.SpellInterrupt: case LogEventName.SpellStolen:
                case LogEventName.SpellCastSuccess: case LogEventName.SpellCastStart: case LogEventName.SpellCastFailed:
                case LogEventName.SpellAuraApplied: case LogEventName.SpellAuraRemoved:
                case LogEventName.SpellAuraRefresh:
                case LogEventName.SpellAuraAppliedDose: case LogEventName.SpellAuraRemovedDose:
                case LogEventName.SpellPeriodicDamage: case LogEventName.SpellPeriodicHeal:
                case LogEventName.SpellPeriodicEnergize: case LogEventName.SpellPeriodicMissed:
                case LogEventName.SpellPeriodicLeech:
                case LogEventName.DamageShield: case LogEventName.DamageSplit: case LogEventName.DamageShieldMissed:
                case LogEventName.SpellCreate: case LogEventName.SpellInstaKill:
                case LogEventName.SpellExtraAttacks: case LogEventName.SpellAuraBrokenSpell:
                    _spell = ReadSpell();
                    break;

                case LogEventName.EnvironmentalDamage:
                    _environmentalKind = ReadEnum<CombatLogEnvironmental>();
                    break;
            }

            // Suffix
            switch ( _name ) {
                case LogEventName.SwingDamage:
                case LogEventName.RangeDamage:
                case LogEventName.SpellDamage:
                case LogEventName.SpellPeriodicDamage:
                case LogEventName.DamageShield:
                case LogEventName.DamageSplit:
                case LogEventName.EnvironmentalDamage:
                    _amount = ReadInt32();
                    _overkill = ReadInt32();
                    _damageSchool = (CombatLogSpellSchool)ReadUInt32();
                    _resisted = ReadInt32();
                    _blocked = ReadInt32();
                    _absorbed = ReadInt32();
                    _critical = (String.Compare(ReadString(), "nil", StringComparison.InvariantCultureIgnoreCase) != 0);
                    _glancing = (String.Compare(ReadString(), "nil", StringComparison.InvariantCultureIgnoreCase) != 0);
                    _crushing = (String.Compare(ReadString(), "nil", StringComparison.InvariantCultureIgnoreCase) != 0);
                    break;

                case LogEventName.SwingMissed:
                case LogEventName.RangeMissed:
                case LogEventName.SpellMissed:
                case LogEventName.SpellPeriodicMissed:
                case LogEventName.DamageShieldMissed:
                    _missKind = ReadEnum<CombatLogMissKind>();
                    if ( _missKind == CombatLogMissKind.Block ||
                         _missKind == CombatLogMissKind.Absorb ||
                         _missKind == CombatLogMissKind.Resist
                       ) {
                        _amount = ReadInt32();
                    }
                    break;

                case LogEventName.SpellHeal:
                    _amount = ReadInt32();
                    _overkill = ReadInt32();
                    _absorbed = ReadInt32();
                    _critical = (String.Compare(ReadString(), "nil", StringComparison.InvariantCultureIgnoreCase) != 0);
                    break;

                case LogEventName.SpellStolen:
                case LogEventName.SpellDispel:
                    _extraSpell = ReadSpell();
                    _auraKind = ReadEnum<CombatLogAuraKind>();
                    break;

                case LogEventName.SpellInterrupt:
                    _extraSpell = ReadSpell();
                    break;

                case LogEventName.SpellEnergize:
                    _amount = ReadInt32();
                    _powerType = ReadInt32();
                    break;

                case LogEventName.SpellPeriodicLeech:
                    _amount = ReadInt32();
                    _powerType = ReadInt32();
                    _extraAmount = ReadInt32();
                    break;

                case LogEventName.SpellExtraAttacks:
                    _amount = ReadInt32();
                    break;

                case LogEventName.SpellPeriodicHeal:
                    _amount = ReadInt32();
                    _overkill = ReadInt32();
                    _absorbed = ReadInt32();
                    _critical = (String.Compare(ReadString(), "nil", StringComparison.InvariantCultureIgnoreCase) != 0);
                    break;

                case LogEventName.SpellPeriodicEnergize:
                    _amount = ReadInt32();
                    _powerType = ReadInt32();
                    break;

                case LogEventName.SpellAuraAppliedDose:
                case LogEventName.SpellAuraRemovedDose:
                    _auraKind = ReadEnum<CombatLogAuraKind>();
                    _amount = ReadInt32();
                    break;

                case LogEventName.SpellAuraApplied:
                case LogEventName.SpellAuraRemoved:
                case LogEventName.SpellAuraRefresh:
                    _auraKind = ReadEnum<CombatLogAuraKind>();
                    if ( parameters.MoveNext() ) {
                        // Note: seems to be used for absorb effects.
                        _amount = ParseInt32();
                        string unknown1 = ReadString();
                        string unknown2 = ReadString();
                    }
                    break;

                case LogEventName.SpellAuraBrokenSpell:
                    _extraSpell = ReadSpell();
                    _auraKind = ReadEnum<CombatLogAuraKind>();
                    break;

                case LogEventName.SpellCastFailed:
                    _failMessage = ReadString();
                    break;

                case LogEventName.EnchantApplied:
                case LogEventName.EnchantRemoved:
                    _spell = new CombatLogSpell(-1, ReadString(), CombatLogSpellSchool.None);
                    _itemId = ReadInt32();
                    _itemName = ReadString();
                    break;
            }

            if ( parameters.MoveNext() )
                throw new ApplicationException("Additional unknown parameters for " + _name + ".");
        }


        private T ReadEnum<T>() where T : struct, IConvertible
        {
            if ( !typeof(T).IsEnum ) {
                throw new ArgumentException("T must be an enumerated type");
            }

            _parameters.MoveNext();
            return (T)Enum.Parse(typeof(T), _parameters.Current, true);
        }

        private CombatLogSpell ReadSpell()
        {
            int spellId = ReadInt32();
            string spellName = ReadString();
            CombatLogSpellSchool spellSchool = (CombatLogSpellSchool)ReadUInt32();

            return new CombatLogSpell(spellId, spellName, spellSchool);
        }

        private CombatLogUnit ReadUnit()
        {
            long guid = ReadInt64();
            string name = ReadString();
            CombatLogUnitFlags flags = (CombatLogUnitFlags)ReadUInt32();
            CombatLogRaidFlags raidFlags = (CombatLogRaidFlags)ReadUInt32();

            return new CombatLogUnit(name, guid, flags, raidFlags);
        }

        private string ReadString()
        {
            _parameters.MoveNext();
            return _parameters.Current;
        }

        private int ReadInt32()
        {
            _parameters.MoveNext();
            string value = _parameters.Current;
            if ( value.StartsWith("0x") ) {
                return Int32.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
            } else {
                return Int32.Parse(value);
            }
        }

        private int ParseInt32()
        {
            string value = _parameters.Current;
            if ( value.StartsWith("0x") ) {
                return Int32.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
            } else {
                return Int32.Parse(value);
            }
        }

        private uint ReadUInt32()
        {
            _parameters.MoveNext();
            string value = _parameters.Current;
            if ( value.StartsWith("0x") ) {
                return UInt32.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
            } else {
                return UInt32.Parse(value);
            }
        }

        private long ReadInt64()
        {
            _parameters.MoveNext();
            string value = _parameters.Current;
            if ( value.StartsWith("0x") ) {
                return Int64.Parse(value.Substring(2), System.Globalization.NumberStyles.HexNumber);
            } else {
                return Int64.Parse(value);
            }
        }


        public DateTime Timestamp { get { return _timestamp; } }
        public LogEventName Name { get { return _name; } }
        public CombatLogUnit Source { get { return _source; } }
        public CombatLogUnit Destination { get { return _destination; } }
    }
}
