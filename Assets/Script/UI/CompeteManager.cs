using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LitJson;
using MoonSharp.VsCodeDebugger.SDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityWebSocket;
using Button = UnityEngine.UI.Button;

public class CompeteManager : MonoBehaviour
{
    private IWebSocket _socket;

    public Button JoinRoomBtn;

    public InputField RoomNumber;

    public Text notice;
    private string pattern = @"^[0-9]*$";

    // Start is called before the first frame update
    void Start()
    {
        User.GetInstance().Mode = FightCode.PVP;
        WebSocketHandShaking();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void BackToMainScene()
    {
        if (_socket != null)
        {
            FightMessage message = new FightMessage(FightCode.EXIT);
            SendData(message);
            _socket.CloseAsync();
        }

        SceneManager.LoadScene("Main");
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
    }

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
        string stage = jsonData["code"].ToString();
        switch (stage)
        {
            case "CONNECTION_SUCCESS": // 连接成功
                JoinRoomBtn.enabled = true;
                break;
            case "ROOM_ALREADY_EXIST": //房间已经存在，重新输入房间号
                notice.text = "房间已存在";
                break;
            case "WAITING": //等待另一位玩家加入
                notice.text = "等待另一位玩家加入";
                break;
            case "ROOM_FOUND": // 找到房间，准备开始战斗
                User.GetInstance().PVPSocket = _socket;
                _socket.OnMessage -= SocketOnMessage;
                SceneManager.LoadScene("Fight");
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
        Debug.Log("Send Message: " + jsonData);
        _socket.SendAsync(jsonData);
    }

    #endregion


    public void OnClickJoinRoomBtn()
    {
        int roomNumber;
        if (string.IsNullOrEmpty(RoomNumber.text))
        {
            notice.text = "您的输入为空";
        }
        else if (int.TryParse(RoomNumber.text, out roomNumber) == false)
        {
            notice.text = "您输入的不是合法数字！";
        }
        else
        {
            FightMessage message = new FightMessage(FightCode.PVP)
            {
                CurrLevel = roomNumber
            };
            SendData(message);
        }
    }
}