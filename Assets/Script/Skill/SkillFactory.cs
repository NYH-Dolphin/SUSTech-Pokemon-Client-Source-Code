using Script.Skill.Skills;

namespace Script.Skill
{
    public class SkillFactory
    {
        public static BasicSkill CreateSkill(SkillName skillName)
        {
            BasicSkill skill = null;
            switch (skillName)
            {
                case SkillName.Spark:
                    skill = SkillSpark.Instance;
                    break;
            }

            return skill;
        }
    }
}