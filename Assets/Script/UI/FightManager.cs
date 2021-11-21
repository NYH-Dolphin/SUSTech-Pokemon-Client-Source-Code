using System.Collections.Generic;
using LitJson;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityWebSocket;

public class FightManager : MonoBehaviour
{
    public Canvas ExitCanvas;
    public Canvas PokemonCanvas;
    public Canvas SkillCanvas;

    public Image UserPortrait;
    public Image UserPokemonImg;
    public Text UserPokemonName;
    public Text UserPokemonLevel;
    public Text UserPokemonHp;

    public Image OpponentPortrait;
    public Image OpponentPokemonImg;
    public Text OpponentPokemonName;
    public Text OpponentPokemonLevel;
    public Text OpponentPokemonHp;

    public Image Pokemon1;
    public Image Pokemon2;
    public Image Pokemon3;

    public Text FightMessage; // 战斗详细信息
    public Text StateMessage; // 回合状态信息

    public List<Text> SkillNames;
    public List<Text> SkillPPs;

    public Toggle PokemonToggle; // 宝可梦 Toggle
    public Toggle SkillToggle; // 技能 Toggle
    public List<Toggle> Pokemons; // 下面 3 个宝可梦 Toggle
    public List<Button> Skills; // 下面 4 个 Skills


    private int currPokemon; // 当前出战的宝可梦

    // 1- AdventurePokemon1
    // 2- AdventurePokemon2
    // 3- AdventurePokemon3
    private int currSkill; // 当前选择的技能

    // 1- Skill1
    // 2- Skill2
    // 3- Skill3
    // 4- Skill4
    private string currStage; // 当前的状态 逻辑服务器发来的 code 部分

    private IWebSocket _socket;

    // Start is called before the first frame update
    void Start()
    {
        CanvasInitial();
        ProhibitAllToggleAndBtn();
        currPokemon = 1;
        UserDataSync();
        PokemonDataSync();
        WebSocketHandShaking();
    }

    // Start 初始化
    //-----------------------------------------------------------------------

    #region [StartInitial]

    private void CanvasInitial()
    {
        PokemonCanvas.enabled = true;
        SkillCanvas.enabled = false;
        ExitCanvas.enabled = false;
    }


    // WebSocket基础握手
    void WebSocketHandShaking()
    {
        string address = BackEndConfig.GetGameLogicAddress(User.GetInstance().Token);
        Debug.Log(address);
        _socket = new WebSocket(address);
        _socket.OnOpen += SocketOnOpen;
        _socket.OnMessage += SocketOnMessage;
        _socket.OnClose += SocketOnClose;
        _socket.OnError += SocketOnError;
        _socket.ConnectAsync();
        FightMessage.text = "Connecting";
    }

    // 同步用户信息
    private void UserDataSync()
    {
        User user = User.GetInstance();
        int portraitNum = user.Portrait;
        string path = "User/Portrait/p" + portraitNum;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        UserPortrait.sprite = sprite;

        SetAdventurePokemon(Pokemon1, user.AdventurePokemon1.ID);
        SetAdventurePokemon(Pokemon2, user.AdventurePokemon2.ID);
        SetAdventurePokemon(Pokemon3, user.AdventurePokemon3.ID);
    }

    // 设置宝可梦Toggle的图片(初始化后就不会改变了)
    private void SetAdventurePokemon(Image pokemon, int num)
    {
        string path = "Pokemon/Portrait/" + num;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        pokemon.sprite = sprite;
    }

    #endregion

    //-----------------------------------------------------------------------


    // Web Socket 的功能
    //-----------------------------------------------------------------------

    #region [SocketFunction]

    void SocketOnOpen(object sender, OpenEventArgs e)
    {
    }

    void SocketOnMessage(object sender, MessageEventArgs e)
    {
        string recvMsg = e.Data
            .Replace("\\", "")
            .Replace("\"{", "{")
            .Replace("}\"", "}");
        
        Debug.Log(recvMsg);
        JsonData jsonData = JsonMapper.ToObject(recvMsg);
        currStage = jsonData["code"].ToString();
        switch (currStage)
        {
            case "CONNECTION_SUCCESS": // 连接成功
                StateMessage.text = currStage;
                SelectMode(FightCode.PVE);
                break;
            case "INITIALIZATION": // 设置信息
                StateMessage.text = currStage;
                Initial(jsonData);
                InitSetCurPokemon();
                break;
        }
    }


    void SocketOnClose(object sender, CloseEventArgs e)
    {
    }


    void SocketOnError(object sender, ErrorEventArgs e)
    {
    }

    private void SendData(FightMessage message)
    {
        string jsonData = JsonMapper.ToJson(message);
        _socket.SendAsync(jsonData);
    }

    #endregion

    //-----------------------------------------------------------------------


    //-----------------------------------------------------------------------
    // 通知游戏逻辑服务器选择PVP/PVE模式
    void SelectMode(FightCode mode)
    {
        FightMessage message = new FightMessage(mode);
        if (mode == FightCode.PVE)
        {
            message.CurrLevel = User.GetInstance().CurrentLevel;
        }

        SendData(message);
    }
    //-----------------------------------------------------------------------


    //-----------------------------------------------------------------------

