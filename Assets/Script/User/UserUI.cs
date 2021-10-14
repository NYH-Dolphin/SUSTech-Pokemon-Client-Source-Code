using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserUI
{
    private  string _name; //名字

    private  string _password; //密码

    private  int _level; // 等级

    private  int _coin; //金币

    private  int _pokeBall; //精灵球
    
    private  int _portrait; // 头像 1-9
    
    private static UserUI _instance; // 单例模式用户实例

    private UserUI(string uName, string uPassword)
    {
        _name = uName;
        _password = uPassword;
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
     * 1 - 缺失账号
     * 2 - 缺失密码
     * 3 - 账号与密码不符合
     * 4 - 账号已注册
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
    
    
    public String GetName()
    {
        return _instance._name;
    }

    public int GetLevel()
    {
        return _instance._level;
    }
    
    public int GetCoin()
    {
        return _instance._coin;
    }

    public int GetPokeBall()
    {
        return _instance._pokeBall;
    }

    public int GetPortrait()
    {
        return _instance._portrait;
    }

    public void SetPortrait(int num)
    {
        _instance._portrait = num;
    }
    


}