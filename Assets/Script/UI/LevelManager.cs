using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Text UserInfo;
    public Image Portrait;
    
    // Start is called before the first frame update
    void Start()
    {
        UserDataSync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UserDataSync()
    {
        User user = User.GetInstance();
        UserInfo.text = user.Name;
        string path = "User/Portrait/p" + user.Portrait;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        Portrait.sprite = sprite;
    }


    public void BackToTheAdventureScene()
    {
        SceneManager.LoadScene("Adventure");
    }
}
