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
            message.text = "您尚未输入账号";
        else if (String.IsNullOrEmpty(password.text))
            message.text = "您尚未输入密码";
        else if (String.IsNullOrEmpty(passwordCheck.text))
            message.text = "您尚未输入确认密码";
        else if (!password.text.Equals(passwordCheck.text))
            message.text = "您两次输入的密码不匹配";
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
            switch (statusCode)
            {
                case 10000:
                    message.text = "";
                    UserUI.SetInstance(request.value);
                    StartOP();
                    break;
                case 50000:
                    message.text = "您输入的账号已经有人注册";
                    break;
                default:
                    message.text = "服务器异常，请稍等";
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