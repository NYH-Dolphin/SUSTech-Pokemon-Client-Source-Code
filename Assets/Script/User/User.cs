using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using LitJson;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityWebSocket;

public class User
{

    private IWebSocket _PVPSocket;

    public IWebSocket PVPSocket
    {
        get => _PVPSocket;
        set => _PVPSocket = value;
    }
    private FightCode mode;

    public FightCode Mode
    {
        get => mode;
        set => mode = value;
    }


    private static User _instance = new User(); // 单例模式用户实例

    /**
 * 获取单例User
 */
    public static User GetInstance()
    {
        return _instance;
    }


    public void ResetAdventurePokemonPP()
    {
        foreach (var skill in _adventurePokemon1.Skills)
        {
            skill.ResetPP();
        }
        foreach (var skill in _adventurePokemon2.Skills)
        {
            skill.ResetPP();
        }
        foreach (var skill in _adventurePokemon3.Skills)
        {
            skill.ResetPP();
        }
        
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
        _instance.AdventureLevel = int.Parse(userData["data"]["setting"]["user"]["adventure"].ToString());
        _instance.Token = userData["data"]["token"].ToString();
        _instance.PokemonDisplay1 = int.Parse(userData["data"]["setting"]["pokemon_show1"].ToString());
        _instance.PokemonDisplay2 = int.Parse(userData["data"]["setting"]["pokemon_show2"].ToString());
        _instance.PokemonDisplay3 = int.Parse(userData["data"]["setting"]["pokemon_show3"].ToString());
        int adventure1 = int.Parse(userData["data"]["setting"]["pokemon_battle1"].ToString());
        int adventure2 = int.Parse(userData["data"]["setting"]["pokemon_battle2"].ToString());
        int adventure3 = int.Parse(userData["data"]["setting"]["pokemon_battle3"].ToString());
        _instance.AdventurePokemon1 = new Pokemon(adventure1);
        _instance.AdventurePokemon2 = new Pokemon(adventure2);
        _instance.AdventurePokemon3 = new Pokemon(adventure3);
    }
    
    public static void SetPackage(JsonData jsonData)
    {
        _instance.Package = new Package();
        for (int i = 0; i < jsonData.Count; i++)
        {
            JsonData jsonItem = jsonData[i]["item"];
            Item item = new Item(jsonItem["name"].ToString(), int.Parse(jsonItem["id"].ToString()));
            item.Name_EN = jsonItem["name_en"].ToString();
            item.Description = jsonItem["description"] == null ? "暂时为空" : jsonItem["description"].ToString();
            item.Description_EN = jsonItem["description_en"] == null ? "null" : jsonItem["description_en"].ToString();
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

    public static void SetPokemons(JsonData jsonData)
    {
        _instance.Pokemons = new List<Pokemon>();
        _instance.AllPokemons = new List<Pokemon>();

        for (int i = 0; i < jsonData.Count; i++)
        {
            JsonData jsonPokemon = jsonData[i]["pokemon"];
            Pokemon pokemon = new Pokemon();
            pokemon.ID = int.Parse(jsonPokemon["id"].ToString());
            pokemon.Name = jsonPokemon["name"].ToString();
            pokemon.Name_EN = jsonPokemon["name_en"].ToString();
            pokemon.Genre = jsonPokemon["genre"].ToString();
            pokemon.NextID = int.Parse(jsonPokemon["nextId"].ToString());
            pokemon.NextLevel = int.Parse(jsonPokemon["nextLevel"].ToString());
            pokemon.Rarity = int.Parse(jsonPokemon["rarity"].ToString());
            pokemon.GrowType = jsonPokemon["growType"].ToString();
            pokemon.IsDeprecated = jsonData[i]["isDeprecated"].ToString().Equals("True");
            pokemon.Level = int.Parse(jsonData[i]["level"].ToString());
            pokemon.CurrentExp = int.Parse(jsonData[i]["experience"].ToString());
            pokemon.Potential = int.Parse(jsonData[i]["potential"].ToString());
            JsonData jsonMonster = jsonData[i]["monster"];
            pokemon.Atk = int.Parse(jsonMonster["baseAtk"].ToString());
            pokemon.Hp = int.Parse(jsonMonster["baseHp"].ToString());
            pokemon.Def = int.Parse(jsonMonster["baseDef"].ToString());
            pokemon.Satk = int.Parse(jsonMonster["baseSatk"].ToString());
            pokemon.Sdef = int.Parse(jsonMonster["baseSdef"].ToString());
            pokemon.Speed = int.Parse(jsonMonster["baseSpeed"].ToString());

            JsonData jsonSkills = jsonData[i]["skills"];
            for (int j = 0; j < jsonSkills.Count; j++)
            {
                Skill skill = new Skill(int.Parse(jsonSkills[j]["id"].ToString()),
                    jsonSkills[j]["name"].ToString(),
                    jsonSkills[j]["description"].ToString(),
                    jsonSkills[j]["genre"].ToString(),
                    int.Parse(jsonSkills[j]["pp"].ToString()),
                    int.Parse(jsonSkills[j]["hit"].ToString()),
                    int.Parse(jsonSkills[j]["power"].ToString()));
                skill.Name_EN = jsonSkills[j]["name_en"].ToString();
                skill.Description_EN = jsonSkills[j]["description_en"].ToString();
                pokemon.Skills.Add(skill);
            }

            try
            {
                JsonData jsonEvolve = jsonData[i]["evolve"];
                foreach (string id in jsonEvolve.Keys)
                {
                    pokemon.EvolveMap.Add(int.Parse(id), int.Parse(jsonEvolve[id].ToString()));
                }
            }
            catch
            {
                pokemon.EvolveMap.Add(0, 0);
            }


            if (_instance.AdventurePokemon1.ID == pokemon.ID)
                _instance.AdventurePokemon1 = pokemon;

            if (_instance.AdventurePokemon2.ID == pokemon.ID)
                _instance.AdventurePokemon2 = pokemon;

            if (_instance.AdventurePokemon3.ID == pokemon.ID)
                _instance.AdventurePokemon3 = pokemon;

            _instance.AllPokemons.Add(pokemon);
            if (!pokemon.IsDeprecated)
                _instance.Pokemons.Add(pokemon);
        }
    }


    private List<Pokemon> _pokemons = new List<Pokemon>(); // 拥有的宝可梦(不包括弃用)

    public List<Pokemon> Pokemons
    {
        get => _pokemons;
        set => _pokemons = value;
    }

    private List<Pokemon> _allPokemons = new List<Pokemon>(); // 所有的宝可梦（包括弃用）

    public List<Pokemon> AllPokemons
    {
        get => _allPokemons;
        set => _allPokemons = value;
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


    private int _pokemonShowNum; // 在 Pokemon Scene 中表示目前显示的是哪只宝可梦

    public int PokemonShowNum
    {
        get => _pokemonShowNum;
        set => _pokemonShowNum = value;
    }


    private int _currLevel; // 当前在冒险哪一关

    public int CurrentLevel
    {
        get => _currLevel;
        set => _currLevel = value;
    }
}