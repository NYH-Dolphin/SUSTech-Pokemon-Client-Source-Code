using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
public class HttpTest : MonoBehaviour
{
    private string postUrl = "http://127.0.0.1:8082/exer/record";
    private const string account = "account";
    private const string password = "password";
    public Text accountText;
    public Text passwordTest;


    public void loginTest()
    {
        StartCoroutine("Post");
    }

    [System.Obsolete]
    IEnumerator Post()
    {
        WWWForm form = new WWWForm();
        form.AddField(account, accountText.text);
        form.AddField(password, passwordTest.text);
        
        UnityWebRequest request = UnityWebRequest.Post(postUrl, form);
        yield return request.SendWebRequest();
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
    }
}