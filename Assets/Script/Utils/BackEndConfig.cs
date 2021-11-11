using System;
using LitJson;

public class BackEndConfig
{
    private static string url = "http://localhost";
    private static string port = "10922";

    public static string GetUrl()
    {
        return url + ":" + port;
    }


    public static void SetData(JsonData jsonData, Func<User, JsonData, bool> setting)
    {
        setting.Invoke(User.GetInstance(), jsonData);
    }
}