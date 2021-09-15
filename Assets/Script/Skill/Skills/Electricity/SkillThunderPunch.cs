using System;

namespace Script.Skill.Skills
{
    public sealed class SkillThunderPunch : SpecialSkill
    {
        private static readonly Lazy<SkillThunderPunch> Lazy =
            new Lazy<SkillThunderPunch>(() => new SkillThunderPunch());

        public static SkillThunderPunch Instance => Lazy.Value;


        private SkillThunderPunch()
            : base("电气拳", (int)SkillName.ThunderPunch, Genre.Electricity, SkillKind.Physical,
                65, 100, 20, 3, State.Paralyze)
        {
        }
    }
}