using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserUI
{

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


    private static UserUI _instance; // 单例模式用户实例

    private UserUI(string uName, string uPassword)
    {
        _name = uName;
        _password = uPassword;
    }

    
    
    public void CheckLogin(string account, String password)
    {
        // if (account.Equals(""))
        // {
        //     // return 2;
        // }
        //
        // if (password.Equals(""))
        // {
        //     // return 3;
        // }
       



        // return 0;
    }
    
    
    

    /**
     * 重新配置新的User
     */
    public static UserUI SetFreshInstance(string name, string password)
    {
        _instance = new UserUI(name, password);
        return _instance;
    }

    /**
     * 获取单例User
     */
    public static UserUI GetInstance()
    {
        return _instance;
    }


    /**
     * 【未实现】
     * 0 - 首次登录
     * 1 - 非首次登录成功
     * 2 - 缺失账号
     * 3 - 缺失密码
     * 4 - 账号与密码不符合
     * 5 - 账号已注册
     */
    public static int CheckLogin()
    {
        // 从数据库获取内容
        GetPersonData();
        return 0;
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