using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.UI;

public class PokemonSkillManager : MonoBehaviour
{
    public GameObject SkillLayout;
    public Canvas PokemonAllSkillCanvas;
    private int curPokemonShowNum = 0;
    public GameObject AllSkillLayout;


    private List<Skill> _skills; // PokemonAllSkillCanvas 中的所有技能显示


    // Start is called before the first frame update
    void Start()
    {
        PokemonDataSync();
    }

    // Update is called once per frame
    void Update()
    {
        if (curPokemonShowNum != User.GetInstance().PokemonShowNum)
        {
            PokemonDataSync();
        }
    }


    public void PokemonDataSync()
    {
        User user = User.GetInstance();
        int index = user.PokemonShowNum;
        curPokemonShowNum = index;
        Pokemon pokemon = user.Pokemons[index];

        // 清空 skillLayout内的东西，并重新设定
        int num = SkillLayout.transform.childCount;
        for (int i = 0; i < num; i++)
        {
            Destroy(SkillLayout.transform.GetChild(i).gameObject);
        }

        List<GameObject> skillBtns = new List<GameObject>();
        int btnPos = 0;
        foreach (Skill skill in pokemon.Skills)
        {
            GameObject skillBtnPrefab = Resources.Load("Item/Prefab/skill_btn") as GameObject;
            GameObject skillBtn = Instantiate(skillBtnPrefab);
            Text skillName = skillBtn.transform.GetChild(0).GetComponent<Text>();
            Image skillGenre = skillBtn.transform.GetChild(1).GetComponent<Image>();
            Text skillDescription = skillBtn.transform.GetChild(2).GetComponent<Text>();
            Text skillValue = skillBtn.transform.GetChild(3).GetComponent<Text>();
            skillName.text = PlayerPrefs.GetString("language") == "CN" ? skill.Name : skill.Name_EN;
            string genrePath = "Pokemon/Properties/" + skill.GetGenreMap().Get(skill.Genre);
            Sprite spriteGenre = Resources.Load(genrePath, typeof(Sprite)) as Sprite;
            skillGenre.sprite = spriteGenre;
            skillDescription.text =
                PlayerPrefs.GetString("language") == "CN" ? skill.Description : skill.Description_EN;
            if (PlayerPrefs.GetString("language") == "CN")
            {
                string hit = skill.Hit == 0 ? "必中" : skill.Hit + "%";
                string value = "PP: " + skill.PP + "  威力: " + skill.Power + "  命中: " + hit;
                skillValue.text = value;
            }
            else
            {
                string hit = skill.Hit == 0 ? "100%" : skill.Hit + "%";
                string value = "PP: " + skill.PP + "  Pow: " + skill.Power + "  Hit Rate: " + hit;
                skillValue.text = value;
            }
            
            var pos = btnPos;
            skillBtn.GetComponent<Button>().onClick.AddListener(() => OnClickSkillBtn(pokemon, pos));
            skillBtns.Add(skillBtn);
            btnPos++;
        }

        foreach (GameObject btn in skillBtns)
        {
            btn.transform.SetParent(SkillLayout.transform, false);
        }
    }


    private void OnClickSkillBtn(Pokemon pokemon, int btnPos)
    {
        PokemonAllSkillCanvas.enabled = true;
        // 清空 allSkillLayout内的东西，并重新设定
        int num = AllSkillLayout.transform.childCount;
        for (int i = 0; i < num; i++)
        {
            Destroy(AllSkillLayout.transform.GetChild(i).gameObject);
        }

        StartCoroutine(GetPokemonSkills(pokemon, btnPos));
    }


    public void OnClickCrossBtn()
    {
        PokemonAllSkillCanvas.enabled = false;
    }

    IEnumerator GetPokemonSkills(Pokemon pokemon, int btnPos)
    {
        WWWForm form = new WWWForm();
        form.AddField("pokemon", pokemon.ID);
        form.AddField("level", pokemon.Level);
        string url = BackEndConfig.GetUrl() + "/userPokemon/showSkills";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            _skills = new List<Skill>();
            JsonData jsonData = request.value["data"];
            for (int i = 0; i < jsonData.Count; i++)
            {
                JsonData skillData = jsonData[i];
                Skill skill = new Skill(int.Parse(skillData["id"].ToString()),
                    skillData["name"].ToString(),
                    skillData["description"].ToString(),
                    skillData["genre"].ToString(),
                    int.Parse(skillData["pp"].ToString()),
                    int.Parse(skillData["hit"].ToString()),
                    int.Parse(skillData["power"].ToString()));
                skill.Name_EN = skillData["name_en"].ToString();
                skill.Description_EN = skillData["description_en"].ToString();
                _skills.Add(skill);
            }

            List<GameObject> skillBtns = new List<GameObject>();
            foreach (Skill skill in _skills)
            {
                GameObject skillBtnPrefab = Resources.Load("Item/Prefab/skill_btn_simple") as GameObject;
                GameObject skillBtn = Instantiate(skillBtnPrefab);
                Text skillName = skillBtn.transform.GetChild(0).GetComponent<Text>();
                skillName.text = PlayerPrefs.GetString("language") == "CN" ? skill.Name : skill.Name_EN;
                skillBtns.Add(skillBtn);
                skillBtn.GetComponent<Button>().onClick
                    .AddListener(() => OnChangePokemonSkills(pokemon, skill, btnPos));

                // 如果已经选中该技能，那么禁止
                skillBtn.GetComponent<Button>().interactable = true;
                for (int i = 0; i < pokemon.Skills.Count; i++)
                {
                    if (skill.ID == pokemon.Skills[i].ID)
                    {
                        skillBtn.GetComponent<Button>().interactable = false;
                        break;
                    }
                }
            }

            float height = skillBtns.Count > 5 ? skillBtns.Count * 70 : 350;
            AllSkillLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(300, height);
            foreach (GameObject btn in skillBtns)
            {
                btn.transform.SetParent(AllSkillLayout.transform, false);
            }
        }
    }

    /**
     * [当点击显示的Btn的时候，更换宝可梦的技能，并关闭面板]
     */
    private void OnChangePokemonSkills(Pokemon pokemon, Skill skill, int btnPos)
    {
        User user = User.GetInstance();
        for (int i = 0; i < user.Pokemons.Count; i++)
        {
            if (user.Pokemons[i].ID == pokemon.ID)
            {
                Pokemon userPokemon = user.Pokemons[i];
                userPokemon.Skills[btnPos] = new Skill(skill.ID, skill.Name, skill.Description,
                    skill.Genre, skill.PP, skill.Hit, skill.Power);
                userPokemon.Skills[btnPos].Name_EN = skill.Name_EN;
                userPokemon.Skills[btnPos].Description_EN = skill.Description_EN;
            }
        }

        PokemonDataSync();
        OnClickCrossBtn();
        StartCoroutine(UploadPokemonSkills(pokemon, skill, btnPos));
    }

    /**
     * [向后端传递改变PokemonSkill]
     */
    IEnumerator UploadPokemonSkills(Pokemon pokemon, Skill skill, int btnPos)
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        form.AddField("pokemon", pokemon.ID);
        form.AddField("seq", btnPos + 1);
        form.AddField("skill", skill.ID);
        string url = BackEndConfig.GetUrl() + "/userPokemon/changeSkill";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
        }
    }
}