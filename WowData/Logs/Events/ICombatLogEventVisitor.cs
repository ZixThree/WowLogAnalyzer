using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public interface ICombatLogEventVisitor
	{
		void VisitDamageShield( DamageShield e );
		void VisitDamageShieldMissed( DamageShieldMissed e );
		void VisitDamageSplit( DamageSplit e );
		void VisitEnchantApplied( EnchantApplied e );
		void VisitEnchantRemoved( EnchantRemoved e );
		void VisitEnvironmentalDamage( EnvironmentalDamage e );
		void VisitPartyKill( PartyKill e );
		void VisitRangeDamage( RangeDamage e );
		void VisitRangeMissed( RangeMissed e );
		void VisitSpellAuraApplied( SpellAuraApplied e );
		void VisitSpellAuraAppliedDose( SpellAuraAppliedDose e );
		void VisitSpellAuraBrokenSpell( SpellAuraBrokenSpell e );
		void VisitSpellAuraRefresh( SpellAuraRefresh e );
		void VisitSpellAuraRemoved( SpellAuraRemoved e );
		void VisitSpellAuraRemovedDose( SpellAuraRemovedDose e );
		void VisitSpellCastFailed( SpellCastFailed e );
		void VisitSpellCastStart( SpellCastStart e );
		void VisitSpellCastSuccess( SpellCastSuccess e );
		void VisitSpellCreate( SpellCreate e );
		void VisitSpellDamage( SpellDamage e );
		void VisitSpellDispel( SpellDispel e );
		void VisitSpellDrain( SpellDrain e );
		void VisitSpellEnergize( SpellEnergize e );
		void VisitSpellExtraAttacks( SpellExtraAttacks e );
		void VisitSpellHeal( SpellHeal e );
		void VisitSpellInstakill( SpellInstakill e );
		void VisitSpellInterrupt( SpellInterrupt e );
		void VisitSpellMissed( SpellMissed e );
		void VisitSpellPeriodicDamage( SpellPeriodicDamage e );
		void VisitSpellPeriodicDrain( SpellPeriodicDrain e );
		void VisitSpellPeriodicEnergize( SpellPeriodicEnergize e );
		void VisitSpellPeriodicHeal( SpellPeriodicHeal e );
		void VisitSpellPeriodicLeech( SpellPeriodicLeech e );
		void VisitSpellPeriodicMissed( SpellPeriodicMissed e );
		void VisitSpellResurrect( SpellResurrect e );
		void VisitSpellStolen( SpellStolen e );
		void VisitSpellSummon( SpellSummon e );
		void VisitSwingDamage( SwingDamage e );
		void VisitSwingMissed( SwingMissed e );
		void VisitUnitDestroyed( UnitDestroyed e );
		void VisitUnitDied( UnitDied e );

	}
}