    #region [INITIAL_MESSAGE]
    // 游戏逻辑服务器传来自己和对手的信息
    void Initial(JsonData jsonData)
    {
        User user = User.GetInstance();
        JsonData userData = jsonData["userInfo"];
        user.AdventurePokemon1.Hp = int.Parse(userData["battle1"]["baseHp"].ToString());
        user.AdventurePokemon1.CurrentHp = user.AdventurePokemon1.Hp;
        user.AdventurePokemon2.Hp = int.Parse(userData["battle2"]["baseHp"].ToString());
        user.AdventurePokemon2.CurrentHp = user.AdventurePokemon2.Hp;
        user.AdventurePokemon3.Hp = int.Parse(userData["battle3"]["baseHp"].ToString());
        user.AdventurePokemon3.CurrentHp = user.AdventurePokemon3.Hp;
        PokemonDataSync();
        JsonData opponentData = jsonData["monsterInfo"];
        string pokemonImgPath = "Pokemon/Pixel/Front/" + int.Parse(opponentData["id"].ToString());
        Sprite pokemonSprite = Resources.Load(pokemonImgPath, typeof(Sprite)) as Sprite;
        OpponentPokemonImg.sprite = pokemonSprite;
        OpponentPokemonName.text = opponentData["name"].ToString();
        OpponentPokemonLevel.text = "Lv." + opponentData["level"];
        OpponentPokemonHp.text = opponentData["currentHp"] + "/" + opponentData["baseHp"];
    }


    // 初始化时设置出场宝可梦
    void InitSetCurPokemon()
    {
        StateMessage.text = "选择出战宝可梦";
        // 禁用所有 skill btn
        foreach (Button skillBtn in Skills)
        {
            skillBtn.interactable = false;
        }

        // 为所有 pokemon toggle 添加监听事件
        foreach (Toggle pokemon in Pokemons)
        {
            pokemon.interactable = true;
            pokemon.onValueChanged.RemoveAllListeners();
            pokemon.onValueChanged.AddListener(value => OnChooseInitialPokemon(pokemon, value));
        }
    }


    // 选择出场的宝可梦
    void OnChooseInitialPokemon(Toggle toggle, bool value)
    {
        if (toggle.isOn)
        {
            ProhibitAllToggleAndBtn();
            currPokemon = int.Parse(toggle.name.Replace("pokemon", ""));
            PokemonDataSync();
            FightMessage message = new FightMessage(FightCode.SET_INITIAL_POKEMON)
            {
                CurrPokemon = currPokemon
            };
            SendData(message);
        }
    }
    #endregion

    //-----------------------------------------------------------------------


    // 通用宝可梦信息
    private void PokemonDataSync()
    {
        User user = User.GetInstance();
        Pokemon curPokemon = user.AdventurePokemon1;
        switch (currPokemon)
        {
            case 1:
                curPokemon = user.AdventurePokemon1;
                break;
            case 2:
                curPokemon = user.AdventurePokemon2;
                break;
            case 3:
                curPokemon = user.AdventurePokemon3;
                break;
        }

        string pokemonImgPath = "Pokemon/Pixel/Back/" + curPokemon.ID;
        Sprite pokemonSprite = Resources.Load(pokemonImgPath, typeof(Sprite)) as Sprite;
        UserPokemonImg.sprite = pokemonSprite;
        UserPokemonName.text = curPokemon.Name;
        UserPokemonLevel.text = "Lv." + curPokemon.Level;
        UserPokemonHp.text = curPokemon.CurrentHp + "/" + curPokemon.Hp;


        for (int i = 0; i < 4; i++)
        {
            SkillNames[i].text = curPokemon.Skills[i].Name;
            SkillPPs[i].text = "PP: " + curPokemon.Skills[i].PP;
        }
    }


    public void OnClickPokemon1(Toggle toggle)
    {
        if (toggle.isOn)
        {
            currPokemon = 1;
            SwitchPanel();
        }
    }

    public void OnClickPokemon2(Toggle toggle)
    {
        if (toggle.isOn)
        {
            currPokemon = 2;
            SwitchPanel();
        }
    }

    public void OnClickPokemon3(Toggle toggle)
    {
        if (toggle.isOn)
        {
            currPokemon = 3;
            SwitchPanel();
        }
    }


    // 用于点击宝可梦后，切换到技能页面
    private void SwitchPanel()
    {
        PokemonDataSync();
        PokemonToggle.isOn = false;
        SkillToggle.isOn = true;
        SkillCanvas.enabled = true;
        PokemonCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnClickPokemonToggle(Toggle toggle)
    {
        if (!toggle.isOn) return;
        PokemonCanvas.enabled = true;
        SkillCanvas.enabled = false;
    }

    public void OnClickSkillToggle(Toggle toggle)
    {
        if (!toggle.isOn) return;
        SkillCanvas.enabled = true;
        PokemonCanvas.enabled = false;
    }


    public void OnClickExitToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            ExitCanvas.enabled = true;
        }
    }


    public void OnClickExitConfirmBtn()
    {
        SceneManager.LoadScene("Adventure");
    }

    public void OnClickExitCancelBtn()
    {
        PokemonToggle.isOn = true;
        ExitCanvas.enabled = false;
    }


    private void ProhibitAllToggleAndBtn()
    {
        foreach (Button skillBtn in Skills)
        {
            skillBtn.interactable = false;
        }

        foreach (Toggle pokemon in Pokemons)
        {
            pokemon.interactable = false;
        }
    }
}