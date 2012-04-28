using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WowLogAnalyzer.Wow.Logs.Events;

namespace WowLogAnalyzer.Wow.Logs
{
	public class ReverseCombatLogVisitor : Events.ICombatLogEventVisitor
	{
		TextWriter writer;

		public ReverseCombatLogVisitor( Stream stream )
		{
			writer = new StreamWriter(stream, Encoding.UTF8);
		}

		public ReverseCombatLogVisitor( TextWriter textWriter )
		{
			writer = textWriter;
		}

		private void WriteCombatLogEvent( CombatLogEvent e )
		{
			writer.Write("{0}/{1} {2:00}:{3:00}:{4:00}.{5:000}  ",
				e.TimeStamp.Month, e.TimeStamp.Day,
				e.TimeStamp.Hour, e.TimeStamp.Minute, e.TimeStamp.Second, e.TimeStamp.Millisecond);
			writer.Write("{0},0x{1:X16},{2},0x{3:x},0x{4:x},0x{5:X16},{6},0x{7:x},0x{8:x}",
				e.Name,
				e.Source.Id, e.Source.Id != 0 ? '"' + e.Source.Name + '"' : "nil",
				(uint)e.Source.Flags, (uint)e.Source.RaidFlags,
				e.Destination.Id, e.Destination.Id != 0 ? '"' + e.Destination.Name + '"' : "nil",
				(uint)e.Destination.Flags, (uint)e.Destination.RaidFlags
				);
		}

		private void WriteSpell( CombatLogSpell spell )
		{
			writer.Write(",{0},\"{1}\",0x{2:x}", spell.Id, spell.Name, (uint)spell.School);
		}

		private void WriteExtraSpell( CombatLogSpell spell )
		{
			writer.Write(",{0},\"{1}\",{2}", spell.Id, spell.Name, (uint)spell.School);
		}

		private void WriteDamage( CombatLogEventDamage e )
		{
			writer.Write(",{0},{1},{2},{3},{4},{5},{6},{7},{8}",
				e.Amount, e.Overkill, (uint)e.DamageSchool, e.Resisted, e.Blocked, e.Absorbed,
				(e.Critical ? "1" : "nil"), (e.Glancing ? "1" : "nil"), (e.Crushing ? "1" : "nil"));
		}

		private void WriteMissed( CombatLogEventMissed e )
		{
			bool validAmount = false;
			validAmount |= e.MissKind == CombatLogMissKind.Block;
			validAmount |= e.MissKind == CombatLogMissKind.Absorb;
			validAmount |= e.MissKind == CombatLogMissKind.Resist;
			if( validAmount )
				writer.Write(",{0},{1}", e.MissKind.ToString().ToUpperInvariant(), e.MissAmount);
			else
				writer.Write(",{0}", e.MissKind.ToString().ToUpperInvariant(), e.MissAmount);
		}

		private void WriteAura( CombatLogEventAura e, bool extraParameters )
		{
			if( extraParameters || e.Amount != 0 )
				writer.Write(",{0},{1}", e.AuraKind.ToString().ToUpperInvariant(), e.Amount);
			else
				writer.Write(",{0}", e.AuraKind.ToString().ToUpperInvariant());
		}

		private void WriteDrain( CombatLogEventDrain e )
		{
			writer.Write(",{0},{1},{2}", e.Amount, e.PowerType, e.ExtraAmount);
		}

		private void WriteEnergize( CombatLogEventEnergize e )
		{
			writer.Write(",{0},{1}", e.Amount, e.PowerType);
		}

		private void WriteHeal( CombatLogEventHeal e )
		{
			writer.Write(",{0},{1},{2},{3}",
				e.Amount, e.Overkill, e.Absorbed, e.Critical ? "1" : "nil");
		}

		#region ICombatLogEventVisitor Members

		public void VisitDamageShield( Events.DamageShield e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteDamage(e);
			writer.Flush();
		}

		public void VisitDamageShieldMissed( Events.DamageShieldMissed e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteMissed(e);
			writer.Flush();
		}

		public void VisitDamageSplit( Events.DamageSplit e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteDamage(e);
			writer.Flush();
		}

		public void VisitEnchantApplied( Events.EnchantApplied e )
		{
			WriteCombatLogEvent(e);
			writer.Write(",\"{0}\",{1},\"{2}\"", e.Enchant, e.ItemId, e.ItemName);
			writer.Flush();
		}

		public void VisitEnchantRemoved( Events.EnchantRemoved e )
		{
			WriteCombatLogEvent(e);
			writer.Write(",\"{0}\",{1},\"{2}\"", e.Enchant, e.ItemId, e.ItemName);
			writer.Flush();
		}

		public void VisitEnvironmentalDamage( Events.EnvironmentalDamage e )
		{
			WriteCombatLogEvent(e);
			writer.Write(",{0}", e.EnvironmentKind.ToString().ToUpperInvariant());
			WriteDamage(e);
			writer.Flush();
		}

		public void VisitPartyKill( Events.PartyKill e )
		{
			WriteCombatLogEvent(e);
			writer.Flush();
		}

