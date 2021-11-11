using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Org.BouncyCastle.Crypto.Tls;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PokemonChangeManager : BoardManager
{
    public GameObject pokemonLayout;

    private string _displayBoard; // 记录要更换的是哪个宝可梦
    public Image pokemon1; // 展示宝可梦1
    public Image pokemon2; // 展示宝可梦2
    public Image pokemon3; // 展示宝可梦3

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // 将User所有的宝可梦显示出来
    public void InitialBtn()
    {
        User user = User.GetInstance();
        List<GameObject> pokemonBtns = new List<GameObject>();
        for (int i = 0; i < user.Pokemons.Count; i++)
        {
            GameObject pokemonBtnPrefab = Resources.Load("Item/Prefab/pokemon_btn") as GameObject;
            GameObject pokemonBtn = Instantiate(pokemonBtnPrefab);
            pokemonBtns.Add(pokemonBtn);
            String spritePath = "Pokemon/Portrait/" + user.Pokemons[i].ID;
            Sprite sprite = Resources.Load(spritePath, typeof(Sprite)) as Sprite;
            Image portraitImg = pokemonBtn.transform.GetChild(0).GetComponent<Image>();
            portraitImg.sprite = sprite;
            // 设置回调函数
            pokemonBtn.GetComponent<Button>().onClick.AddListener(() => OnClickGridBtn(portraitImg));
        }

        foreach (var btn in pokemonBtns)
        {
            btn.transform.SetParent(pokemonLayout.transform, false);
        }
    }

    // 打开的时候，记录点开的是哪个btn的展示面板
    public void OpenBoardWithRecord(GameObject btn)
    {
        InitialBtn();
        _displayBoard = btn.name;
        board.enabled = true;
    }

    // 关闭面板的时候，将pokemonLayout所有的btn删除
    public override void CloseBoard()
    {
        int num = pokemonLayout.transform.childCount;
        for(int i = 0; i < num; i++)
        {
            Destroy(pokemonLayout.transform.GetChild(i).gameObject);
        }

        base.CloseBoard();
    }


    // 更换指定面板上的展示宝可梦
    public void OnClickGridBtn(Image img)
    {
        int num = int.Parse(img.sprite.name);
        User user = User.GetInstance();
        switch (_displayBoard)
        {
            case "pokemon1":
                user.PokemonDisplay1 = num;
                ChangeDisplayPokemon(pokemon1, num);
                break;
            case "pokemon2":
                user.PokemonDisplay2 = num;
                ChangeDisplayPokemon(pokemon2, num);
                break;
            case "pokemon3":
                user.PokemonDisplay3 = num;
                ChangeDisplayPokemon(pokemon3, num);
                break;
        }

        CloseBoard();
    }


    public void ChangeDisplayPokemon(Image pokemon, int num)
    {
        string path = "Pokemon/Image/" + num;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        pokemon.sprite = sprite;
        pokemon.SetNativeSize();
        pokemon.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }


    // Update is called once per frame
    void Update()
    {
    }
}