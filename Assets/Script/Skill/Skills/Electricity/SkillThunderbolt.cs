using System;

namespace Script.Skill.Skills
{
    public sealed class SkillThunderbolt : SpecialSkill
    {
        private static readonly Lazy<SkillThunderbolt> Lazy =
            new Lazy<SkillThunderbolt>(() => new SkillThunderbolt());

        public static SkillThunderbolt Instance => Lazy.Value;

        private SkillThunderbolt() :
            base("十万伏特", (int)SkillName.Thunderbolt, Genre.Electricity, SkillKind.Special,
                90, 100, 15, 3, State.Paralyze)
        {
        }
    }
}