using System;
using System.Collections.Generic;
using LitJson;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityWebSocket;

public class FightManager : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    private bool isPlayerStarted;


    public Canvas ExitCanvas;
    public Canvas LoseCanvas;
    public Canvas WinCanvas;
    public Canvas PokemonCanvas;
    public Canvas SkillCanvas;

    public Image UserPortrait;
    public Image UserPokemonImg;
    public Text UserPokemonName;
    public Text UserPokemonLevel;
    public Text UserPokemonHp;
    public RectTransform UserPokemonHpBar;

    public Image OpponentPortrait;
    public Image OpponentPokemonImg;
    public Text OpponentPokemonName;
    public Text OpponentPokemonLevel;
    public Text OpponentPokemonHp;
    public RectTransform OpponentPokemonHpBar;

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

    public Text pokeballNum;
    public Text coinNum;
    public Image itemImg;


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

    void Update()
    {
        HideVideoAfterFinished();
    }

    // 当召唤视频播放完后，自动隐藏
    void HideVideoAfterFinished()
    {
        if (!isPlayerStarted && VideoPlayer.isPlaying)
        {
            // When the player is started, set this information
            isPlayerStarted = true;
        }

        if (isPlayerStarted && !VideoPlayer.isPlaying)
        {
            // When the player stopped playing, hide it
            VideoPlayer.gameObject.SetActive(false);
        }
    }


    // Start 初始化
    //-----------------------------------------------------------------------

    #region [StartInitial]

    private void CanvasInitial()
    {
        PokemonCanvas.enabled = true;
        SkillCanvas.enabled = false;
        ExitCanvas.enabled = false;
        LoseCanvas.enabled = false;
        WinCanvas.enabled = false;
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

        Debug.Log("Receive Message:" + recvMsg);
        JsonData jsonData = JsonMapper.ToObject(recvMsg);
        currStage = jsonData["code"].ToString();
        switch (currStage)
        {
            case "CONNECTION_SUCCESS": // 连接成功
                StateMessage.text = "连接成功";
                SelectMode(FightCode.PVE); // 选择 PVE 模式
                break;
            case "INITIALIZATION": // 设置信息
                StateMessage.text = "初始化信息";
                Initial(jsonData); // 配置初始信息
                InitSetCurPokemon(); // 初始化敌我双方的宝可梦信息
                break;
            case "VALID":
                StateMessage.text = "到你的回合！";
                SelectCurPokemonAndSkill(); // 允许选择宝可梦
                break;
            case "FORBIDDEN":
                StateMessage.text = "对手的回合！";
                ProhibitAllToggleAndBtn(); // 禁用按钮
                break;
            case "ROUND_INFO": // 通知双方宝可梦扣血的信息
                DisplayRoundInfo(jsonData);
                break;
            case "FIGHT_MESSAGE": // 传详细的文字战斗信息
                DisplayFightMessage(jsonData);
                break;
            case "LOSE":
                ProhibitAllToggleAndBtn();
                StateMessage.text = "战斗失败...";
                LoseCanvas.enabled = true;
                break;
            case "WIN":
                ProhibitAllToggleAndBtn();
                StateMessage.text = "战斗胜利";
                SetWinItems(jsonData);
                WinCanvas.enabled = true;
                break;
        }
    }

    private void DisplayRoundInfo(JsonData jsonData)
    {
        ProhibitAllToggleAndBtn();
        User user = User.GetInstance();
        Pokemon curPokemon = user.AdventurePokemon1;
        switch (int.Parse(jsonData["userPokemonPos"].ToString()))
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

        curPokemon.CurrentHp = int.Parse(jsonData["userPokemonCurrentHp"].ToString());
        curPokemon.Hp = int.Parse(jsonData["userPokemonBaseHp"].ToString());
        UserPokemonHp.text = curPokemon.CurrentHp + "/" + curPokemon.Hp;
        float userHpBarWidth = 270 * ((float)curPokemon.CurrentHp / (float)curPokemon.Hp);
        UserPokemonHpBar.sizeDelta = new Vector2(userHpBarWidth, UserPokemonHpBar.sizeDelta.y);

        int opponentCurrentHp = int.Parse(jsonData["oppoPokemonCurrentHp"].ToString());
        int opponentHp = int.Parse(jsonData["oppoPokemonBaseHp"].ToString());
        OpponentPokemonHp.text = opponentCurrentHp + "/" + opponentHp;
        float opponentHpBarWidth = 270 * (opponentCurrentHp / (float)opponentHp);
        OpponentPokemonHpBar.sizeDelta = new Vector2(opponentHpBarWidth, OpponentPokemonHpBar.sizeDelta.y);
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
        Debug.Log("Send Message: " + jsonData);
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
        string opponentPokemonPortraitPath = "Pokemon/Portrait/" + int.Parse(opponentData["id"].ToString());
        Sprite opponentPortrait = Resources.Load(opponentPokemonPortraitPath, typeof(Sprite)) as Sprite;
        OpponentPortrait.sprite = opponentPortrait;
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
            pokemon.onValueChanged.AddListener(value => OnChooseInitialPokemon(pokemon));
        }
    }


    // 选择出场的宝可梦
    void OnChooseInitialPokemon(Toggle toggle)
    {
        if (toggle.isOn)
        {
            currPokemon = int.Parse(toggle.name.Replace("pokemon", ""));
            PokemonDataSync();
            FightMessage message = new FightMessage(FightCode.SET_INITIAL_POKEMON)
            {
                CurrPokemon = currPokemon
            };
            SendData(message);
            ProhibitAllToggleAndBtn();
        }
    }

    #endregion


    // Valid 信息 允许用户进行操作
    //-----------------------------------------------------------------------

    #region [VALID]

    void SelectCurPokemonAndSkill()
    {
        PokemonToggle.isOn = true;
        PokemonDataSync();
        ProhibitPokemonToggleByHp();
        ProhibitSkillBtnByPP();

        foreach (Toggle pokemon in Pokemons)
        {
            pokemon.interactable = true;
            pokemon.onValueChanged.RemoveAllListeners();
            pokemon.onValueChanged.AddListener(value => OnClickPokemon(pokemon));
        }

        foreach (Button skill in Skills)
        {
            skill.onClick.RemoveAllListeners();
            skill.onClick.AddListener(() => OnClickSkill(skill));
        }
    }


    private void OnClickPokemon(Toggle toggle)
    {
        if (toggle.isOn)
        {
            currPokemon = int.Parse(toggle.name.Replace("pokemon", ""));
            ProhibitSkillBtnByPP();
            SwitchPanel();
        }
    }

    private void OnClickSkill(Button button)
    {
        currSkill = int.Parse(button.name.Replace("skill", ""));
        Pokemon curPokemon = GetCurPokemonByPos();
        Skill skill = curPokemon.Skills[currSkill - 1];
        if (skill != null)
        {
            skill.PP -= 1;
            SkillPPs[currSkill - 1].text = "PP: " + skill.PP;
        }

        FightMessage message = new FightMessage(FightCode.CHOICE)
        {
            CurrPokemon = currPokemon,
            CurrSkill = currSkill
        };
        ProhibitAllToggleAndBtn();
        SendData(message);
    }

    #endregion

    //-----------------------------------------------------------------------


    #region [FIGHT_MESSAGE]

    private void DisplayFightMessage(JsonData data)
    {
        FightMessage.text = data["message"].ToString();
        ProhibitAllToggleAndBtn();
    }

    #endregion

    // 通用宝可梦信息
    private void PokemonDataSync()
    {
        ProhibitPokemonToggleByHp();
        Pokemon curPokemon = GetCurPokemonByPos();
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


    // 用于点击宝可梦后，切换到技能页面
    private void SwitchPanel()
    {
        PokemonDataSync();
        PokemonToggle.isOn = false;
        SkillToggle.isOn = true;
        SkillCanvas.enabled = true;
        PokemonCanvas.enabled = false;
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
            ExitCanvas.enabled = true;
    }


    // 用户逃跑
    public void OnClickExitConfirmBtn()
    {
        FightMessage message = new FightMessage(FightCode.SURRENDER);
        SendData(message);
        _socket.CloseAsync();
        SceneManager.LoadScene("Adventure");
    }

    // 用户取消逃跑
    public void OnClickExitCancelBtn()
    {
        PokemonToggle.isOn = true;
        ExitCanvas.enabled = false;
    }


    // 用户确认输了
    public void OnClickLoseExitBtn()
    {
        _socket.CloseAsync();
        SceneManager.LoadScene("Adventure");
    }


    // 获得赢得道具
    private void SetWinItems(JsonData jsonData)
    {
        pokeballNum.text = "x" + jsonData["pokemonBall"];
        coinNum.text = "x" + jsonData["coin"];
        string imgPath = "Item/Image/" + jsonData["item"];
        itemImg.sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
    }

    // 用户确认赢了
    public void OnClickWinExitBtn()
    {
        _socket.CloseAsync();
        User.GetInstance().AdventureLevel += 1;
        SceneManager.LoadScene("Adventure");
    }


    // 通过 currPokemon 获得当前冒险的宝可梦
    private Pokemon GetCurPokemonByPos()
    {
        User user = User.GetInstance();
        return currPokemon switch
        {
            1 => user.AdventurePokemon1,
            2 => user.AdventurePokemon2,
            3 => user.AdventurePokemon3,
            _ => user.AdventurePokemon1
        };
    }


    // 禁用所有的Pokemon Toggle和Skill Btn
    private void ProhibitAllToggleAndBtn()
    {
        foreach (Button skillBtn in Skills)
            skillBtn.interactable = false;

        foreach (Toggle pokemon in Pokemons)
            pokemon.interactable = false;
    }

    // 通过宝可梦血量禁用宝可梦Toggle
    private void ProhibitPokemonToggleByHp()
    {
        User user = User.GetInstance();
        Pokemons[0].interactable = user.AdventurePokemon1.CurrentHp > 0;
        Pokemons[1].interactable = user.AdventurePokemon2.CurrentHp > 0;
        Pokemons[2].interactable = user.AdventurePokemon3.CurrentHp > 0;
    }

    // 通过当前出战的宝可梦的技能PP禁用 Skill Btn
    private void ProhibitSkillBtnByPP()
    {
        Pokemon curPokemon = GetCurPokemonByPos();
        for (int i = 0; i < Skills.Count; i++)
        {
            Skills[i].interactable = curPokemon.Skills[i].PP > 0;
        }
    }
}