using Script.Skill;

namespace Script.Pokemon.Pokemons
{
    public class Pichu : ThreeOrderPokemon
    {
        public Pichu()
        {
            BasicSetting();
        }

        public sealed override void BasicSetting()
        {
            ID = 1;
            Genre = Genre.Electricity;
            InitName("皮丘", "皮卡丘", "雷丘");
            InitBasicProperty(20, 40, 15, 35, 35, 60);
            SkillsSet = new HashMap<SkillName, int>();
            SkillsSet.Add(SkillName.Pound, 0);
            SkillsSet.Add(SkillName.ThunderShock, 0);
            SkillsSet.Add(SkillName.ThunderPunch, 0);
            SkillsSet.Add(SkillName.Thunderbolt, 0);
        }
    }
}