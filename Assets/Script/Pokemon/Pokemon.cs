using System;
using Script.Skill;

namespace Script.Pokemon
{
    public class Pokemon
    {
        protected int id; // 宝可梦ID
        protected Genre genre; //宝可梦种类
        protected String name;
        protected HashMap<SkillName, int> skillsSet; //宝可梦的所有技能 <技能名, 解锁等级>

        protected int rarity; // 稀有度

        // 基础属性
        protected double hp; // 基础生命
        protected double atk; // 基础攻击
        protected double def; // 基础防御
        protected double satk; // 基础特攻
        protected double sdef; // 基础特防
        protected double speed; // 基础速度

        protected double currentHp; // 当前生命
        protected int currentExp; // 当前经验

        public int ID
        {
            get => id;
            set => id = value;
        }

        public Genre Genre
        {
            get => genre;
            set => genre = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public HashMap<SkillName, int> SkillsSet
        {
            get => skillsSet;
            set => skillsSet = value;
        }

        public int Rarity
        {
            get => rarity;
            set => rarity = value;
        }

        public double Hp
        {
            get => hp;
            set => hp = value;
        }

        public double Atk
        {
            get => atk;
            set => atk = value;
        }

        public double Def
        {
            get => def;
            set => def = value;
        }

        public double Satk
        {
            get => satk;
            set => satk = value;
        }

        public double Sdef
        {
            get => sdef;
            set => sdef = value;
        }

        public double Speed
        {
            get => speed;
            set => speed = value;
        }

        public double CurrentHp
        {
            get => currentHp;
            set => currentHp = value;
        }

        public int CurrentExp
        {
            get => currentExp;
            set => currentExp = value;
        }
    }
}