		public void VisitRangeDamage( Events.RangeDamage e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteDamage(e);
			writer.Flush();
		}

		public void VisitRangeMissed( Events.RangeMissed e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteMissed(e);
			writer.Flush();
		}

		public void VisitSpellAuraApplied( Events.SpellAuraApplied e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteAura(e, e.Unknown1 != null);
			if( e.Unknown1 != null ) {
				writer.Write(",{0},{1}", e.Unknown1, e.Unknown2);
			}
			writer.Flush();
		}

		public void VisitSpellAuraAppliedDose( Events.SpellAuraAppliedDose e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteAura(e, false);
			writer.Flush();
		}

		public void VisitSpellAuraBrokenSpell( Events.SpellAuraBrokenSpell e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteExtraSpell(e.ExtraSpell);
			WriteAura(e, false);
			writer.Flush();
		}

		public void VisitSpellAuraRefresh( Events.SpellAuraRefresh e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteAura(e, e.Unknown1 != null);
			if( e.Unknown1 != null ) {
				writer.Write(",{0},{1}", e.Unknown1, e.Unknown2);
			}
			writer.Flush();
		}

		public void VisitSpellAuraRemoved( Events.SpellAuraRemoved e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteAura(e, e.Unknown1 != null);
			if( e.Unknown1 != null ) {
				writer.Write(",{0},{1}", e.Unknown1, e.Unknown2);
			}
			writer.Flush();
		}

		public void VisitSpellAuraRemovedDose( Events.SpellAuraRemovedDose e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteAura(e, false);
			writer.Flush();
		}

		public void VisitSpellCastFailed( Events.SpellCastFailed e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			writer.Write(",\"{0}\"", e.Reason);
			writer.Flush();
		}

		public void VisitSpellCastStart( Events.SpellCastStart e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			writer.Flush();
		}

		public void VisitSpellCastSuccess( Events.SpellCastSuccess e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			writer.Flush();
		}

		public void VisitSpellCreate( Events.SpellCreate e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			writer.Flush();
		}

		public void VisitSpellDamage( Events.SpellDamage e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteDamage(e);
			writer.Flush();
		}

		public void VisitSpellDispel( Events.SpellDispel e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteExtraSpell(e.ExtraSpell);
			writer.Write(",{0}", e.AuraKind.ToString().ToUpperInvariant());
			writer.Flush();
		}

		public void VisitSpellDrain( Events.SpellDrain e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteDrain(e);
			writer.Flush();
		}

		public void VisitSpellEnergize( Events.SpellEnergize e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteEnergize(e);
			writer.Flush();
		}

		public void VisitSpellExtraAttacks( Events.SpellExtraAttacks e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			writer.Write(",{0}", e.Amount);
			writer.Flush();
		}

		public void VisitSpellHeal( Events.SpellHeal e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteHeal(e);
			writer.Flush();
		}

		public void VisitSpellInstakill( Events.SpellInstakill e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			writer.Flush();
		}

		public void VisitSpellInterrupt( Events.SpellInterrupt e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteExtraSpell(e.ExtraSpell);
			writer.Flush();
		}

		public void VisitSpellMissed( Events.SpellMissed e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteMissed(e);
			writer.Flush();
		}

		public void VisitSpellPeriodicDamage( Events.SpellPeriodicDamage e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteDamage(e);
			writer.Flush();
		}

		public void VisitSpellPeriodicDrain( Events.SpellPeriodicDrain e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteDrain(e);
			writer.Flush();
		}

		public void VisitSpellPeriodicEnergize( Events.SpellPeriodicEnergize e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteEnergize(e);
			writer.Flush();
		}

		public void VisitSpellPeriodicHeal( Events.SpellPeriodicHeal e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteHeal(e);
			writer.Flush();
		}

		public void VisitSpellPeriodicLeech( Events.SpellPeriodicLeech e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteDrain(e);
			writer.Flush();
		}

		public void VisitSpellPeriodicMissed( Events.SpellPeriodicMissed e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteMissed(e);
			writer.Flush();
		}

		public void VisitSpellResurrect( Events.SpellResurrect e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			writer.Flush();
		}

		public void VisitSpellStolen( Events.SpellStolen e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			WriteExtraSpell(e.ExtraSpell);
			writer.Write(",{0}", e.AuraKind.ToString().ToUpperInvariant());
			writer.Flush();
		}

		public void VisitSpellSummon( Events.SpellSummon e )
		{
			WriteCombatLogEvent(e);
			WriteSpell(e.Spell);
			writer.Flush();
		}

		public void VisitSwingDamage( Events.SwingDamage e )
		{
			WriteCombatLogEvent(e);
			WriteDamage(e);
			writer.Flush();
		}

		public void VisitSwingMissed( Events.SwingMissed e )
		{
			WriteCombatLogEvent(e);
			WriteMissed(e);
			writer.Flush();
		}

		public void VisitUnitDestroyed( Events.UnitDestroyed e )
		{
			WriteCombatLogEvent(e);
			writer.Flush();
		}

		public void VisitUnitDied( Events.UnitDied e )
		{
			WriteCombatLogEvent(e);
			writer.Flush();
		}

		#endregion
	}
}
