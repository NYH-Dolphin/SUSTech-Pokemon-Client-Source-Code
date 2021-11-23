using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PPTManager : MonoBehaviour
{
    private int pageNum = 1;

    private int allPage = 9; // 页面数量

    public Image ppt;

    // Start is called before the first frame update
    void Start()
    {
        ChangePPT();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnClickPrevPage()
    {
        if (pageNum - 1 >= 1)
        {
            pageNum--;
            ChangePPT();
        }
    }


    // index 从0开始 
    // 一共有4页ppt 0 1 2 3
    public void OnClickNextPage()
    {
        if (pageNum + 1 <= allPage)
        {
            pageNum++;
            ChangePPT();
        }
    }


    private void ChangePPT()
    {
        string pptPath = "Other/PPT/" + pageNum;
        Sprite spriteGenre = Resources.Load(pptPath, typeof(Sprite)) as Sprite;
        ppt.sprite = spriteGenre;
    }


    public void BackToTheMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}