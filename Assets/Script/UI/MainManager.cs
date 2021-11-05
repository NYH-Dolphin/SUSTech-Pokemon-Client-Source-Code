using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private static UserUI _user;

    public Text name;
    public Text level;
    public Text coin;
    public Text pokeBall;
    public Image portrait; // 头像
    private int portraitNum; // 头像编号

    // Start is called before the first frame update
    
    private void Awake()
    {
        // 同步获取User的相关信息
        UserDataSyncTemp();
    }

    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        // 检查头像
        if (_user.Portrait != portraitNum)
        {
            portraitNum = _user.Portrait;
            ChangePortraitImg(portraitNum);
        }
    }


    /**
     * 单例模式同步用户信息并显示在 UI 上
     * 
     */
    void UserDataSync()
    {
        _user = UserUI.GetInstance();
        // 信息配置
        name.text = _user.Name;
        level.text = "Lv." + _user.Level;
        coin.text = _user.Coin > 1000000 ? _user.Coin / 10000 + "万" : _user.Coin + "";
        pokeBall.text = _user.PokeBall + "个";
        // 头像配置
        portraitNum = _user.Portrait;
        ChangePortraitImg(portraitNum);
    }

    /**
     * 测试部分暂用，后删除
     */
    void UserDataSyncTemp()
    {
        // 测试部分，随后可删除
        _user = UserUI.SetFreshInstance("测试","测试");
        UserUI.CheckLogin();
        name.text = _user.Name;
        level.text = "Lv." + _user.Level;
        coin.text = _user.Coin > 1000000 ? _user.Coin / 10000 + "万" : _user.Coin + "";
        pokeBall.text = _user.PokeBall + "个";
        portraitNum = _user.Portrait;
        ChangePortraitImg(portraitNum);
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