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
                case SkillName.Pound:
                    skill = SkillPound.Instance;
                    break;
                case SkillName.ThunderShock:
                    skill = SkillThunderShock.Instance;
                    break;
                case SkillName.ThunderPunch:
                    skill = SkillThunderPunch.Instance;
                    break;
                case SkillName.Thunderbolt:
                    skill = SkillThunderPunch.Instance;
                    break;
            }

            return skill;
        }
    }
}