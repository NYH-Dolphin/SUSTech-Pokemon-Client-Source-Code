using System;
using Script.Skill;

namespace Script.Pokemon
{
    public class Pokemon
    {
        private int _id; // 宝可梦ID
        private Genre _genre; //宝可梦种类
        private String _name;
        private HashMap<SkillName, int> _skillsSet; //宝可梦的所有技能 <技能名, 解锁等级>

        private int _rarity; // 稀有度

        // 基础属性
        private double _hp; // 基础生命
        private double _atk; // 基础攻击
        private double _def; // 基础防御
        private double _satk; // 基础特攻
        private double _sdef; // 基础特防
        private double _speed; // 基础速度

        private double _currentHp; // 当前生命
        private int _currentExp; // 当前经验

        public Pokemon(int id)
        {
            _id = id;
        }

        public Pokemon()
        {
        }

        public int ID
        {
            get => _id;
            set => _id = value;
        }

        public Genre Genre
        {
            get => _genre;
            set => _genre = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public HashMap<SkillName, int> SkillsSet
        {
            get => _skillsSet;
            set => _skillsSet = value;
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