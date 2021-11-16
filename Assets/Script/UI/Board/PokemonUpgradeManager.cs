using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LitJson;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.UI;

public class PokemonUpgradeManager : MonoBehaviour
{
    public GameObject GameManager;

    public Text ExpDescription;
    public Text ExpNum;
    public InputField ExpInput;
    public Text PokemonExp;
    public RectTransform PokemonExpStick;
    public Canvas UpgradeMessageCanvas;
    public Text Message; // 如果升级的输入出现问题，会提示
    public Text UpgradeHintMessage; // 准备升级，显示预计升级的目标
    public Text OverFlowMessage; // 如果有经验溢出，进行提示


    private HashMap<Item, int> _items;
    private int _curExpType;
    private int _smallExpNum;
    private int _middleExpNum;
    private int _largeExpNum;

    private int _curPokemonShowNum = 0;
    private string pattern = @"^[0-9]*$";
    private Pokemon _curPokemon;
    private int _curExpNum;

    private bool _pokemonHasChanged = false;
    private bool _itemHasChanged = false;

    void Start()
    {
        UserDataSync();
        PokemonDataSync();
        // 初始化
        ExpDescription.text = "小型经验包：";
        ExpNum.text = "Max:" + _smallExpNum;
        _curExpType = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_curPokemonShowNum != User.GetInstance().PokemonShowNum)
        {
            PokemonDataSync();
        }

