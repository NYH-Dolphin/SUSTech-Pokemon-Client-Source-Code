using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private static UserUI _user;
    public GameObject itemList;
    private List<GameObject> grids = new List<GameObject>();
    private static int occupy; // 已经使用了多少个格子


    public Text pokeBall;

    public Text coin;

    // Start is called before the first frame update
    void Start()
    {
        _user = UserUI.GetInstance();
        InitializeShop();
        UserDataSync();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }


    private void InitializeShop()
    {
        occupy = 0;
        // 找到格子
        foreach (Transform grid in itemList.transform)
        {
            // 实例化
            grids.Add(grid.gameObject);
        }

        SetGridMessage("草系钻石", 1, 1000);
        SetGridMessage("四叶草", 2, 2000);
        SetGridMessage("绿色水晶", 3, 3000);
        SetGridMessage("黄色珍珠", 4, 500);
    }

    private void UserDataSync()
    {
        pokeBall.text = _user.GetPokeBall() + "";
        coin.text = _user.GetCoin() > 1000000 ? _user.GetCoin() / 10000 + "万" : _user.GetCoin() + "";
    }

    private void SetGridMessage(string name, int id, int cost)
    {
        ItemUI item = new ItemUI(name, id, cost);
        GameObject itemPrefab = item.CreateShopItem();
        itemPrefab.transform.SetParent(grids[occupy].transform, false);
        occupy++;
    }
}