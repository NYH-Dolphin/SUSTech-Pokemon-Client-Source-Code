using System;
using Script.Skill;

namespace Script.Pokemon
{
    public abstract class Pokemon
    {
        protected int ID; // 宝可梦ID
        protected Genre Genre; //宝可梦种类

        protected HashMap<SkillName, int> SkillsSet; //宝可梦的所有技能 <技能名, 解锁等级>

        // 基础属性
        protected double Hp; //生命
        protected double Atk; //攻击
        protected double Def; //防御
        protected double SAtk; //特攻
        protected double SDef; //特防
        protected double Speed; //速度

        protected double CurrentHp; // 当前生命
        protected int Exp; // 当前经验

        protected void InitBasicProperty(double hp, double atk, double def,
            double sAtk, double sDef, double speed)
        {
            Hp = hp;
            Atk = atk;
            Def = def;
            SAtk = sAtk;
            SDef = sDef;
            Speed = speed;
        }

        public abstract string GetName(); // 获取宝可梦当前名字

        public int GetLevel() // 获取宝可梦当前等级
        {
            return (int)Math.Pow(Exp, 1.0 / 3);
        }

        public virtual double GetHp()
        {
            return Hp;
        }

        public virtual double GetAtk()
        {
            return Atk;
        }

        public virtual double GetDef()
        {
            return Def;
        }

        public virtual double GetSAtk()
        {
            return SAtk;
        }

        public virtual double GetSDef()
        {
            return SDef;
        }

        public virtual double GetSpeed()
        {
            return Speed;
        }
    }
}