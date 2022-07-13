using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RegisterManager : BoardManager
{
    public InputField account;
    public InputField password;
    public InputField passwordCheck;
    public Toggle passwordToggle;
    public Toggle passwordCheckToggle;
    public Text message;


    void Start()
    {
        base.Start();
        Initialize();
    }

    
    
    public void OnPasswordCheckToggleChange()
    {
        SwitchContext(passwordCheck, passwordCheckToggle);
    }

    
    
    public void OnPasswordToggleChange()
    {
        SwitchContext(password, passwordToggle);
    }

    public void OnRegister()
    {
        StartCoroutine("Register");
    }

    IEnumerator Register()
    {
        if (String.IsNullOrEmpty(account.text))
            message.text = PlayerPrefs.GetString("language") == "CN" ? "您尚未输入账号" : "You don't enter the account";
        else if (String.IsNullOrEmpty(password.text))
            message.text = PlayerPrefs.GetString("language") == "CN" ? "您尚未输入密码" : "You don't enter the password";
        else if (String.IsNullOrEmpty(passwordCheck.text))
            message.text = PlayerPrefs.GetString("language") == "CN" ? "您尚未输入确认密码" : "You don't enter the confirm password";
        else if (!password.text.Equals(passwordCheck.text))
            message.text = PlayerPrefs.GetString("language") == "CN" ? "您两次输入的密码不匹配" : "Passwords don't match";
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("account", account.text);
            form.AddField("password", password.text);
            string url = BackEndConfig.GetUrl() + "/user/register";
            HttpRequest request = new HttpRequest();
            StartCoroutine(request.Post(url, form));
            while (!request.isComplete)
            {
                yield return null;
            }

            int statusCode = int.Parse(request.value["code"].ToString());
            Debug.Log("status code " + statusCode);
            switch (statusCode)
            {
                case 10000:
                    message.text = "";
                    PlayerPrefs.SetString("account", account.text);
                    PlayerPrefs.SetString("password", password.text);
                    User.SetInstance(request.value);
                    StartOP();
                    break;
                case 10007:
                    message.text = PlayerPrefs.GetString("language") == "CN" ? "您输入的账号已经有人注册" : "This account has been registered";
                    break;
                default:
                    message.text = PlayerPrefs.GetString("language") == "CN" ? "服务器异常，请稍等" : "Servers Error. Please wait";
                    break;
            }
        }
    }


    /**
     * 初始化密码为密文状态
     */
    private void Initialize()
    {
        password.contentType = InputField.ContentType.Password;
        passwordCheck.contentType = InputField.ContentType.Password;
        message.text = "";
    }

    /**
     * 通过toggle切换field为密文/明文
     */
    private static void SwitchContext(InputField field, Toggle toggle)
    {
        field.contentType = toggle.isOn? InputField.ContentType.Standard : InputField.ContentType.Password;
        field.ForceLabelUpdate();
    }
    
    void StartOP()
    {
        SceneManager.LoadScene("Op");
    }


    
}