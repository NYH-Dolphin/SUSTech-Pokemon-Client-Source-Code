using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField account; // 账号栏
    public InputField password;// 密码栏
    public Toggle toggle;// 明文或密文勾选
    public Text message; // 提示


   
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        PasswordChange();
    }

    
    
    void PasswordChange()
    {
        password.contentType = toggle.isOn? InputField.ContentType.Standard : InputField.ContentType.Password;
        password.ForceLabelUpdate();
    }


    /**
     * 用于开始游戏 Button
     */
    public void Login()
    {
        User.SetFreshInstance(account.text, password.text);
        int checkSum = User.CheckLogin();
        switch (checkSum)
        {
            case 0:
                message.text = "";
                StartGame();
                break;
            case 1:
                message.text = "您尚未输入账号";
                break;
            case 2:
                message.text = "您尚未输入密码";
                break;
            case 3:
                message.text = "您输入的账号和密码有误";
                break;
        }
    }
    
    


    void StartGame()
    {
        SceneManager.LoadScene("Main");
    }




}
