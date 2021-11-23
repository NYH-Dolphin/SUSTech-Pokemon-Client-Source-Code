using System;
using LitJson;

public class BackEndConfig
{
    private static string url = "http://10.20.48.171";
    private static string port = "10921";

    private static string gameLogicUrl = "ws://10.21.66.18";
    private static string gameLogicLocalHost = "ws://127.0.0.1";
    private static string gameLogicPort = "8086";

    public static string GetGameLogicAddress(string userToken)
    {
        return gameLogicUrl + ":" + gameLogicPort + "/socketServer/" + userToken;
    }

    public static string GetUrl()
    {
        return url + ":" + port;
    }


    public static void SetData(JsonData jsonData, Func<User, JsonData, bool> setting)
    {
        setting.Invoke(User.GetInstance(), jsonData);
    }
}