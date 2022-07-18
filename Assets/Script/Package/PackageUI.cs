using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PackageUI
{
    private static GameObject gridList; // gridList传入绑定
    private static GameObject itemIntro; // itemIntro传入绑定
    private static string path; // item 图片地址
    private static int itemNum; // 当前种类的道具一共有多少个
    private static int pageNum; // 第几页
    private static int toggleNum; // 查看的道具的种类toggle
    


    /*
     * [绑定GameObject]
     */
    public static void BindGameObject(GameObject gridList, GameObject itemIntro)
    {
        PackageUI.gridList = gridList;
        PackageUI.itemIntro = itemIntro;
    }

    /*
     * [配置Package面板的道具]
     * gridList:Package 面板下的 grid_list 对象
     * toggleNum:1-Medicine面板 2-Material面板 3-Experience面板
     * pageNum:确定是第几页
     */
    public static int SetPackageItem(GameObject gridList, int toggleNum, int pageNum)
    {
        // 设置第几页和第几个toggle
        PackageUI.pageNum = pageNum;
        PackageUI.toggleNum = toggleNum;
        User user = User.GetInstance();

        // 通过 toggleNum 找到具体用哪类道具的信息和数量
        List<Item> itemList = new List<Item>();
        switch (toggleNum)
        {
            case 1:
                itemList = user.Package.MedicineItems.Keys.ToList();
                break;
            case 2:
                itemList = user.Package.MaterialItems.Keys.ToList();
                break;
            case 3:
                itemList = user.Package.ExperienceItems.Keys.ToList();
                break;
        }

        itemNum = itemList.Count;

        // 从哪一页开始
        int i = 9 * pageNum;
        int validGridCnt = 0;
        foreach (Transform grid in gridList.transform)
        {
            Image gridImage = grid.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            if (i <= itemList.Count - 1)
            {
                path = "Item/Image/" + itemList[i].GetId();
                grid.transform.GetChild(0).GetComponent<Toggle>().enabled = true;
                gridImage.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
                validGridCnt += 1;
            }
            else
            {
                gridImage.sprite = Resources.Load("Item/Image/null", typeof(Sprite)) as Sprite;
            }

            i++;
        }
        return validGridCnt;
    }

    /*
     * [下一页grid_list的刷新]
     */
    public static int NextRefreshGridList()
    {
        if (pageNum < itemNum / 9)
        {
            pageNum++;
        }

        int curValidGrid = SetPackageItem(gridList, toggleNum, pageNum);
        return curValidGrid;
    }

    /*
     * [上一页grid_list的刷新]
     */
    public static void PrevRefreshGridList()
    {
        pageNum = pageNum > 0 ? pageNum - 1 : pageNum;
        SetPackageItem(gridList, toggleNum, pageNum);
    }


    /*
     * [根据哪个gridList中的Toggle被选中，而确定ItemIntro中的内容]
     * grid:具体传入的grid
     */
    public static GameObject SetItemIntro(GameObject grid)
    {
        User user = User.GetInstance();
        int gridNum = int.Parse(grid.name.Replace("grid", ""));
        Item item = new Item("", 0, "");
        int num = 0; // item 的数量
        switch (toggleNum)
        {
            case 1:
                if (user.Package.MedicineItems.Keys.ToList().Count - 1 >= 9 * pageNum + gridNum - 1)
                {
                    item = user.Package.MedicineItems.Keys.ToList()[9 * pageNum + gridNum - 1];
                    num = user.Package.MedicineItems.Values.ToList()[9 * pageNum + gridNum - 1];
                }

                break;
            case 2:
                if (user.Package.MaterialItems.Keys.ToList().Count - 1 >= 9 * pageNum + gridNum - 1)
                {
                    item = user.Package.MaterialItems.Keys.ToList()[9 * pageNum + gridNum - 1];
                    num = user.Package.MaterialItems.Values.ToList()[9 * pageNum + gridNum - 1];
                }

                break;
            case 3:
                if (user.Package.ExperienceItems.Keys.ToList().Count - 1 >= 9 * pageNum + gridNum - 1)
                {
                    item = user.Package.ExperienceItems.Keys.ToList()[9 * pageNum + gridNum - 1];
                    num = user.Package.ExperienceItems.Values.ToList()[9 * pageNum + gridNum - 1];
                }

                break;
        }

        path = "Item/Image/" + item.ID;
        Image itemImg = itemIntro.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        itemImg.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        Text itemName = itemIntro.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        itemName.text = PlayerPrefs.GetString("language") == "CN"? item.Name: item.Name_EN;
        Text description = itemIntro.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        description.text = PlayerPrefs.GetString("language") == "CN"? item.Description : item.Description_EN;
        Text itemNumber = itemIntro.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();
        itemNumber.text = "X" + num;
        return grid;
    }

    
}