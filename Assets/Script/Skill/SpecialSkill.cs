namespace Script.Skill
{
    public class SpecialSkill : BasicSkill
    {
        private int _duration; //技能时效
        private State _state; //技能追加效果


        protected SpecialSkill(string name, int id, Genre genre, SkillKind kind, int power, int hitRate, int pp,
            int duration, State state) : base(name, id, genre, kind, power, hitRate, pp)
        {
            _duration = duration;
            _state = state;
        }
    }
}