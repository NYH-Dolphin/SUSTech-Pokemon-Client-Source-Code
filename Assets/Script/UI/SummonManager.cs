using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SummonManager : MonoBehaviour
{
    public Text pokeBall;

    public Text coin;

    public Text lackOfMessage; // 当精灵球的个数不够的时候，提示！


    public List<GameObject> CNPart;
    public List<GameObject> ENPart;


    // Start is called before the first frame update
    void Start()
    {
        UserDataSync();
        bool active = PlayerPrefs.GetString("language") == "CN";

        foreach (var obj in CNPart)
        {
            obj.SetActive(active);
        }

        foreach (var obj in ENPart)
        {
            obj.SetActive(!active);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void UserDataSync()
    {
        // 信息配置
        coin.text = User.GetInstance().Coin > 1000000
            ? User.GetInstance().Coin / 10000 + "万"
            : User.GetInstance().Coin + "";
        pokeBall.text = User.GetInstance().PokeBall.ToString();
    }


    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }


    // 单抽
    public void OnSummonOneTime()
    {
        if (User.GetInstance().PokeBall >= 1)
        {
            User.GetInstance().SummonNum = 1;
        }
        else
        {
            lackOfMessage.text = PlayerPrefs.GetString("language") == "CN" ? "精灵球个数不够哦~" : "Not enough Pokemon ball~";
        }
    }

    // 十连抽
    public void OnSummonTenTimes()
    {
        if (User.GetInstance().PokeBall >= 10)
        {
            User.GetInstance().SummonNum = 10;
        }
        else
        {
            lackOfMessage.text = PlayerPrefs.GetString("language") == "CN" ? "精灵球个数不够哦~" : "Not enough Pokemon ball~";
        }
    }
}