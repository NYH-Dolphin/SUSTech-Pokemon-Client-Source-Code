using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Text UserInfo;
    public Image Portrait;
    public Image pokemon1;
    public Image pokemon2;
    public Image pokemon3;
    public Flowchart flowchart;

    // Start is called before the first frame update
    void Start()
    {
        UserDataSync();
        flowchart.SetStringVariable("language", PlayerPrefs.GetString("language", "EN"));
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
        ChangeDisplayPokemon(pokemon1, user.AdventurePokemon1.ID);
        ChangeDisplayPokemon(pokemon2, user.AdventurePokemon2.ID);
        ChangeDisplayPokemon(pokemon3, user.AdventurePokemon3.ID);
    }

    public void ChangeDisplayPokemon(Image pokemon, int num)
    {
        string path = "Pokemon/Portrait/" + num;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        pokemon.sprite = sprite;
        pokemon.SetNativeSize();
        pokemon.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    public void BackToTheAdventureScene()
    {
        SceneManager.LoadScene("Adventure");
    }
}
