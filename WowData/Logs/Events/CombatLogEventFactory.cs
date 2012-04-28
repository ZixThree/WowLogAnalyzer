using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public static class CombatLogEventFactory
	{
		public static CombatLogEvent CreateEvent( string name )
		{
			StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;
			switch(Char.ToUpperInvariant(name[0])) {
				case 'D':
					if( comparer.Equals(name, "DAMAGE_SHIELD") )
						return new DamageShield();
					if( comparer.Equals(name, "DAMAGE_SHIELD_MISSED") )
						return new DamageShieldMissed();
					if( comparer.Equals(name, "DAMAGE_SPLIT") )
						return new DamageSplit();
					break;
				case 'E':
					if( comparer.Equals(name, "ENCHANT_APPLIED") )
						return new EnchantApplied();
					if( comparer.Equals(name, "ENCHANT_REMOVED") )
						return new EnchantRemoved();
					if( comparer.Equals(name, "ENVIRONMENTAL_DAMAGE") )
						return new EnvironmentalDamage();
					break;
				case 'P':
					if( comparer.Equals(name, "PARTY_KILL") )
						return new PartyKill();
					break;
				case 'R':
					if( comparer.Equals(name, "RANGE_DAMAGE") )
						return new RangeDamage();
					if( comparer.Equals(name, "RANGE_MISSED") )
						return new RangeMissed();
					break;
				case 'S':
					if( Char.ToUpperInvariant(name[1]) == 'W' ) {
						if( comparer.Equals(name, "SWING_DAMAGE") )
							return new SwingDamage();
						if( comparer.Equals(name, "SWING_MISSED") )
							return new SwingMissed();
					} else {
						switch( Char.ToUpperInvariant(name[6]) ) {
							case 'A':
								if( comparer.Equals(name, "SPELL_AURA_APPLIED") )
									return new SpellAuraApplied();
								if( comparer.Equals(name, "SPELL_AURA_APPLIED_DOSE") )
									return new SpellAuraAppliedDose();
								if( comparer.Equals(name, "SPELL_AURA_BROKEN_SPELL") )
									return new SpellAuraBrokenSpell();
								if( comparer.Equals(name, "SPELL_AURA_REFRESH") )
									return new SpellAuraRefresh();
								if( comparer.Equals(name, "SPELL_AURA_REMOVED") )
									return new SpellAuraRemoved();
								if( comparer.Equals(name, "SPELL_AURA_REMOVED_DOSE") )
									return new SpellAuraRemovedDose();
								break;
							case 'C':
								if( comparer.Equals(name, "SPELL_CAST_FAILED") )
									return new SpellCastFailed();
								if( comparer.Equals(name, "SPELL_CAST_START") )
									return new SpellCastStart();
								if( comparer.Equals(name, "SPELL_CAST_SUCCESS") )
									return new SpellCastSuccess();
								if( comparer.Equals(name, "SPELL_CREATE") )
									return new SpellCreate();
								break;
							case 'D':
								if( comparer.Equals(name, "SPELL_DAMAGE") )
									return new SpellDamage();
								if( comparer.Equals(name, "SPELL_DISPEL") )
									return new SpellDispel();
								if( comparer.Equals(name, "SPELL_DRAIN") )
									return new SpellDrain();
								break;
							case 'E':
								if( comparer.Equals(name, "SPELL_ENERGIZE") )
									return new SpellEnergize();
								if( comparer.Equals(name, "SPELL_EXTRA_ATTACKS") )
									return new SpellExtraAttacks();
								break;
							case 'H':
								if( comparer.Equals(name, "SPELL_HEAL") )
									return new SpellHeal();
								break;
							case 'I':
								if( comparer.Equals(name, "SPELL_INSTAKILL") )
									return new SpellInstakill();
								if( comparer.Equals(name, "SPELL_INTERRUPT") )
									return new SpellInterrupt();
								break;
							case 'M':
								if( comparer.Equals(name, "SPELL_MISSED") )
									return new SpellMissed();
								break;
							case 'P':
								if( comparer.Equals(name, "SPELL_PERIODIC_DAMAGE") )
									return new SpellPeriodicDamage();
								if( comparer.Equals(name, "SPELL_PERIODIC_DRAIN") )
									return new SpellPeriodicDrain();
								if( comparer.Equals(name, "SPELL_PERIODIC_ENERGIZE") )
									return new SpellPeriodicEnergize();
								if( comparer.Equals(name, "SPELL_PERIODIC_HEAL") )
									return new SpellPeriodicHeal();
								if( comparer.Equals(name, "SPELL_PERIODIC_LEECH") )
									return new SpellPeriodicLeech();
								if( comparer.Equals(name, "SPELL_PERIODIC_MISSED") )
									return new SpellPeriodicMissed();
								break;
							case 'R':
								if( comparer.Equals(name, "SPELL_RESURRECT") )
									return new SpellResurrect();
								break;
							case 'S':
								if( comparer.Equals(name, "SPELL_STOLEN") )
									return new SpellStolen();
								if( comparer.Equals(name, "SPELL_SUMMON") )
									return new SpellSummon();
								break;
						}
					}
					break;
				case 'U':
					if( comparer.Equals(name, "UNIT_DESTROYED") )
						return new UnitDestroyed();
					if( comparer.Equals(name, "UNIT_DIED") )
						return new UnitDied();
					break;
			}

			throw new NotSupportedException(String.Format("{0} is not a supported event.", name));
		}
	}
}
