using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using LitJson;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class DrawCardManager : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    private bool isPlayerStarted;
    private bool showSummonResult; // 标志抽卡视频播放完成，可以展示抽到的宝可梦了


    private string pokemonImgPath = "Pokemon/Image/"; // 宝可梦图片的地址
    private string itemImgPath = "Item/Image/"; // 道具图片的地址
    private string blankPath = "Other/blank"; // 空图片地址（稀有度中为Null的部分）
    private string starPath = "Other/star"; // 稀有度表示星星的地址
    private int showPokemonNum = 0; // 展示第几个宝可梦 点击屏幕会切换下一个宝可梦
    private int showItemNum = 0; // 展示第几个Item


    // 显示的部分
    public Text pokemonName;
    public Image pokemonImage;
    public List<Image> starList;


    private List<Pokemon> pokemons = new List<Pokemon>(); // 抽到的宝可梦
    private List<Item> items = new List<Item>(); // 抽到的道具

    private bool pop = true; // 标识着刚开始弹出，从小弹出来！

    private UserUI _user;

    // Start is called before the first frame update
    void Start()
    {
        _user = UserUI.GetInstance();
        StartCoroutine("ToSummon"); // 开始抽卡
    }

    // Update is called once per frame
    void Update()
    {
        if (!showSummonResult) HideVideoAfterFinished();
        else
        {
            if (showItemNum < items.Count)
            {
                DisplayItem(showItemNum);
            }
            else if (showPokemonNum < pokemons.Count)
            {
                DisplayPokemon(showPokemonNum);
            }
            else
            {
                BackToSummonScene();
            }
        }
    }


    /**
     * [抽卡!!]
     */
    IEnumerator ToSummon()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", _user.Token);
        form.AddField("num", _user.SummonNum);
        string url = BackEndConfig.GetUrl() + "/userPokemon/summon";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            try
            {
                JsonData jsonItemData = request.value["data"]["item"];
                for (int i = 0; i < jsonItemData.Count; i++)
                {
                    JsonData jsonItem = jsonItemData[i];
                    Item item = new Item(jsonItem["name"].ToString(), int.Parse(jsonItem["id"].ToString()));
                    items.Add(item);
                }
            }
            catch
            {
            }


            try
            {
                JsonData jsonPokemonData = request.value["data"]["pokemon"];
                for (int i = 0; i < jsonPokemonData.Count; i++)
                {
                    JsonData jsonPokemon = jsonPokemonData[i];
                    Pokemon pokemon = new Pokemon();
                    pokemon.ID = int.Parse(jsonPokemon["id"].ToString());
                    pokemon.Name = jsonPokemon["name"].ToString();
                    pokemon.Rarity = int.Parse(jsonPokemon["rarity"].ToString());
                    pokemons.Add(pokemon);
                }
            }
            catch
            {
            }
        }
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
            showSummonResult = true;
        }
    }


    private float itemScale = 1.5f;
    // 展示抽到的道具
    void DisplayItem(int num)
    {
        pokemonName.text = items[num].Name;
        pokemonImage.GetComponent<Image>().sprite =
            Resources.Load(itemImgPath + items[num].ID, typeof(Sprite)) as Sprite;
        if (pop)
        {
            pokemonImage.transform.localScale = new Vector3(0, 0, 0);
            pop = false;
        }
        else
        {
            pokemonImage.SetNativeSize();
            float popOutDelta = pokemonImage.transform.localScale.x + Time.deltaTime * 3;
            popOutDelta = popOutDelta > itemScale ? itemScale : popOutDelta;
            pokemonImage.transform.localScale = new Vector3(popOutDelta, popOutDelta, popOutDelta);
        }

        for (int i = 0; i < starList.Count; i++)
        {
            starList[i].sprite = Resources.Load(blankPath, typeof(Sprite)) as Sprite;
        }
    }


    private float pokemonScale = 0.6f;
    // 展示抽到的宝可梦
    void DisplayPokemon(int num)
    {
        pokemonName.text = pokemons[num].Name;
        pokemonImage.GetComponent<Image>().sprite =
            Resources.Load(pokemonImgPath + pokemons[num].ID, typeof(Sprite)) as Sprite;

        // 进行一个精灵从小到大弹出来的效果，表示每个抽到的精灵！
        if (pop)
        {
            pokemonImage.transform.localScale = new Vector3(0, 0, 0);
            pokemonImage.transform.localScale = new Vector3(0, 0, 0);
            pop = false;
        }
        else
        {
            pokemonImage.SetNativeSize();
            float popOutDelta = pokemonImage.transform.localScale.x + Time.deltaTime * 3;
            popOutDelta = popOutDelta > pokemonScale ? pokemonScale : popOutDelta;
            pokemonImage.transform.localScale = new Vector3(popOutDelta, popOutDelta, popOutDelta);
        }


        int rarity = pokemons[num].Rarity;
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
        if (showSummonResult)
        {
            if (showItemNum < items.Count)
            {
                showItemNum++;
                pop = true;
            }
            else if (showPokemonNum < pokemons.Count)
            {
                showPokemonNum++;
                pop = true;
            }

            if (showItemNum == items.Count && showPokemonNum == pokemons.Count)
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