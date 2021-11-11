using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdventureManager : MonoBehaviour
{
    
    public Image adventurePokemon1;
    public Image adventurePokemon2;
    public Image adventurePokemon3;
    // Start is called before the first frame update
    void Start()
    {
        UserDataSync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void UserDataSync()
    {
        User user = User.GetInstance();
        // 信息配置
        string pokemon1 = "Pokemon/Image/" + user.AdventurePokemon1.ID;
        string pokemon2 = "Pokemon/Image/" + user.AdventurePokemon2.ID;
        string pokemon3 = "Pokemon/Image/" + user.AdventurePokemon3.ID;
        Sprite sprite1 = Resources.Load(pokemon1, typeof(Sprite)) as Sprite;
        Sprite sprite2 = Resources.Load(pokemon2, typeof(Sprite)) as Sprite;
        Sprite sprite3 = Resources.Load(pokemon3, typeof(Sprite)) as Sprite;
        adventurePokemon1.sprite = sprite1;
        adventurePokemon2.sprite = sprite2;
        adventurePokemon3.sprite = sprite3;
    }
    
    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
