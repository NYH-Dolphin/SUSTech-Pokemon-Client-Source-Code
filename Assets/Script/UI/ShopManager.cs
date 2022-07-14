using System.Collections;
using System.Collections.Generic;
using Script.Shop;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // BuyCanvas的东西
    public Canvas buyCanvas; // 购买Canvas
    public Text message; // 购买时候提示的消息
    public Button certificateBtn; // 确定购买的Btn


    private static User _user;
    private List<GameObject> grids = new List<GameObject>();
    private static int occupy; // 已经使用了多少个格子


    // ShopCanvas的数据
    public Text pokeBall;
    public Text coin;
    public GameObject itemList;

    // Start is called before the first frame update
    void Start()
    {
        buyCanvas.enabled = false;
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
        shop.Items.ForEach(item => SetGridMessage(item));
    }

    // 同步User的数据
    private void UserDataSync()
    {
        pokeBall.text = _user.PokeBall + "";
        coin.text = _user.Coin > 1000000 ? _user.Coin / 10000 + "万" : _user.Coin + "";
    }

    // 将Gird里面的数据Set进去
    private void SetGridMessage(Item item)
    {
        GameObject itemBtn = CreateShopItemPrefab(item);
        itemBtn.transform.SetParent(grids[occupy].transform, false);
        occupy++;
    }

    /**
     * [实例化ShopItem Prefab并且配置所有内容]
     */
    private GameObject CreateShopItemPrefab(Item item)
    {
        // 实例化对象并设置各类属性
        GameObject itemPrefab = Resources.Load("Item/Prefab/shop_item") as GameObject;
        // 一定要实例化
        GameObject itemBtn = Instantiate(itemPrefab);
        Text pName = itemBtn.transform.GetChild(0).GetComponent<Text>();
        Image pImage = itemBtn.transform.GetChild(1).GetComponent<Image>();
        // 设置btn的点击事件
        Button pPurchase = itemBtn.transform.GetChild(2).GetComponent<Button>();
        pPurchase.onClick.AddListener(() => OpenBuyCanvas(item.ID, item.Name, item.Price));
        Text pCost = pPurchase.transform.GetChild(1).GetComponent<Text>();
        pName.text = PlayerPrefs.GetString("language") == "CN"? item.Name: item.Name_EN;
        string imgPath = "Item/Image/" + item.ID;
        pImage.sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
        pCost.text = item.Price + "";
        return itemBtn;
    }


    // 打开BuyCanvas的时候记录itemId和price
    private int _itemId;
    private int _price;

    private void OpenBuyCanvas(int itemId, string itemName, int price)
    {
        _itemId = itemId;
        _price = price;

        buyCanvas.enabled = true;
        if (price > User.GetInstance().Coin)
        {
            message.text = PlayerPrefs.GetString("language") == "CN" ? "您的金币不够！" : "You don't have enough coin";
            certificateBtn.interactable = false;
        }
        else
        {
            message.text = PlayerPrefs.GetString("language") == "CN" ? $"您是否要购买[{itemName}]？" : $"Are you sure to purchase [{itemName}]?";
            certificateBtn.interactable = true;
        }
    }

    // 点击[取消]按钮关闭面板
    public void CloseBuyCanvas()
    {
        buyCanvas.enabled = false;
    }

    // 点击[确定]购买
    public void OnClickBuy()
    {
        StartCoroutine(BuyOneItem(_itemId, _price));
    }


    // 点击[确认]按钮-购买物品
    IEnumerator BuyOneItem(int itemId, int price)
    {
        WWWForm form = new WWWForm();
        string url = BackEndConfig.GetUrl() + "/shop/buyOne";
        form.AddField("token", User.GetInstance().Token);
        form.AddField("item", itemId);
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));

        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            User.GetInstance().Coin -= price;
            UserDataSync();
            Debug.Log("Success");
        }
    }
    
}