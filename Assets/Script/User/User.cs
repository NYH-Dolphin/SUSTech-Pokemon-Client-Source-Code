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
        // 测试用
        if (!_instance.testSet)
        {
            TestSetInstance();
        }

        return _instance;
    }


    /**
     * [测试用]
     */
    private bool testSet;

    private static void TestSetInstance()
    {
        // int[] pokemonList = { 1, 2, 3, 4, 5, 6, 7, 16, 25, 26, 27, 28, 35, 39, 54, 96, 97, 98 };
        // for (int i = 0; i < pokemonList.Length; i++)
        // {
        //     _instance.Pokemons.Add(new Pokemon(pokemonList[i]));
        // }

        // _instance.PokemonDisplay1 = 4;
        // _instance.PokemonDisplay2 = 7;
        // _instance.PokemonDisplay3 = 39;
        _instance._adventurePokemon1 = new Pokemon(1);
        _instance._adventurePokemon2 = new Pokemon(5);
        _instance._adventurePokemon3 = new Pokemon(39);

        _instance._adventureLevel = 2;
        _instance.testSet = true;
    }


    /**
     * [将后端传送回来的JsonData设置进去]
     */
    public static void SetInstance(JsonData userData)
    {
        _instance.Account = userData["data"]["setting"]["user"]["account"].ToString();
        _instance.Password = userData["data"]["setting"]["user"]["password"].ToString();
        _instance.Name = userData["data"]["setting"]["user"]["name"].ToString();
        _instance.Coin = int.Parse(userData["data"]["setting"]["user"]["coin"].ToString());
        _instance.PokeBall = int.Parse(userData["data"]["setting"]["user"]["pokemonBall"].ToString());
        _instance.Level = int.Parse(userData["data"]["setting"]["user"]["level"].ToString());
        _instance.Portrait = int.Parse(userData["data"]["setting"]["user"]["portrait"].ToString());
        _instance.Token = userData["data"]["token"].ToString();
        _instance.PokemonDisplay1 = int.Parse(userData["data"]["setting"]["pokemon_show1"].ToString());
        _instance.PokemonDisplay2 = int.Parse(userData["data"]["setting"]["pokemon_show2"].ToString());
        _instance.PokemonDisplay3 = int.Parse(userData["data"]["setting"]["pokemon_show3"].ToString());
    }


    public bool HasSetPackage;

    public static void SetPackage(JsonData jsonData)
    {
        _instance.Package = new Package();
        for (int i = 0; i < jsonData.Count; i++)
        {
            JsonData jsonItem = jsonData[i]["item"];
            Item item = new Item(jsonItem["name"].ToString(), int.Parse(jsonItem["id"].ToString()));
            item.Description = jsonItem["description"] == null ? "暂时为空" : jsonItem["description"].ToString();
            int number = int.Parse(jsonData[i]["num"].ToString());
            switch (jsonItem["type"].ToString())
            {
                case "experience":
                    _instance.Package.ExperienceItems.Add(item, number);
                    break;
                case "material":
                    _instance.Package.MaterialItems.Add(item, number);
                    break;
                case "medicine":
                    _instance.Package.MedicineItems.Add(item, number);
                    break;
            }
        }
    }


    public bool HasSetPokemons = false;
    public static void SetPokemons(JsonData jsonData)
    {
        _instance.Pokemons = new List<Pokemon>();
        for (int i = 0; i < jsonData.Count; i++)
        {
            JsonData jsonPokemon = jsonData[i]["pokemon"];
            Pokemon pokemon = new Pokemon();
            pokemon.ID = int.Parse(jsonPokemon["id"].ToString());
            pokemon.Name = jsonPokemon["name"].ToString();
            pokemon.Genre = jsonPokemon["genre"].ToString();
            pokemon.Rarity = int.Parse(jsonPokemon["rarity"].ToString());
            
            pokemon.Level = int.Parse(jsonData[i]["level"].ToString());
            pokemon.CurrentExp = int.Parse(jsonData[i]["experience"].ToString());
            pokemon.Potential = int.Parse(jsonData[i]["potential"].ToString());
            
            JsonData jsonSkills = jsonData[i]["skills"];
            for (int j = 0; j < jsonSkills.Count; j++)
            {
                Skill skill = new Skill(int.Parse(jsonSkills[j]["id"].ToString()),
                    jsonSkills[j]["name"].ToString(),
                    jsonSkills[j]["description"].ToString(),
                    jsonSkills[j]["genre"].ToString());
                pokemon.Skills.Add(skill);
            }
            _instance.Pokemons.Add(pokemon);
        }
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


    private Pokemon _adventurePokemon1;

    public Pokemon AdventurePokemon1
    {
        get => _adventurePokemon1;
        set => _adventurePokemon1 = value;
    }

    private Pokemon _adventurePokemon2;

    public Pokemon AdventurePokemon2
    {
        get => _adventurePokemon2;
        set => _adventurePokemon2 = value;
    }

    public Pokemon AdventurePokemon3
    {
        get => _adventurePokemon3;
        set => _adventurePokemon3 = value;
    }


    private Pokemon _adventurePokemon3;


    private int _adventureLevel; // 目前用户冒险到了第几关

    public int AdventureLevel
    {
        get => _adventureLevel;
        set => _adventureLevel = value;
    }
}