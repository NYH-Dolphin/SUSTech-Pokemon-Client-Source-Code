using System.Collections;
using System.Collections.Generic;
using Script.Shop;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private static User _user;
    public GameObject itemList;
    private List<GameObject> grids = new List<GameObject>();
    private static int occupy; // 已经使用了多少个格子


    public Text pokeBall;

    public Text coin;

    // Start is called before the first frame update
    void Start()
    {
        _user = User.GetInstance();
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
        
        Shop shop = Shop.getInstance();
        foreach (var item in shop.Items)
        {
            SetGridMessage(item);
        }
    }

    private void UserDataSync()
    {
        pokeBall.text = _user.PokeBall + "";
        coin.text = _user.Coin > 1000000 ? _user.Coin / 10000 + "万" : _user.Coin + "";
    }

    private void SetGridMessage(Item item)
    {
        ItemUI itemUI = new ItemUI(item.Name, item.ID, item.Price);
        GameObject itemPrefab = itemUI.CreateShopItemPrefab();
        itemPrefab.transform.SetParent(grids[occupy].transform, false);
        occupy++;
    }
}