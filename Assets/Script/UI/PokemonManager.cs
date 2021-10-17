using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PokemonManager : MonoBehaviour
{
    public List<Toggle> ToggleList; // 四个toggle

    public List<Canvas> CanvasList;

    // Start is called before the first frame update
    void Start()
    {
        // 先把第0个Canvas设置显示
        CanvasList[0].enabled = true;
        for (int i = 1; i < CanvasList.Count; i++)
            CanvasList[i].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    
    // 切换某个Canvas到enable状态
    private void EnableCanvas(int num)
    {
        for (int i = 0; i < CanvasList.Count; i++)
        {
            CanvasList[i].enabled = (i == num);
        }
    }

    public void IntroToggle()
    {
        EnableCanvas(0);
    }

    public void UpgradeToggle()
    {
        EnableCanvas(1);
    }

    public void Evolution_Canvas()
    {
        EnableCanvas(2);
    }

    public void TalentCanvas()
    {
        EnableCanvas(3);
    }
}