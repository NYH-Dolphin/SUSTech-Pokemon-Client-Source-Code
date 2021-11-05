using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : BoardManager
{
    
    public UserUI user;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        user = UserUI.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePortrait(Image image)
    {
        string name = image.name.Replace("portrait", "");
        user.Portrait = int.Parse(name);
        CloseBoard();
    }
}
