using System;

namespace Script.Skill.Skills
{
    public sealed class SkillSpark : BasicSkill
    {
        private static readonly Lazy<SkillSpark> Lazy =
            new Lazy<SkillSpark>(() => new SkillSpark());

        public static SkillSpark Instance => Lazy.Value;

        private SkillSpark() : base(SkillName.Spark, 1, Genre.Electricity, 65, 100, 20)
        {
        }
    }
}