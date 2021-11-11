namespace Script.Pokemon
{
    public class Skill
    {
        private int _id;
        private string _name;
        private string _description;
        private string _genre;

        public string Name
        {
            get => _name;
            set => _name = value;
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
        
        public Skill(int id, string name, string description, string genre)
        {
            _id = id;
            _name = name;
            _description = description;
            _genre = genre;
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
    }
}