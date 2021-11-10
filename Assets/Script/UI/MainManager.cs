using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    
    private void Awake()
    {
        // 同步获取User的相关信息
        UserDataSync();
    }

    void Start()
    {
        
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
        coin.text = _user.Coin > 1000000 ? _user.Coin / 10000 + "万" : _user.Coin + "";
        pokeBall.text = _user.PokeBall + "个";
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
        Sprite sprite = Resources.Load(path,typeof(Sprite)) as Sprite;
        portrait.sprite = sprite;
    }

    public void ChangeDisplayPokemon(int p1, int p2, int p3)
    {
        string path1 = "Pokemon/Image/" + p1;
        string path2 = "Pokemon/Image/" + p2;
        string path3 = "Pokemon/Image/" + p3;
        Sprite sprite1 = Resources.Load(path1,typeof(Sprite)) as Sprite;
        Sprite sprite2 = Resources.Load(path2,typeof(Sprite)) as Sprite;
        Sprite sprite3 = Resources.Load(path3,typeof(Sprite)) as Sprite;
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
}