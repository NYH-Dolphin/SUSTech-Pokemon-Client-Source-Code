using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PokemonManager : MonoBehaviour
{
    public List<Canvas> CanvasList;
    public List<Toggle> GridList;

    private int pageNum;


    // Start is called before the first frame update
    void Start()
    {
        UserDataSync();
        // 先把第0个Canvas设置显示
        CanvasList[0].enabled = true;
        for (int i = 1; i < CanvasList.Count; i++)
            CanvasList[i].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnClickNextPage()
    {
        if (pageNum < User.GetInstance().Pokemons.Count / 9)
            pageNum++;

        UserDataSync();
    }

    public void OnClickPrevPage()
    {
        pageNum = pageNum > 0 ? pageNum - 1 : pageNum;
        UserDataSync();
    }

    private void UserDataSync()
    {
        int index = 9 * pageNum;
        User user = User.GetInstance();
        foreach (Toggle grid in GridList)
        {
            Image pokemonImg = grid.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            string path = index < user.Pokemons.Count
                ? "Pokemon/Portrait/" + user.Pokemons[index].ID
                : "Pokemon/Portrait/0";
            Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            pokemonImg.sprite = sprite;
            index++;
        }
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }


    // 切换某个Canvas到enable状态
    private void EnableCanvas(int num)
    {
        for (int i = 0; i < CanvasList.Count; i++)
        {
            CanvasList[i].enabled = (i == num);
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

    public void TalentCanvas()
    {
        EnableCanvas(4);
    }
}