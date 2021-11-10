using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : BoardManager
{
    
    private User _user;
    public Image portrait; // 头像
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _user = User.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    /*
     * [点击头像框按钮改变头像]
     */
    public void ChangePortrait(Image image)
    {
        string index = image.name.Replace("portrait", "");
        _user.Portrait = int.Parse(index);
        string path = "User/Portrait/p" + _user.Portrait;
        Sprite sprite = Resources.Load(path,typeof(Sprite)) as Sprite;
        portrait.sprite = sprite;
        CloseBoard();
    }
}
