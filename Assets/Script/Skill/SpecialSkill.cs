namespace Script.Skill
{
    public class SpecialSkill : BasicSkill
    {
        private int _duration; //技能时效
        private State _state;


        protected SpecialSkill(SkillName name, int id, Genre genre, int power, int hitRate, int pp, int duration,
            State state) : base(name, id, genre, power, hitRate, pp)
        {
            _duration = duration;
            _state = state;
        }
    }
}