using System.Collections;
using System.Collections.Generic;
using Fungus;
using Script.Pokemon;
using UnityEngine;

public class IntroManager : MonoBehaviour
{

    public Flowchart Flowchart;
    // Start is called before the first frame update
    void Start()
    {
        Flowchart.SetStringVariable("language", PlayerPrefs.GetString("language", "EN"));
    }

    // Update is called once per frame
    void Update()
    {
    }


    // 选择哪个书院
    public void OnSelectCollege(string college)
    {
        User.GetInstance().AdventurePokemon1 = new Pokemon(35);
        User.GetInstance().AdventurePokemon3 = new Pokemon(39);
        User.GetInstance().PokemonDisplay1 = 35;
        User.GetInstance().PokemonDisplay3 = 39;
        switch (college)
        {
            case "zhiren":
                User.GetInstance().PokemonDisplay2 = 7;
                User.GetInstance().AdventurePokemon2 = new Pokemon(7);
                break;
            case "shuren":
                User.GetInstance().PokemonDisplay2 = 1;
                User.GetInstance().AdventurePokemon2 = new Pokemon(1);
                break;
            case "shude":
                User.GetInstance().PokemonDisplay2 = 92;
                User.GetInstance().AdventurePokemon2 = new Pokemon(92);
                break;
            case "zhicheng":
                User.GetInstance().PokemonDisplay2 = 4;
                User.GetInstance().AdventurePokemon2 = new Pokemon(4);
                break;
            case "zhixin":
                User.GetInstance().PokemonDisplay2 = 27;
                User.GetInstance().AdventurePokemon2 = new Pokemon(27);
                break;
            case "shuli":
                User.GetInstance().PokemonDisplay2 = 99;
                User.GetInstance().AdventurePokemon2 = new Pokemon(99);
                break;
        }

        StartCoroutine(SetUserColleagueSelection(college));
    }


    IEnumerator SetUserColleagueSelection(string college)
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        form.AddField("college", college);
        string url = BackEndConfig.GetUrl() + "/userPokemon/decideCollege";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            Debug.Log("Success");
        }
    }
}