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

    public List<Button> gameLevelBtn;


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


        // user 5
        // i 4
        for (int i = 0; i < gameLevelBtn.Count; i++)
        {
            Button btn = gameLevelBtn[i];
            string imgPath;
            if (i + 1 < user.AdventureLevel)
            {
                imgPath = "Other/done_btn";
                btn.enabled = true;
            }
            else if (i + 1 == user.AdventureLevel)
            {
                imgPath = "Other/doing_btn";
                btn.enabled = true;
            }
            else
            {
                imgPath = "Other/todo_btn";
                btn.enabled = false;
            }
            Sprite sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
            btn.GetComponent<Image>().sprite = sprite;
        }
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}