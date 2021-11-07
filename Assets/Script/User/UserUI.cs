using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserUI
{

    private static UserUI _instance = new UserUI(); // 单例模式用户实例
    /**
     * 获取单例User
     */
    public static UserUI GetInstance()
    {
        return _instance;
    }


    /**
     * [将后端传送回来的JsonData设置进去]
     */
    public static void SetInstance(JsonData userData)
    {
        _instance.Account = userData["data"]["user"]["account"].ToString();
        _instance.Password = userData["data"]["user"]["password"].ToString();
        _instance.Name = userData["data"]["user"]["name"].ToString();
        _instance.Coin =  int.Parse(userData["data"]["user"]["coin"].ToString());
        _instance.PokeBall = int.Parse(userData["data"]["user"]["pokemonBall"].ToString());
        _instance.Level = int.Parse(userData["data"]["user"]["level"].ToString());
        _instance.Portrait = int.Parse(userData["data"]["user"]["portrait"].ToString());
        _instance.Token = userData["data"]["token"].ToString();
    }

    private string _account; // 账户
    public string Account
    {
        get => _account;
        set => _account = value;
    }
    private string _name; //名字
    public string Name
    {
        get => _name;
        set => _name = value;
    }

    private string _password; //密码

    public String Password
    {
        get => _password;
        set => _password = value;
    }

    private string _token;

    public String Token
    {
        get => _token;
        set => _token = value;
    }

    private int _level; // 等级

    public int Level
    {
        get => _level;
        set => _level = value;
    }

    private int _coin; //金币

    public int Coin
    {
        get => _coin;
        set => _coin = value;
    }

    private int _pokeBall; //精灵球

    public int PokeBall
    {
        get => _pokeBall;
        set => _pokeBall = value;
    }

    private int _portrait; // 头像 1-9

    public int Portrait
    {
        get => _portrait;
        set => _portrait = value;
    }
    

    /**
     * 【未实现】
     * 通过用户名和密码从数据库获取个人数据
     */
    private static void GetPersonData()
    {
        _instance._level = 25;
        _instance._coin = 29481275;
        _instance._pokeBall = 120;
        _instance._portrait = 1;
    }
    
}