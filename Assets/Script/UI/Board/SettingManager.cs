using System.Collections;
using System.Collections.Generic;
using Script.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingManager : BoardManager
{
    public Toggle LanguageToggle;


    public override void OpenBoard()
    {
        base.OpenBoard();
        LanguageToggle.isOn = PlayerPrefs.GetString("language", "EN") == "EN";
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnChangeLanguage()
    {
        if (LanguageToggle.isOn)
        {
            PlayerPrefs.SetString("language", "EN");
            Language.SetEnglishLanguage();
        }
        else
        {
            PlayerPrefs.SetString("language", "CN");
            Language.SetChineseLanguage();
        }
    }

    // 退出游戏
    public void OnClickExitGameBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }
}