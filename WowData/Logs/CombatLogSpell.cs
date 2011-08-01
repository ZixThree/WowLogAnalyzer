using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WowLogAnalyzer.Wow.Logs
{
    public class CombatLogSpell
    {
        private readonly int _id;
        private readonly string _name;
        private readonly CombatLogSpellSchool _school;

        public CombatLogSpell(int id, string name, CombatLogSpellSchool school)
        {
            _id = id;
            _name = name;
            _school = school;
        }

        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        public CombatLogSpellSchool School { get { return _school; } }
    }
}
