using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Script.Shop;

public class MainManager : MonoBehaviour
{
    private static User _user;

    public Text name;
    public Text level;
    public Text coin;
    public Text pokeBall;
    public Image portrait; // 头像
    public Image pokemon1; // 展示宝可梦1
    public Image pokemon2; // 展示宝可梦2
    public Image pokemon3; // 展示宝可梦3

    private GameObject _bgm;

    void Start()
    {
        GameObject bgmPrefab = MusicPlayer.GetBgm("main_music");
        _bgm = Instantiate(bgmPrefab);

        // 同步获取User的相关信息
        UserDataSync();

        StartCoroutine("GetAllUserItems");
        StartCoroutine("GetAllUserPokemons");
        if (!Shop.getInstance().FirstInitial)
            StartCoroutine("GetAllShopItems");
    }


    // Update is called once per frame
    void Update()
    {
    }


    /**
     * 单例模式同步用户信息并显示在 UI 上
     */
    void UserDataSync()
    {
        _user = User.GetInstance();
        // 信息配置
        name.text = _user.Name;
        level.text = "Lv." + _user.Level;

        if (PlayerPrefs.GetString("language") == "CN")
        {
            coin.text = _user.Coin > 1000000 ? _user.Coin / 10000 + "万" : _user.Coin + "";
        }
        else
        {
            string coinNum = (_user.Coin / 1000000).ToString("0.000");
            coin.text = _user.Coin > 1000000 ? coinNum + "million" : _user.Coin + "";
        }
        
        pokeBall.text = _user.PokeBall.ToString();
        // 头像配置
        ChangePortraitImg(_user.Portrait);
        // 展示宝可梦配置
        ChangeDisplayPokemon(_user.PokemonDisplay1, _user.PokemonDisplay2, _user.PokemonDisplay3);
    }

    /**
     * 更换 Main 面板中的头像
     */
    public void ChangePortraitImg(int num)
    {
        string path = "User/Portrait/p" + num;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        portrait.sprite = sprite;
    }

    public void ChangeDisplayPokemon(int p1, int p2, int p3)
    {
        string path1 = "Pokemon/Image/" + p1;
        string path2 = "Pokemon/Image/" + p2;
        string path3 = "Pokemon/Image/" + p3;
        Sprite sprite1 = Resources.Load(path1, typeof(Sprite)) as Sprite;
        Sprite sprite2 = Resources.Load(path2, typeof(Sprite)) as Sprite;
        Sprite sprite3 = Resources.Load(path3, typeof(Sprite)) as Sprite;
        pokemon1.sprite = sprite1;
        pokemon1.SetNativeSize();
        pokemon1.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
        pokemon2.sprite = sprite2;
        pokemon2.SetNativeSize();
        pokemon2.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
        pokemon3.sprite = sprite3;
        pokemon3.SetNativeSize();
        pokemon3.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }


    /**
     * 搜索获得账号中所有的物品！
     */
    IEnumerator GetAllUserItems()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        string url = BackEndConfig.GetUrl() + "/knapsack/my";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            User.SetPackage(request.value["data"]);
        }
    }

    /**
     * 搜索获得账号中所有的宝可梦
     */
    IEnumerator GetAllUserPokemons()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        string url = BackEndConfig.GetUrl() + "/userPokemon/my";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            User.SetPokemons(request.value["data"]);
        }
    }

    /**
     * 搜索获得商店的所有东西
     */
    IEnumerator GetAllShopItems()
    {
        string url = BackEndConfig.GetUrl() + "/shop/show";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Get(url));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            Shop.SetItems(request.value["data"]);
        }
    }


    public void OpenPPTScene()
    {
        SceneManager.LoadScene("PPT");
    }

    public void OpenShopScene()
    {
        SceneManager.LoadScene("Shop");
    }


    public void OpenSummonScene()
    {
        SceneManager.LoadScene("Summon");
    }

    public void OpenPackageScene()
    {
        SceneManager.LoadScene("Package");
    }

    public void OpenCompeteScene()
    {
        SceneManager.LoadScene("Compete");
    }

    public void OpenAdventureScene()
    {
        SceneManager.LoadScene("Adventure");
    }

    public void OpenPokemonScene()
    {
        SceneManager.LoadScene("Pokemon");
    }


    // 切换账号
    public void OnClickReloginBtn()
    {
        SceneManager.LoadScene("Start");
    }
}