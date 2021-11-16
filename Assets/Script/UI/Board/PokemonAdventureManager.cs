using System.Collections;
using System.Collections.Generic;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.UI;

public class PokemonAdventureManager : BoardManager
{
    public GameObject pokemonLayout;

    private int openNum; // 打开的是哪个版面
    public Image adventurePokemon1;
    public Image adventurePokemon2;
    public Image adventurePokemon3;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }


    // 传入Index，表示是从第几个Board中打开的
    public void OpenBoard(int num)
    {
        openNum = num;
        board.enabled = true;
        InitialBtn();
        DisabledBtn();
    }

    // 在关闭面板的时候将pokemonLayout内的btnPrefeb全部扔掉
    public override void CloseBoard()
    {
        int num = pokemonLayout.transform.childCount;
        for (int i = 0; i < num; i++)
        {
            Destroy(pokemonLayout.transform.GetChild(i).gameObject);
        }
        board.enabled = false;
    }

    /**
     * [初始化所有的 Btn]
     */
    public void InitialBtn()
    {
        User user = User.GetInstance();
        List<GameObject> pokemonBtns = new List<GameObject>();
        for (int i = 0; i < user.Pokemons.Count; i++)
        {
            GameObject pokemonBtnPrefab = Resources.Load("Item/Prefab/pokemon_btn") as GameObject;
            GameObject pokemonBtn = Instantiate(pokemonBtnPrefab);
            pokemonBtns.Add(pokemonBtn);
            int pokemonId = user.Pokemons[i].ID;
            string spritePath = "Pokemon/Portrait/" + pokemonId;
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

    private void OnClickGridBtn(Image img)
    {
        int pokemonId = int.Parse(img.sprite.name);
        User user = User.GetInstance();
        Pokemon specificPokemon = new Pokemon();
        foreach (var pokemon in user.Pokemons)
        {
            if (pokemon.ID == pokemonId)
            {
                specificPokemon = pokemon;
            }
        }

        switch (openNum)
        {
            case 1:
                ChangeDisplayPokemon(adventurePokemon1, pokemonId);
                user.AdventurePokemon1 = specificPokemon;
                break;
            case 2:
                ChangeDisplayPokemon(adventurePokemon2, pokemonId);
                user.AdventurePokemon2 = specificPokemon;
                break;
            case 3:
                ChangeDisplayPokemon(adventurePokemon3, pokemonId);
                user.AdventurePokemon3 = specificPokemon;
                break;
        }
        StartCoroutine(SetUserAdventurePokemon(openNum, pokemonId));
        CloseBoard();
    }

    IEnumerator SetUserAdventurePokemon(int seq, int pokemonId)
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        form.AddField("seq", seq);
        form.AddField("pokemon", pokemonId);
        string url = BackEndConfig.GetUrl() + "/userSetting/changeBattle";
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

    private void ChangeDisplayPokemon(Image pokemon, int num)
    {
        string path = "Pokemon/Image/" + num;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        pokemon.sprite = sprite;
        pokemon.SetNativeSize();
        pokemon.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }


    // 根据用户目前正在冒险的宝可梦禁用宝可梦的Id
    private void DisabledBtn()
    {
        User user = User.GetInstance();
        int childCount = pokemonLayout.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject btn = pokemonLayout.transform.GetChild(i).gameObject;
            int pokemonId = int.Parse(btn.transform.GetChild(0).GetComponent<Image>().sprite.name);
            // 如果宝可梦已经出场，禁用按钮，否则启用按钮
            if (pokemonId == user.AdventurePokemon1.ID || pokemonId == user.AdventurePokemon2.ID ||
                pokemonId == user.AdventurePokemon3.ID)
                btn.GetComponent<Button>().interactable = false;
            else
                btn.GetComponent<Button>().interactable = true;
        }
    }


// Update is called once per frame
    void Update()
    {
    }
}