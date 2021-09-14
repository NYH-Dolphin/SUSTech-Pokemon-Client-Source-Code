using Script.Skill;

namespace Script.Pokemon.Pokemons
{
    public class Pichu : ThreeOrderPokemon
    {
        public Pichu()
        {
            ID = 1;
            Genre = Genre.Electricity;
            InitName("Pichu", "Pikachu", "Raichu");
            InitBasicProperty(20, 40, 15, 35, 35, 60);
            SkillsSet = new HashMap<SkillName, int>();
            SkillsSet.Add(SkillName.Spark, 0);
        }
    }
}