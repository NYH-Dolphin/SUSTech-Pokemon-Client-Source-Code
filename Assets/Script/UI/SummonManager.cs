using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SummonManager : MonoBehaviour
{
    private static User _user;
    public Text pokeBall;

    public Text coin;

    public Text lackOfMessage; // 当精灵球的个数不够的时候，提示！
    

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
        _user = User.GetInstance();
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
        if (User.GetInstance().PokeBall > 1)
        {
            User.GetInstance().SummonNum = 1;
            User.GetInstance().PokeBall = User.GetInstance().PokeBall - 1;
            // OpenDrawCardScene();
        }
        else
        {
            lackOfMessage.text = "精灵球个数不够哦~";
        }
    }

    // 十连抽
    public void OnSummonTenTimes()
    {
        if (User.GetInstance().PokeBall > 10)
        {
            User.GetInstance().SummonNum = 10;
            User.GetInstance().PokeBall = User.GetInstance().PokeBall - 10;
            // OpenDrawCardScene();
        }
        else
        {
            lackOfMessage.text = "精灵球个数不够哦~";
        }
    }
}