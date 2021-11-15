using System.Collections;
using System.Collections.Generic;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.UI;

public class PokemonPotentialManager : MonoBehaviour
{
    public Text potentialNum;
    public List<Image> potentialImg;
    private int curPokemonShowNum = 0;

    private string _starPath = "Other/potential"; // 表示潜能图片的地址
    private string _blankPath = "Other/blank"; // 空图片地址（稀有度中为Null的部分）

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
        int potential = pokemon.Potential;
        Sprite spriteStar = Resources.Load(_starPath, typeof(Sprite)) as Sprite;
        Sprite spriteBlank = Resources.Load(_blankPath, typeof(Sprite)) as Sprite;
        potentialNum.text = potential.ToString();
        for (int i = 0; i < potentialImg.Count; i++)
        {
            if (i + 1 <= potential)
            {
                potentialImg[i].sprite = spriteStar;
            }
            else
            {
                potentialImg[i].sprite = spriteBlank;
            }
        }
        
    }
}