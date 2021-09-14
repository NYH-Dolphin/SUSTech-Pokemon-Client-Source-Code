using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PackageUI : Package
{
    private static string path; // item 图片地址

    public static void SetPackageItem(GameObject gridList, int toggleNum, int pageNum)
    {
        // 测试用 - 以后删除
        InializeTest();
        
        // 通过 toggleNum 找到具体用哪类道具的idList
        List<int> idList = new List<int>();
        switch (toggleNum)
        {
            case 1:
                idList = EnhanceItems.Values.ToList();
                break;
            case 2:
                idList = MaterialItems.Values.ToList();
                break;
            case 3:
                idList = BookItems.Values.ToList();
                break;
        }
        
        // 从哪一页开始
        int i =  9 * pageNum;
        foreach (Transform grid in gridList.transform)
        {
            if (i <= idList.Count - 1)
            {
                path = "Item/Image/" + idList[i];
                Image gridImage = grid.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
                gridImage.sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            }
            i++;
        }
    }

    private static void InializeTest()
    {
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 1);
        EnhanceItems.Add(new EnhanceItem(), 2);
        EnhanceItems.Add(new EnhanceItem(), 3);
        EnhanceItems.Add(new EnhanceItem(), 2);
        EnhanceItems.Add(new EnhanceItem(), 2);
        
        
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 2);
        MaterialItems.Add(new MaterialItem(), 4);
        MaterialItems.Add(new MaterialItem(), 1);
        MaterialItems.Add(new MaterialItem(), 3);
        MaterialItems.Add(new MaterialItem(), 4);
        MaterialItems.Add(new MaterialItem(), 1);
        
        BookItems.Add(new BookItem(), 3);
        BookItems.Add(new BookItem(), 3);
        BookItems.Add(new BookItem(), 3);
        BookItems.Add(new BookItem(), 3);
        BookItems.Add(new BookItem(), 3);
        BookItems.Add(new BookItem(), 3);
        BookItems.Add(new BookItem(), 3);
        BookItems.Add(new BookItem(), 3);
        BookItems.Add(new BookItem(), 3);
    }
}