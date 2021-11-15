using System;
using System.Collections.Generic;
using Script.Skill;
using UnityEngine;

namespace Script.Pokemon
{
    public class Pokemon
    {
        private int _id; // 宝可梦ID
        private string _genre; //宝可梦种类
        private String _name;
        private List<Skill> _skills = new List<Skill>(); // 宝可梦的技能
        private int _rarity; // 稀有度
        private bool _isDeprecated; // 是否弃用

        // 基础属性
        private double _hp; // 基础生命
        private double _atk; // 基础攻击
        private double _def; // 基础防御
        private double _satk; // 基础特攻
        private double _sdef; // 基础特防
        private double _speed; // 基础速度

        private double _currentHp; // 当前生命
        private int _currentExp; // 当前经验
        private int _level; // 当前等级
        private int _potential; // 当前是几命

        private static HashMap<string, int> _genreMap = new HashMap<string, int>();

        public bool IsDeprecated
        {
            get => _isDeprecated;
            set => _isDeprecated = value;
        }

        public HashMap<string, int> GetGenreMap()
        {
            if (_genreMap.Count == 0)
            {
                _genreMap.Add("normal", 5);
                _genreMap.Add("grass", 11);
                _genreMap.Add("poison", 12);
                _genreMap.Add("electricity", 18);
                _genreMap.Add("fire", 16);
                _genreMap.Add("water", 1);
                _genreMap.Add("ground", 14);
                _genreMap.Add("ice", 17);
                _genreMap.Add("flying", 7);
            }
            return _genreMap;
        }


        public int Potential
        {
            get => _potential;
            set => _potential = value;
        }

        public Pokemon(int id)
        {
            _id = id;
        }

        public Pokemon()
        {
        }


        public int Level
        {
            get => _level;
            set => _level = value;
        }


        public List<Skill> Skills
        {
            get => _skills;
            set => _skills = value;
        }

        public int ID
        {
            get => _id;
            set => _id = value;
        }

        public string Genre
        {
            get => _genre;
            set => _genre = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }


        public int Rarity
        {
            get => _rarity;
            set => _rarity = value;
        }

        public double Hp
        {
            get => _hp;
            set => _hp = value;
        }

        public double Atk
        {
            get => _atk;
            set => _atk = value;
        }

        public double Def
        {
            get => _def;
            set => _def = value;
        }

        public double Satk
        {
            get => _satk;
            set => _satk = value;
        }

        public double Sdef
        {
            get => _sdef;
            set => _sdef = value;
        }

        public double Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public double CurrentHp
        {
            get => _currentHp;
            set => _currentHp = value;
        }

        public int CurrentExp
        {
            get => _currentExp;
            set => _currentExp = value;
        }
    }
}