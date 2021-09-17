using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PackageUI : Package
{
    private static GameObject gridList;// gridList传入绑定
    private static GameObject itemIntro;// itemIntro传入绑定
    private static string path; // item 图片地址
    private static int itemNum; // 当前种类的道具一共有多少个
    private static int pageNum; // 第几页
    private static int toggleNum;// 查看的道具的种类toggle


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
     * toggleNum:1-Enhance面板 2-Material面板 3-Book面板
     * pageNum:确定是第几页
     */
    public static void SetPackageItem(GameObject gridList, int toggleNum, int pageNum)
    {
        // 设置第几页和第几个toggle
        PackageUI.pageNum = pageNum;
        PackageUI.toggleNum = toggleNum;

        // 通过 toggleNum 找到具体用哪类道具的信息和数量
        List<int> numList = new List<int>();
        List<Item> itemList = new List<Item>();
        switch (toggleNum)
        {
            case 1:
                numList = EnhanceItems.Values.ToList();
                itemList = EnhanceItems.Keys.ToList();
                break;
            case 2:
                numList = MaterialItems.Values.ToList();
                itemList = MaterialItems.Keys.ToList();
                break;
            case 3:
                numList = BookItems.Values.ToList();
                itemList = BookItems.Keys.ToList();
                break;
        }

        itemNum = itemList.Count;

        // 从哪一页开始
        int i = 9 * pageNum;
        foreach (Transform grid in gridList.transform)
        {
            Image gridImage = grid.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            if (i <= itemList.Count - 1)
            {
                path = "Item/Image/" + itemList[i].GetId();
                gridImage.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            }
            else
            {
                gridImage.sprite = Resources.Load("Item/Image/0", typeof(Sprite)) as Sprite;
            }

            i++;
        }
    }

    /*
     * [下一页grid_list的刷新]
     */
    public static void NextRefreshGridList()
    {
        if (pageNum < itemNum / 9)
        {
            pageNum++;
        }

        SetPackageItem(gridList, toggleNum, pageNum);
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
        int gridNum = int.Parse(grid.name.Replace("grid", ""));
        Item item = new Item("", 0, "");
        int num = 0; // item 的数量
        switch (toggleNum)
        {
            case 1:
                if (EnhanceItems.Keys.ToList().Count - 1 >= 9 * pageNum + gridNum - 1)
                {
                    item = EnhanceItems.Keys.ToList()[9 * pageNum + gridNum - 1];
                    num = EnhanceItems.Values.ToList()[9 * pageNum + gridNum - 1];
                }
                break;
            case 2:
                if (MaterialItems.Keys.ToList().Count - 1 >= 9 * pageNum + gridNum - 1)
                {
                    item = MaterialItems.Keys.ToList()[9 * pageNum + gridNum - 1];
                    num = MaterialItems.Values.ToList()[9 * pageNum + gridNum - 1];
                }

                break;
            case 3:
                if (BookItems.Keys.ToList().Count - 1 >= 9 * pageNum + gridNum - 1)
                {
                    item = BookItems.Keys.ToList()[9 * pageNum + gridNum - 1];
                    num = BookItems.Values.ToList()[9 * pageNum + gridNum - 1];
                }

                break;
        }
        path = "Item/Image/" + item.GetId();
        Image itemImg = itemIntro.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
        itemImg.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
        Text itemName = itemIntro.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
        itemName.text = item.GetName();
        Text description = itemIntro.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        description.text = item.GetDescription();
        Text itemNumber = itemIntro.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();
        itemNumber.text = "X" + num;
        return grid;
    }

    
    
    /*
     * [测试用数据]
     */
    public static void InitializeTest()
    {
        EnhanceItems.Add(new EnhanceItem("测试1", 1, "soidhfsoidj"), 1);
        EnhanceItems.Add(new EnhanceItem("测试2", 2, "soidhfsoidj"), 1);
        EnhanceItems.Add(new EnhanceItem("测试3", 3, "soidhfsoidj"), 1);
        EnhanceItems.Add(new EnhanceItem("测试4", 4, "soidhfsoidj"), 1);



        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);
        MaterialItems.Add(new MaterialItem("测试2", 2, "sdifuwf0upds"), 2);


        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
        BookItems.Add(new BookItem("测试3", 3, "weufwefhfoafoa"), 3);
    }
}