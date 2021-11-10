using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LitJson;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class User
{
    private static User _instance = new User(); // 单例模式用户实例

    /**
 * 获取单例User
 */
    public static User GetInstance()
    {
        TestSetInstance(); // 测试用
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
        _instance.Coin = int.Parse(userData["data"]["user"]["coin"].ToString());
        _instance.PokeBall = int.Parse(userData["data"]["user"]["pokemonBall"].ToString());
        _instance.Level = int.Parse(userData["data"]["user"]["level"].ToString());
        _instance.Portrait = int.Parse(userData["data"]["user"]["portrait"].ToString());
        _instance.Token = userData["data"]["token"].ToString();

        // int[] pokemonList = { 1, 4, 7, 16, 35, 39 };
        // _instance.Pokemon = pokemonList.ToList();
        // _instance.PokemonDisplay1 = 4;
        // _instance.PokemonDisplay2 = 7;
        // _instance.PokemonDisplay3 = 39;
    }

    public static void TestSetInstance()
    {
        // 测试用
        int[] pokemonList = { 1, 2, 3, 4, 5, 6, 7, 16, 25, 26, 27, 28, 35, 39, 54, 96, 97, 98 };
        for (int i = 0; i < pokemonList.Length; i++)
        {
            _instance.Pokemons.Add(new Pokemon(pokemonList[i]));
        }
        _instance.PokemonDisplay1 = 4;
        _instance.PokemonDisplay2 = 7;
        _instance.PokemonDisplay3 = 39;
    }
    
    private List<Pokemon> _pokemons = new List<Pokemon>(); // 拥有的宝可梦
    
    public List<Pokemon> Pokemons
    {
        get => _pokemons;
        set => _pokemons = value;
    }


    private Package _package; // 背包
    public Package Package
    {
        get => _package;
        set => _package = value;
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

    private int _pokemonDisplay1; // 展示的宝可梦1

    public int PokemonDisplay1
    {
        get => _pokemonDisplay1;
        set => _pokemonDisplay1 = value;
    }


    private int _pokemonDisplay2; // 展示的宝可梦2

    public int PokemonDisplay2
    {
        get => _pokemonDisplay2;
        set => _pokemonDisplay2 = value;
    }

    private int _pokemonDisplay3; // 展示的宝可梦3

    public int PokemonDisplay3
    {
        get => _pokemonDisplay3;
        set => _pokemonDisplay3 = value;
    }



    
    
    private int _summonNum; // 当次选择的抽卡次数

    public int SummonNum
    {
        get => _summonNum;
        set => _summonNum = value;
    }
}