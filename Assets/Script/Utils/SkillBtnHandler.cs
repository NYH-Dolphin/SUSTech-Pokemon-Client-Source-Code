using Script.Pokemon;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBtnHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject GameManager;
    public int SkillNum;
    public GameObject SkillDescription;
    private GameObject skillMessage;


    public void OnPointerEnter(PointerEventData eventData)
    {
        Pokemon pokemon = GameManager.GetComponent<FightManager>().GetCurPokemonByPos();
        GameObject skillBtnPrefab = Resources.Load("Item/Prefab/skill_show") as GameObject;
        Skill skill = pokemon.Skills[SkillNum - 1];
        skillMessage = Instantiate(skillBtnPrefab, SkillDescription.transform, false);
        Text skillName = skillMessage.transform.GetChild(0).GetComponent<Text>();
        Image skillGenre = skillMessage.transform.GetChild(1).GetComponent<Image>();
        Text skillDescription = skillMessage.transform.GetChild(2).GetComponent<Text>();
        Text skillValue = skillMessage.transform.GetChild(3).GetComponent<Text>();
        skillName.text = PlayerPrefs.GetString("language", "EN") == "CN" ? skill.Name : skill.Name_EN;
        string genrePath = "Pokemon/Properties/" + skill.GetGenreMap().Get(skill.Genre);
        Sprite spriteGenre = Resources.Load(genrePath, typeof(Sprite)) as Sprite;
        skillGenre.sprite = spriteGenre;
        skillDescription.text =
            PlayerPrefs.GetString("language", "EN") == "CN" ? skill.Description : skill.Description_EN;
        if (PlayerPrefs.GetString("language", "EN") == "CN")
        {
            string hit = skill.Hit == 0 ? "必中" : skill.Hit + "%";
            string value = "PP: " + skill.PP + "  威力: " + skill.Power + "  命中: " + hit;
            skillValue.text = value;
        }
        else
        {
            string hit = skill.Hit == 0 ? "100%" : skill.Hit + "%";
            string value = "PP: " + skill.PP + "  Pow: " + skill.Power + "  Hit: " + hit;
            skillValue.text = value;
        }
        skillMessage.GetComponent<RectTransform>().localPosition = new Vector3(0, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (skillMessage != null)
        {
            Destroy(skillMessage);
        }
    }
}