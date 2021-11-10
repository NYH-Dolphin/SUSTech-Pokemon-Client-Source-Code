using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField account; // 账号栏
    public InputField password; // 密码栏
    public Toggle toggle; // 明文或密文勾选
    public Text message; // 提示


    // Start is called before the first frame update
    void Start()
    {
        password.contentType = InputField.ContentType.Password;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnToggleChange()
    {
        password.contentType = toggle.isOn ? InputField.ContentType.Standard : InputField.ContentType.Password;
        password.ForceLabelUpdate();
    }


    public void OnLogin()
    {
        // 开始协程
        StartCoroutine("Login");
    }


    /**
     * 用于开始游戏 Button
     */ 
    IEnumerator Login()
    {
        if (String.IsNullOrEmpty(account.text))
        {
            message.text = "您尚未输入账号";
        }else if (String.IsNullOrEmpty(password.text))
        {
            message.text = "您尚未输入密码";
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("account", account.text);
            form.AddField("password", password.text);
            string url = BackEndConfig.GetUrl() + "/user/login";
            HttpRequest request = new HttpRequest();
            StartCoroutine(request.Post(url, form));
            while (!request.isComplete)
            {
                yield return null;
            }

            int statusCode = int.Parse(request.value["code"].ToString());
            switch (statusCode)
            {
                case 10000:
                    message.text = "";
                    User.SetInstance(request.value);
                    StartGame();
                    break;
                case 10001:
                    message.text = "您输入的账号和密码有误";
                    break;
            }
        }
    }

   

    void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}