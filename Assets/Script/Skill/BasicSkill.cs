namespace Script.Skill
{
    public class BasicSkill
    {
        private readonly SkillName _name;
        private readonly int _id;
        private readonly Genre _genre;
        private readonly int _power;
        private readonly int _hitRate;
        private readonly int _pp;

        protected BasicSkill(SkillName name, int id, Genre genre, int power, int hitRate, int pp)
        {
            _name = name;
            _id = id;
            _genre = genre;
            _power = power;
            _hitRate = hitRate;
            _pp = pp;
        }

        public SkillName GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public Genre GetGenre()
        {
            return _genre;
        }

        public int GetPower()
        {
            return _power;
        }

        public int GetHitRate()
        {
            return _hitRate;
        }

        public int GetPp()
        {
            return _pp;
        }
    }
}