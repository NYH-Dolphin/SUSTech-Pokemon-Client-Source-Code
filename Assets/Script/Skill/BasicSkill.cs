using Google.Protobuf.WellKnownTypes;

namespace Script.Skill
{
    public class BasicSkill
    {
        private readonly string _name;
        private readonly int _id;
        private readonly Genre _genre;
        private readonly SkillKind _kind;
        private readonly int _power;
        private readonly int _hitRate;
        private readonly int _pp;

        protected BasicSkill(string name, int id, Genre genre, SkillKind kind, int power, int hitRate, int pp)
        {
            _name = name;
            _id = id;
            _genre = genre;
            _kind = kind;
            _power = power;
            _hitRate = hitRate;
            _pp = pp;
        }

        public string GetName()
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

        public SkillKind GetSkillType()
        {
            return _kind;
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