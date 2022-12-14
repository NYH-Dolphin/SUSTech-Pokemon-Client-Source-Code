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

    private List<string> fightMessageList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        currStage = "START"; // 开始状态
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
        if (User.GetInstance().Mode == FightCode.PVP)
        {
            _socket = User.GetInstance().PVPSocket;
            _socket.OnOpen += SocketOnOpen;
            _socket.OnMessage += SocketOnMessage;
            _socket.OnClose += SocketOnClose;
            _socket.OnError += SocketOnError;
        }
        else if (User.GetInstance().Mode == FightCode.PVE)
        {
            string address = BackEndConfig.GetGameLogicAddress(User.GetInstance().Token);
            Debug.Log(address);
            _socket = new WebSocket(address);
            _socket.OnOpen += SocketOnOpen;
            _socket.OnMessage += SocketOnMessage;
            _socket.OnClose += SocketOnClose;
            _socket.OnError += SocketOnError;
            _socket.ConnectAsync();
        }
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
                ProhibitAllToggleAndBtn();
                StateMessage.text = PlayerPrefs.GetString("language", "EN") == "CN"
                    ? "连接成功"
                    : "Connection Success";
                SelectMode(User.GetInstance().Mode); // 选择模式
                break;
            case "INITIALIZATION": // 设置信息
                Initial(jsonData); // 配置初始信息
                ProhibitPokemonToggleByHp();
                ProhibitAllSkillBtn();
                FightMessage.text = StateMessage.text = PlayerPrefs.GetString("language", "EN") == "CN"
                    ? "准备开始！"
                    : "Ready To Start";
                StateMessage.text = StateMessage.text = PlayerPrefs.GetString("language", "EN") == "CN"
                    ? "初始化信息"
                    : "Intial Information";
                InitSetCurPokemon(); // 初始化敌我双方的宝可梦信息
                break;
            case "VALID":
                PokemonDataSync();
                ProhibitPokemonToggleByHp();
                ProhibitSkillBtnByPP();
                PokemonToggle.isOn = true;
                StateMessage.text = StateMessage.text = PlayerPrefs.GetString("language", "EN") == "CN"
                    ? "到你的回合！"
                    : "Your round!";
                SelectCurPokemonAndSkill(); // 允许选择宝可梦
                break;
            case "FORBIDDEN":
                ProhibitAllToggleAndBtn();
                StateMessage.text = StateMessage.text = PlayerPrefs.GetString("language", "EN") == "CN"
                    ? "对手的回合！"
                    : "Opponent's round!";
                if (User.GetInstance().Mode == FightCode.PVP)
                {
                    OpponentDataSync(jsonData["monsterInfo"]);
                }

                break;
            case "ROUND_INFO": // 通知双方宝可梦扣血的信息
                DisplayRoundInfo(jsonData);
                break;
            case "FIGHT_MESSAGE": // 传详细的文字战斗信息
                DisplayFightMessage(jsonData);
                break;
            case "LOSE":
                if (User.GetInstance().Mode == FightCode.PVP)
                {
                    FightMessage message = new FightMessage(FightCode.EXIT);
                    SendData(message);
                }

                ProhibitAllToggleAndBtn();
                StateMessage.text = PlayerPrefs.GetString("language", "EN") == "CN"
                    ? "战斗失败..."
                    : "You Lose...";
                LoseCanvas.enabled = true;
                break;
            case "WIN":
                if (User.GetInstance().Mode == FightCode.PVP)
                {
                    FightMessage message = new FightMessage(FightCode.EXIT);
                    SendData(message);
                }

                ProhibitAllToggleAndBtn();
                StateMessage.text = PlayerPrefs.GetString("language", "EN") == "CN"
                    ? "战斗胜利！"
                    : "You Win!";
                SetWinItems(jsonData);
                WinCanvas.enabled = true;
                break;
            default:
                Debug.Log(currStage);
                break;
        }
    }

    private void DisplayRoundInfo(JsonData jsonData)
    {
        Pokemon curPokemon = GetCurPokemonByPos();
        curPokemon.CurrentHp = int.Parse(jsonData["userPokemonCurrentHp"].ToString());
        curPokemon.CurrentHp = curPokemon.CurrentHp < 0 ? 0 : curPokemon.CurrentHp;
        curPokemon.CurrentHp = curPokemon.CurrentHp > curPokemon.Hp ? curPokemon.Hp : curPokemon.CurrentHp;
        curPokemon.Hp = int.Parse(jsonData["userPokemonBaseHp"].ToString());
        UserPokemonHp.text = curPokemon.CurrentHp + "/" + curPokemon.Hp;
        float userHpBarWidth = 270 * ((float)curPokemon.CurrentHp / (float)curPokemon.Hp);
        UserPokemonHpBar.sizeDelta = new Vector2(userHpBarWidth, UserPokemonHpBar.sizeDelta.y);

        int opponentCurrentHp = int.Parse(jsonData["oppoPokemonCurrentHp"].ToString());
        int opponentHp = int.Parse(jsonData["oppoPokemonBaseHp"].ToString());
        opponentCurrentHp = opponentCurrentHp < 0 ? 0 : opponentCurrentHp;
        opponentCurrentHp = opponentCurrentHp > opponentHp ? opponentHp : opponentCurrentHp;
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
        OpponentDataSync(jsonData["monsterInfo"]);
        OpponentPokemonHp.text = jsonData["monsterInfo"]["baseHp"] + "/" + jsonData["monsterInfo"]["baseHp"];
    }

    // 设置对面的信息
    void OpponentDataSync(JsonData opponentData)
    {
        string opponentPokemonPortraitPath = "Pokemon/Portrait/" + int.Parse(opponentData["id"].ToString());
        Sprite opponentPortrait = Resources.Load(opponentPokemonPortraitPath, typeof(Sprite)) as Sprite;
        OpponentPortrait.sprite = opponentPortrait;
        string pokemonImgPath = "Pokemon/Pixel/Front/" + int.Parse(opponentData["id"].ToString());
        Sprite pokemonSprite = Resources.Load(pokemonImgPath, typeof(Sprite)) as Sprite;
        OpponentPokemonImg.sprite = pokemonSprite;
        OpponentPokemonName.text = PlayerPrefs.GetString("language", "EN") == "CN"
            ? opponentData["name"].ToString()
            : opponentData["name_en"].ToString();
        OpponentPokemonLevel.text = "Lv." + opponentData["level"];
    }


    // 初始化时设置出场宝可梦
    void InitSetCurPokemon()
    {
        StateMessage.text = PlayerPrefs.GetString("language", "EN") == "CN"
            ? "选择出战宝可梦"
            : "Select Pokemon";
        // 为所有 pokemon toggle 添加监听事件
        foreach (Toggle pokemon in Pokemons)
        {
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
            currStage = "FINISH_INITIALIZATION";
            ProhibitAllToggleAndBtn();
        }
    }

    #endregion


    // Valid 信息 允许用户进行操作
    //-----------------------------------------------------------------------

    #region [VALID]

    void SelectCurPokemonAndSkill()
    {
        if (GetCurPokemonByPos().CurrentHp <= 0)
        {
            if (User.GetInstance().AdventurePokemon1.CurrentHp > 0)
            {
                currPokemon = 1;
                Pokemons[0].isOn = true;
            }
            else if (User.GetInstance().AdventurePokemon2.CurrentHp > 0)
            {
                currPokemon = 2;
                Pokemons[1].isOn = true;
            }
            else if (User.GetInstance().AdventurePokemon3.CurrentHp > 0)
            {
                currPokemon = 3;
                Pokemons[2].isOn = true;
            }
        }

        foreach (Toggle pokemon in Pokemons)
        {
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
            if (currStage == "VALID")
            {
                currPokemon = int.Parse(toggle.name.Replace("pokemon", ""));
                PokemonDataSync();
                ProhibitSkillBtnByPP();
                SwitchPanel();
            }
        }
    }

    private void OnClickSkill(Button button)
    {
        if (currStage == "VALID")
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
            SendData(message);
            currStage = "FINISH_CHOICE";
            ProhibitAllSkillBtn();
        }
    }

    #endregion

    //-----------------------------------------------------------------------


    #region [FIGHT_MESSAGE]

    private void DisplayFightMessage(JsonData data)
    {
        if (fightMessageList.Count >= 8)
        {
            fightMessageList.RemoveAt(0);
        }

        fightMessageList.Add(data["description"].ToString());
        string displayTest = "";
        foreach (var message in fightMessageList)
        {
            displayTest += message + "\n";
        }

        FightMessage.text = displayTest;
    }

    #endregion

    // 通用宝可梦信息
    private void PokemonDataSync()
    {
        Pokemon curPokemon = GetCurPokemonByPos();
        string pokemonImgPath = "Pokemon/Pixel/Back/" + curPokemon.ID;
        Sprite pokemonSprite = Resources.Load(pokemonImgPath, typeof(Sprite)) as Sprite;
        UserPokemonImg.sprite = pokemonSprite;
        UserPokemonName.text = PlayerPrefs.GetString("language", "EN") == "CN" ? curPokemon.Name : curPokemon.Name_EN;
        UserPokemonLevel.text = "Lv." + curPokemon.Level;
        UserPokemonHp.text = curPokemon.CurrentHp + "/" + curPokemon.Hp;
        for (int i = 0; i < 4; i++)
        {
            SkillNames[i].text = PlayerPrefs.GetString("language", "EN") == "CN" ? curPokemon.Skills[i].Name:curPokemon.Skills[i].Name_EN ;
            if (PlayerPrefs.GetString("language", "EN") == "CN")
            {
                SkillNames[i].fontSize = 20;
            }
            else
            {
                SkillNames[i].fontSize = 15;
            }
            SkillPPs[i].text = "PP: " + curPokemon.Skills[i].PP;
        }

        float userHpBarWidth = 270 * ((float)curPokemon.CurrentHp / (float)curPokemon.Hp);
        UserPokemonHpBar.sizeDelta = new Vector2(userHpBarWidth, UserPokemonHpBar.sizeDelta.y);
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


    // 切换到宝可梦页面
    public void OnClickPokemonToggle(Toggle toggle)
    {
        if (!toggle.isOn) return;
        switch (currStage)
        {
            case "START":
                ProhibitAllPokemonToggle();
                break;
            case "INITIALIZATION":
                ProhibitPokemonToggleByHp();
                break;
            case "VALID":
                ProhibitPokemonToggleByHp();
                break;
            case "FORBIDDEN":
                ProhibitAllPokemonToggle();
                break;
            default:
                ProhibitAllPokemonToggle();
                break;
        }

        PokemonCanvas.enabled = true;
        SkillCanvas.enabled = false;
    }

    // 切换到技能页面
    public void OnClickSkillToggle(Toggle toggle)
    {
        if (!toggle.isOn) return;
        PokemonDataSync();
        switch (currStage)
        {
            case "START":
                ProhibitAllSkillBtn();
                break;
            case "INITIALIZATION":
                ProhibitAllSkillBtn();
                break;
            case "VALID":
                ProhibitSkillBtnByPP();
                break;
            case "FORBIDDEN":
                ProhibitAllSkillBtn();
                break;
            default:
                ProhibitAllSkillBtn();
                break;
        }

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
        if (User.GetInstance().Mode == FightCode.PVE)
        {
            FightMessage message = new FightMessage(FightCode.SURRENDER);
            SendData(message);
        }
        else if (User.GetInstance().Mode == FightCode.PVP)
        {
            FightMessage message = new FightMessage(FightCode.EXIT);
            SendData(message);
        }


        _socket.CloseAsync();
        User.GetInstance().ResetAdventurePokemonPP();
        if (User.GetInstance().Mode == FightCode.PVE)
        {
            SceneManager.LoadScene("Adventure");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
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
        User.GetInstance().ResetAdventurePokemonPP();
        if (User.GetInstance().Mode == FightCode.PVE)
        {
            SceneManager.LoadScene("Adventure");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }


    // 获得赢得道具
    private void SetWinItems(JsonData jsonData)
    {
        JsonData reward = jsonData["reward"];
        try
        {
            pokeballNum.text = "x" + reward["pokemonBall"];
            User.GetInstance().PokeBall += int.Parse(reward["pokemonBall"].ToString());
        }
        catch
        {
            pokeballNum.text = "x0";
        }

        coinNum.text = "x" + reward["coin"];
        User.GetInstance().Coin += int.Parse(reward["coin"].ToString());
        string imgPath = "Item/Image/" + reward["item"];
        itemImg.sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
    }

    // 用户确认赢了
    public void OnClickWinExitBtn()
    {
        _socket.CloseAsync();
        if (User.GetInstance().AdventureLevel == User.GetInstance().CurrentLevel)
        {
            User.GetInstance().AdventureLevel += 1;
        }

        User.GetInstance().ResetAdventurePokemonPP();
        if (User.GetInstance().Mode == FightCode.PVE)
        {
            SceneManager.LoadScene("Adventure");
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }


    // 通过 currPokemon 获得当前冒险的宝可梦
    public Pokemon GetCurPokemonByPos()
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


    // 禁用情况
    //----------------------------------------------
    // 禁用所有的Pokemon Toggle和Skill Btn
    private void ProhibitAllToggleAndBtn()
    {
        ProhibitAllPokemonToggle();
        ProhibitAllSkillBtn();
    }

    // 禁用所有Pokemon toggle
    private void ProhibitAllPokemonToggle()
    {
        foreach (Toggle pokemon in Pokemons)
            pokemon.interactable = false;
    }


    // 禁用所有Skill toggle
    private void ProhibitAllSkillBtn()
    {
        foreach (Button skill in Skills)
            skill.interactable = false;
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
    //----------------------------------------------
}