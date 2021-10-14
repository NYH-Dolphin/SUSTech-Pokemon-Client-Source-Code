using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Script.Pokemon;
using Script.Pokemon.Pokemons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class DrawCardManager : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    private bool isPlayerStarted;
    private bool showPokemon; // 标志抽卡视频播放完成，可以展示抽到的宝可梦了
    private List<string> list = new List<string>(); // 记录了抽到的宝可梦和物品的id，用于展示


    private string imagePath = "Pokemon/"; // 宝可梦图片的地址
    private string blankPath = "Other/blank"; // 空图片地址（稀有度中为Null的部分）

    private string starPath = "Other/star"; // 稀有度表示星星的地址

    // 显示的部分
    public Text pokemonName;
    public Image pokemonImage;
    public List<Image> starList;
    private List<Pokemon> pokemons; // 抽到的宝可梦
    private int showNum = 0; // 展示第几个宝可梦 点击屏幕会切换下一个宝可梦


    // Start is called before the first frame update
    void Start()
    {
        pokemons = GetPokemons();
    }

    // Update is called once per frame
    void Update()
    {
        if (!showPokemon) HideVideoAfterFinished();
        else
        {
            DisplayPokemon(showNum);
        }

    }


    /**
     * [抽卡宝可梦]
     */
    List<Pokemon> GetPokemons()
    {
        List<Pokemon> pokemons = new List<Pokemon>();
        pokemons.Add(new Pichu());
        pokemons.Add(new Pichu());
        pokemons.Add(new Pichu());
        return pokemons;
    }

    // 当召唤视频播放完后，自动隐藏
    void HideVideoAfterFinished()
    {
        if (!isPlayerStarted && VideoPlayer.isPlaying)
        {
            // When the player is started, set this information
            isPlayerStarted = true;
        }

        if (isPlayerStarted && !VideoPlayer.isPlaying)
        {
            // When the player stopped playing, hide it
            VideoPlayer.gameObject.SetActive(false);
            showPokemon = true;
        }
    }


    // 展示抽到的宝可梦
    void DisplayPokemon(int num)
    {
        pokemonName.text = pokemons[num].GetName();
        Debug.Log(pokemonName.text);
        pokemonImage.GetComponent<Image>().sprite =
            Resources.Load(imagePath + pokemons[num].GetID(), typeof(Sprite)) as Sprite;
        int rarity = pokemons[num].GetRarity();
        for (int i = 0; i < starList.Count; i++)
        {
            if (i < rarity)
            {
                starList[i].sprite = Resources.Load(starPath, typeof(Sprite)) as Sprite;
            }
            else
            {
                starList[i].sprite = Resources.Load(blankPath, typeof(Sprite)) as Sprite;
            }
        }
    }

    /**
     * [点击屏幕，展示下一个宝可梦]
     * blank_btn 使用
     */
    public void DisplayNext()
    {
        // 播放完视频后
        if (showPokemon)
        {
            showNum++;
            if (showNum == pokemons.Count)
            {
                BackToSummonScene();
            }
        }
    }
    
    public void BackToSummonScene()
    {
        SceneManager.LoadScene("Summon");
    }

}