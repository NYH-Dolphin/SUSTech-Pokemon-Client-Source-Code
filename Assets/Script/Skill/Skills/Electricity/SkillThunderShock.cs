using System;

namespace Script.Skill.Skills
{
    public sealed class SkillThunderShock : SpecialSkill
    {
        private static readonly Lazy<SkillThunderShock> Lazy =
            new Lazy<SkillThunderShock>(() => new SkillThunderShock());

        public static SkillThunderShock Instance => Lazy.Value;

        private SkillThunderShock() :
            base("电击", (int)SkillName.ThunderShock, Genre.Electricity, SkillKind.Special,
                40, 100, 30, 3, State.Paralyze)
        {
        }
    }
}