        if (_itemHasChanged && _pokemonHasChanged)
        {
            _itemHasChanged = false;
            _pokemonHasChanged = false;
            GameManager.GetComponent<PokemonManager>().UserDataSync();
            GameManager.GetComponent<PokemonManager>().PokemonDataSync();
            UserDataSync();
            PokemonDataSync();

            switch (_curExpType)
            {
                case 1:
                    ExpNum.text = "Max:" + _smallExpNum;
                    break;
                case 2:
                    ExpNum.text = "Max:" + _middleExpNum;
                    break;
                case 3:
                    ExpNum.text = "Max:" + _largeExpNum;
                    break;
            }
        }
    }


    private void UserDataSync()
    {
        _items = User.GetInstance().Package.ExperienceItems;

        foreach (Item key in _items.Keys)
        {
            if (key.ID == 1)
                _smallExpNum = _items.Get(key);
            else if (key.ID == 2)
                _middleExpNum = _items.Get(key);
            else if (key.ID == 3)
                _largeExpNum = _items.Get(key);
        }
    }


    private void PokemonDataSync()
    {
        User user = User.GetInstance();
        int index = user.PokemonShowNum;
        _curPokemonShowNum = index;
        Pokemon pokemon = user.Pokemons[index];
        StartCoroutine(GetPokemonUpgradeData(pokemon));
    }

    IEnumerator GetPokemonUpgradeData(Pokemon pokemon)
    {
        WWWForm form = new WWWForm();
        form.AddField("experience", pokemon.CurrentExp);
        form.AddField("type", pokemon.GrowType);
        string url = BackEndConfig.GetUrl() + "/userPokemon/getLevel";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            JsonData jsonData = request.value["data"];
            int nextExp = int.Parse(jsonData["next"].ToString());
            // 改变pokemon经验的显示和bar
            PokemonExp.text = pokemon.CurrentExp + "/" + nextExp;
            float width = 220 * ((float)pokemon.CurrentExp / nextExp);
            PokemonExpStick.sizeDelta = new Vector2(width, PokemonExpStick.sizeDelta.y);
        }
    }

    // 点击小型经验包
    public void OnClickSmallToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            ExpDescription.text = "小型经验包：";
            ExpNum.text = "Max:" + _smallExpNum;
            _curExpType = 1;
        }
    }


    // 点击中型经验包
    public void OnClickMiddleToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            ExpDescription.text = "中型经验包：";
            ExpNum.text = "Max:" + _middleExpNum;
            _curExpType = 2;
        }
    }

    // 点击大型经验包
    public void OnClickLargeToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            ExpDescription.text = "大型经验包：";
            ExpNum.text = "Max:" + _largeExpNum;
            _curExpType = 3;
        }
    }


    public void OnClickUpgradeBtn()
    {
        Regex regex = new Regex(pattern);
        if (string.IsNullOrEmpty(ExpInput.text))
        {
            Message.text = "您还没有输入内容！";
        }
        else if (!regex.IsMatch(ExpInput.text))
        {
            Message.text = "您输入的不是合法数字！";
        }
        else
        {
            int inputNum = int.Parse(ExpInput.text);
            int expNumber = 0;
            switch (_curExpType)
            {
                case 1:
                    expNumber = _smallExpNum;
                    break;
                case 2:
                    expNumber = _middleExpNum;
                    break;
                case 3:
                    expNumber = _largeExpNum;
                    break;
            }

            if (inputNum > expNumber)
            {
                Message.text = "您的经验书数量不够！";
            }
            else if (inputNum <= 0)
            {
                Message.text = "您输入的数字非法！";
            }
            else
            {
                _curExpNum = inputNum;
                Message.text = "";
                OnOpenMessageCanvas(inputNum);
            }
        }
    }


    public void OnOpenMessageCanvas(int expNumber)
    {
        User user = User.GetInstance();
        int index = user.PokemonShowNum;
        _curPokemonShowNum = index;
        Pokemon pokemon = user.Pokemons[index];
        int addExp = 0;
        // 小 50 中 200 大 500
        switch (_curExpType)
        {
            case 1:
                addExp += 50 * expNumber;
                break;
            case 2:
                addExp += 200 * expNumber;
                break;
            case 3:
                addExp += 500 * expNumber;
                break;
        }

        StartCoroutine(GetProbablyPokemonLevel(pokemon, addExp));
        _curPokemon = pokemon;
        UpgradeMessageCanvas.enabled = true;
    }


    IEnumerator GetProbablyPokemonLevel(Pokemon pokemon, int addExp)
    {
        WWWForm form = new WWWForm();
        form.AddField("experience", pokemon.CurrentExp + addExp);
        form.AddField("type", pokemon.GrowType);
        string url = BackEndConfig.GetUrl() + "/userPokemon/getLevel";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            JsonData jsonData = request.value["data"];
            int nextLevel = int.Parse(jsonData["level"].ToString());
            if (nextLevel >= pokemon.NextLevel)
            {
                UpgradeHintMessage.text = "预计升级到" + pokemon.NextLevel + "级";
                OverFlowMessage.text = "可能存在经验溢出";
            }
            else
            {
                UpgradeHintMessage.text = "预计升级到" + nextLevel + "级";
                OverFlowMessage.text = "";
            }
        }
    }

    public void OnClickConfirmBtn()
    {
        StartCoroutine(UpgradePokemon());
        OnCloseMessageCanvas();
    }


    IEnumerator UpgradePokemon()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        form.AddField("pokemon", _curPokemon.ID);
        form.AddField("type", _curExpType);
        form.AddField("num", _curExpNum);
        string url = BackEndConfig.GetUrl() + "/userPokemon/promote";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            StartCoroutine(GetAllUserPokemons());
            StartCoroutine(GetAllUserItems());
        }
    }


    /**
     * 搜索获得账号中所有的物品！
     */
    IEnumerator GetAllUserItems()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        string url = BackEndConfig.GetUrl() + "/knapsack/my";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            User.SetPackage(request.value["data"]);
            _itemHasChanged = true;
        }
    }


    /**
     * 搜索获得账号中所有的宝可梦
     */
    IEnumerator GetAllUserPokemons()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        string url = BackEndConfig.GetUrl() + "/userPokemon/my";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            User.SetPokemons(request.value["data"]);
            _pokemonHasChanged = true;
        }
    }


    // 关闭 Message Canvas
    public void OnCloseMessageCanvas()
    {
        UpgradeMessageCanvas.enabled = false;
    }
    
}