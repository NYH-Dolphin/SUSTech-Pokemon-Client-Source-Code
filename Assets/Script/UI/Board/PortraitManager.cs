using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : BoardManager
{
    
    public User user;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        user = User.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePortrait(Image image)
    {
        string name = image.name.Replace("portrait", "");
        user.SetPortrait(int.Parse(name));
        CloseBoard();
    }
}
