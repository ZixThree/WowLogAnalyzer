using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs.Events
{
	public interface ICombatLogEventSpell
	{
		CombatLogSpell Spell { get; }
	}
}
