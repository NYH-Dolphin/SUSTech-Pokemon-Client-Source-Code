using System.Collections;
using System.Collections.Generic;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public Canvas ExitCanvas;
    public Canvas PokemonCanvas;
    public Canvas SkillCanvas;
    public Image UserPortrait;
    public Image UserPokemonImg;
    public Text UserPokemonName;
    public Text UserPokemonLevel;
    public Text UserPokemonHp;

    public Image OpponentPortrait;
    public Image OpponentPokemonImg;
    public Text OpponentPokemonName;
    public Text OpponentPokemonLevel;
    public Text OpponentPokemonHp;

    public Image Pokemon1;
    public Image Pokemon2;
    public Image Pokemon3;


    public List<Text> SkillNames;
    public List<Text> SkillPPs;


    public Toggle PokemonToggle;
    public Toggle SkillToggle;


    private int currFightPokemon; // 当前出战的宝可梦
    // 1- AdventurePokemon1
    // 2- AdventurePokemon2
    // 3- AdventurePokemon3

    // Start is called before the first frame update
    void Start()
    {
        CanvasInitial();
        currFightPokemon = 1;
        UserDataSync();
        PokemonDataSync();
    }

    private void CanvasInitial()
    {
        PokemonCanvas.enabled = true;
        SkillCanvas.enabled = false;
        ExitCanvas.enabled = false;
    }


    private void PokemonDataSync()
    {
        User user = User.GetInstance();
        Pokemon curPokemon = user.AdventurePokemon1;
        switch (currFightPokemon)
        {
            case 1:
                curPokemon = user.AdventurePokemon1;
                break;
            case 2:
                curPokemon = user.AdventurePokemon2;
                break;
            case 3:
                curPokemon = user.AdventurePokemon3;
                break;
        }

        string pokemonImgPath = "Pokemon/Pixel/Back/" + curPokemon.ID;
        Sprite pokemonSprite = Resources.Load(pokemonImgPath, typeof(Sprite)) as Sprite;
        UserPokemonImg.sprite = pokemonSprite;
        UserPokemonName.text = curPokemon.Name;
        UserPokemonLevel.text = "Lv." + curPokemon.Level;
        UserPokemonHp.text = curPokemon.CurrentHp + "/" + curPokemon.Hp;


        for (int i = 0; i < 4; i++)
        {
            SkillNames[i].text = curPokemon.Skills[i].Name;
            SkillPPs[i].text = "PP: " + curPokemon.Skills[i].PP.ToString();
        }
    }

    private void UserDataSync()
    {
        User user = User.GetInstance();
        int portraitNum = user.Portrait;
        string path = "User/Portrait/p" + portraitNum;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        UserPortrait.sprite = sprite;

        SetAdventurePokemon(Pokemon1, user.AdventurePokemon1.ID);
        SetAdventurePokemon(Pokemon2, user.AdventurePokemon2.ID);
        SetAdventurePokemon(Pokemon3, user.AdventurePokemon3.ID);
    }

    public void SetAdventurePokemon(Image pokemon, int num)
    {
        string path = "Pokemon/Portrait/" + num;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        pokemon.sprite = sprite;
    }


    public void OnClickPokemon1(Toggle toggle)
    {
        if (toggle.isOn)
        {
            currFightPokemon = 1;
            SwitchPanel();
        }
    }

    public void OnClickPokemon2(Toggle toggle)
    {
        if (toggle.isOn)
        {
            currFightPokemon = 2;
            SwitchPanel();
        }
    }

    public void OnClickPokemon3(Toggle toggle)
    {
        if (toggle.isOn)
        {
            currFightPokemon = 3;
            SwitchPanel();
        }
    }


    // 用于点击宝可梦后，切换到技能页面
    private void SwitchPanel()
    {
        PokemonDataSync();
        PokemonToggle.isOn = false;
        SkillToggle.isOn = true;
        SkillCanvas.enabled = true;
        PokemonCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnClickPokemonToggle(Toggle toggle)
    {
        if (!toggle.isOn) return;
        PokemonCanvas.enabled = true;
        SkillCanvas.enabled = false;
    }

    public void OnClickSkillToggle(Toggle toggle)
    {
        if (!toggle.isOn) return;
        SkillCanvas.enabled = true;
        PokemonCanvas.enabled = false;
    }


    public void OnClickExitToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            ExitCanvas.enabled = true;
        }
    }


    public void OnClickExitConfirmBtn()
    {
        SceneManager.LoadScene("Adventure");
    }

    public void OnClickExitCancelBtn()
    {
        PokemonToggle.isOn = true;
        ExitCanvas.enabled = false;
    }
}