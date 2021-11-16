using System.Collections;
using System.Collections.Generic;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.UI;

public class PokemonIntroManager : MonoBehaviour
{
    public Image pokemonImage;
    public List<Image> starList;
    public Image pokemonGenre; 
    
    private int curPokemonShowNum = 0;
    private string starPath = "Other/star"; // 稀有度表示星星的地址
    private string blankPath = "Other/blank"; // 空图片地址（稀有度中为Null的部分）

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
        string pokemonPath = "Pokemon/Image/" + pokemon.ID;
        Sprite spritePokemon = Resources.Load(pokemonPath, typeof(Sprite)) as Sprite;
        pokemonImage.sprite = spritePokemon;
        pokemonImage.SetNativeSize();
        pokemonImage.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
        
        string genrePath = "Pokemon/Properties/" + pokemon.GetGenreMap().Get(pokemon.Genre);
        Sprite spriteGenre = Resources.Load(genrePath, typeof(Sprite)) as Sprite;
        pokemonGenre.sprite = spriteGenre;
        
        int rarity = pokemon.Rarity;
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
}