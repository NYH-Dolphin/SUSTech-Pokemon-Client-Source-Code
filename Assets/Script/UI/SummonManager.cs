using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SummonManager : MonoBehaviour
{
    private static UserUI _user;
    public Text pokeBall;

    public Text coin;

    // Start is called before the first frame update
    void Start()
    {
        UserDataSync();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void UserDataSync()
    {
        _user = UserUI.GetInstance();
        // 信息配置
        coin.text = _user.Coin > 1000000 ? _user.Coin / 10000 + "万" : _user.Coin + "";
        pokeBall.text = _user.PokeBall + "个";
    }


    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }


    // 单抽
    public void OnSummonOneTime()
    {
        if (UserUI.GetInstance().PokeBall > 1)
        {
            UserUI.GetInstance().SummonNum = 1;
            UserUI.GetInstance().PokeBall = UserUI.GetInstance().PokeBall - 1;
            OpenDrawCardScene();
        }
    }

    // 十连抽
    public void OnSummonTenTimes()
    {
        if (UserUI.GetInstance().PokeBall > 10)
        {
            UserUI.GetInstance().SummonNum = 10;
            UserUI.GetInstance().PokeBall = UserUI.GetInstance().PokeBall - 10;
            OpenDrawCardScene();
        }
    }

    public void OpenDrawCardScene()
    {
        SceneManager.LoadScene("DrawCard");
    }
}