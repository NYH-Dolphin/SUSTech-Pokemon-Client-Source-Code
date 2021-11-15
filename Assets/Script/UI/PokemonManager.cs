using System.Collections;
using System.Collections.Generic;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PokemonManager : MonoBehaviour
{
    public List<Canvas> CanvasList;
    public List<Toggle> GridList;
    public Canvas PokemonAllSkillCanvas;
    public Text pokemonNameAndLevel;
    private int pageNum;


    // Start is called before the first frame update
    void Start()
    {
        PokemonAllSkillCanvas.enabled = false;
        // 先把第0个Canvas设置显示
        EnableCanvas(0);
        UserDataSync();
        PokemonDataSync();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnClickNextPage()
    {
        if (pageNum < User.GetInstance().Pokemons.Count / 9)
            pageNum++;
        GridList[0].isOn = true;
        UserDataSync();
        PokemonDataSync();
    }

    public void OnClickPrevPage()
    {
        pageNum = pageNum > 0 ? pageNum - 1 : pageNum;
        GridList[0].isOn = true;
        UserDataSync();
        PokemonDataSync();
    }

    private void UserDataSync()
    {
        int index = 9 * pageNum;
        User user = User.GetInstance();
        user.PokemonShowNum = index;
        foreach (Toggle grid in GridList)
        {
            Image pokemonImg = grid.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            string path;
            if (index < user.Pokemons.Count)
            {
                grid.interactable = true;
                path = "Pokemon/Portrait/" + user.Pokemons[index].ID;
            }
            else
            {
                grid.interactable = false;
                path = "Pokemon/Portrait/0";
            }

            Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            pokemonImg.sprite = sprite;
            index++;
        }
    }


    private void PokemonDataSync()
    {
        User user = User.GetInstance();
        int index = user.PokemonShowNum;
        Pokemon pokemon = user.Pokemons[index];
        pokemonNameAndLevel.text = pokemon.Name + " Lv." + pokemon.Level + "/100";
    }


    /**
     * [点击宝可梦头像的toggle时，会更新user的PokemonShowNum,以确定User在查看哪个宝可梦的信息]
     */
    public void OnClickGridToggle(Toggle grid)
    {
        if (grid.isOn)
        {
            int gridNum = int.Parse(grid.name.Replace("grid", ""));
            User.GetInstance().PokemonShowNum = pageNum * 9 + gridNum;
            PokemonDataSync();
        }
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }


    // 切换某个Canvas显示的状态
    private void EnableCanvas(int num)
    {
        for (int i = 0; i < CanvasList.Count; i++)
        {
            CanvasList[i].GetComponent<Canvas>().sortingOrder = i == num ? 1 : -1;
        }
    }

    public void IntroToggle()
    {
        EnableCanvas(0);
    }

    public void UpgradeToggle()
    {
        EnableCanvas(1);
    }

    public void Evolution_Canvas()
    {
        EnableCanvas(2);
    }

    public void Skill_Canvas()
    {
        EnableCanvas(3);
    }

    public void PotentialCanvas()
    {
        EnableCanvas(4);
    }
}