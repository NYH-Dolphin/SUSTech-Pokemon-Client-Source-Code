namespace Script.Pokemon
{
    public class Skill
    {
        private int _id;
        private string _name;
        private string _name_EN;
        private string _description;
        private string _description_EN;
        private string _genre;

        private int _allPP; // 总pp值
        private int _pp; // pp 值
        private int _hit; // 命中率
        private int _power; // 伤害

        private static HashMap<string, int> _genreMap = new HashMap<string, int>();
        public HashMap<string, int> GetGenreMap()
        {
            if (_genreMap.Count == 0)
            {
                _genreMap.Add("normal", 5);
                _genreMap.Add("grass", 11);
                _genreMap.Add("poison", 12);
                _genreMap.Add("electricity", 18);
                _genreMap.Add("fire", 16);
                _genreMap.Add("water", 1);
                _genreMap.Add("ground", 14);
                _genreMap.Add("ice", 17);
                _genreMap.Add("flying", 7);
            }
            return _genreMap;
        }
        

        // 冒险结束后重新设置PP
        public void ResetPP()
        {
            _pp = _allPP;
        }

        public int PP
        {
            get => _pp;
            set => _pp = value;
        }

        public int Hit
        {
            get => _hit;
            set => _hit = value;
        }

        public int Power
        {
            get => _power;
            set => _power = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }
        
        public string Name_EN
        {
            get => _name_EN;
            set => _name_EN = value;
        }

        public string Genre
        {
            get => _genre;
            set => _genre = value;
        }

        public Skill()
        {
            
        }

        public Skill(int id)
        {
            _id = id;
        }
        
        public Skill(int id, string name, string description, string genre, int pp, int hit, int power)
        {
            _id = id;
            _name = name;
            _description = description;
            _genre = genre;
            _pp = pp;
            _allPP = pp;
            _hit = hit;
            _power = power;
        }

        public int ID
        {
            get => _id;
            set => _id = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }
        
        public string Description_EN
        {
            get => _description_EN;
            set => _description_EN = value;
        }
    }
}