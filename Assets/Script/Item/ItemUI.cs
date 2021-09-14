using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ItemUI : Item
{
    private string path; // item 图片地址

    public ItemUI(string name, int id, int cost)
    {
        this.name = name;
        this.id = id;
        this.cost = cost;
        path = "Item/Image/" + id;
    }
    

    /**
     * [实例化ShopItem Prefab并且配置所有内容]
     */
    public GameObject CreateShopItemPrefab()
    {
        // 实例化对象并设置各类属性
        GameObject itemPrefab = Resources.Load("Item/Prefab/shop_item") as GameObject;
        // 一定要实例化
        GameObject item = Object.Instantiate(itemPrefab);
        Text pName = item.transform.GetChild(0).GetComponent<Text>();
        Image pImage = item.transform.GetChild(1).GetComponent<Image>();
        Button pPurchase = item.transform.GetChild(2).GetComponent<Button>();
        Text pCost = pPurchase.transform.GetChild(1).GetComponent<Text>();
        pName.text = name;
        pImage.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        pCost.text = cost + "";
        return item;
    }
}