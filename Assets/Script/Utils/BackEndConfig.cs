using System;
using LitJson;

public class BackEndConfig
{
    // private static string url = "http://8.130.20.226";
    private static string url = "http://8.130.20.226";
    private static string port = "10921";

    private static string gameLogicUrl = "ws://8.130.20.226";
    private static string gameLogicPort = "10923";

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