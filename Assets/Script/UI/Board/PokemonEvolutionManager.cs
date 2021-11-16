using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Pokemon;
using UnityEngine;
using UnityEngine.UI;

public class PokemonEvolutionManager : MonoBehaviour
{
    public List<Image> Materials;
    public List<Text> Nums;
    public Image NextPokemon;
    public Canvas MessageCanvas;
    public Text Message;


    private int curPokemonShowNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        PokemonDataSync();
    }

    // Update is called once per frame
    void Update()
    {
        if (curPokemonShowNum != User.GetInstance().PokemonShowNum)
        {
            PokemonDataSync();
        }
    }

    public void PokemonDataSync()
    {
        User user = User.GetInstance();
        int index = user.PokemonShowNum;
        curPokemonShowNum = index;
        Pokemon pokemon = user.Pokemons[index];

        List<int> materials = pokemon.EvolveMap.Keys.ToList();

        // 有没突破的情况
        if (pokemon.NextID != -1)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                string materialPath = "Item/Image/" + materials[i];
                Sprite materialSprite = Resources.Load(materialPath, typeof(Sprite)) as Sprite;
                Materials[i].sprite = materialSprite;
                Nums[i].text = pokemon.EvolveMap.Get(materials[i]).ToString();
            }
        }
        else // 已突破
        {
            for (int i = 0; i < 3; i++)
            {
                string materialPath = "Item/Image/0";
                Sprite materialSprite = Resources.Load(materialPath, typeof(Sprite)) as Sprite;
                Materials[i].sprite = materialSprite;
                Nums[i].text = "";
            }
        }


        string nextPokemonPath = "Pokemon/Image/" + pokemon.NextID;
        Sprite nextPokemonSprite = Resources.Load(nextPokemonPath, typeof(Sprite)) as Sprite;
        NextPokemon.sprite = nextPokemonSprite;
        NextPokemon.SetNativeSize();
        NextPokemon.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }


    public void OnClickEvolutionBtn()
    {
        StartCoroutine(EvolutePokemon());
    }


    public void OpenMessageCanvas()
    {
        MessageCanvas.enabled = true;
    }


    public void OnClickCrossBtn()
    {
        MessageCanvas.enabled = false;
    }

    IEnumerator EvolutePokemon()
    {
        User user = User.GetInstance();
        Pokemon pokemon = user.Pokemons[curPokemonShowNum];

        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        form.AddField("pokemon", pokemon.ID);
        string url = BackEndConfig.GetUrl() + "/userPokemon/evolve";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        OpenMessageCanvas();
        switch (statusCode)
        {
            case 10000:
                Message.text = "进化成功！";
                StartCoroutine(GetAllUserPokemons());
                PokemonDataSync();
                break;
            case 10004:
                Message.text = "进化材料不足！";
                break;
            case 10006:
                Message.text = "宝可梦等级不够！";
                break;
        }
    }


    /**
     * 搜索获得账号中所有的宝可梦
     */
    IEnumerator GetAllUserPokemons()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", User.GetInstance().Token);
        string url = BackEndConfig.GetUrl() + "/userPokemon/my";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            User.SetPokemons(request.value["data"]);
        }
    }
}