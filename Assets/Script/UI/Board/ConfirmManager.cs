using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmManager : BoardManager
{

    public Text num;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }
    

    // Update is called once per frame
    void Update()
    {
        if(num != null)
            num.text = User.GetInstance().SummonNum.ToString();
    }


    // 只有拥有足够的pokeball才可以准备抽卡
    public void OpenBoard(int limit)
    {
        if (User.GetInstance().PokeBall >= limit)
        {
            base.OpenBoard();
        }
    }

    public void OnClickCancel()
    {
        base.CloseBoard();
    }

    public void OnClickConfirm()
    {
        OpenDrawCardScene();
    }

    public void OpenDrawCardScene()
    {
        SceneManager.LoadScene("DrawCard");
    }
    
    
}
