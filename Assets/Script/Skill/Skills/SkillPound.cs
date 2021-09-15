using System;

namespace Script.Skill.Skills
{
    public sealed class SkillPound : BasicSkill
    {
        private static readonly Lazy<SkillPound> Lazy =
            new Lazy<SkillPound>(() => new SkillPound());

        public static SkillPound Instance => Lazy.Value;

        private SkillPound() :
            base("拍击", (int)SkillName.Pound, Genre.Normal, SkillKind.Physical,
                40, 100, 35)
        {
        }
    }
}