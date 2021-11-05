public class BackEndConfig
{
    private static string url = "http://localhost";
    private static string port = "10922";

    public static string GetUrl()
    {
        return url + ":" + port;
    }


    public static string SetAttribute(string url, string attribute, string value)
    {
        return url + attribute + "=" + value;
    }

}
