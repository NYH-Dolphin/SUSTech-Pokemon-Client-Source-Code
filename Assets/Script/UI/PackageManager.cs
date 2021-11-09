using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PackageManager : MonoBehaviour
{
    public GameObject gridList; // 背包面板
    public GameObject itemIntro; // 道具介绍面板
    public GameObject currentGrid;

    // 三个触发器
    public Toggle inEnhance;
    public Toggle inMaterial;
    public Toggle inBook;

    // 三类Items，用于传入PackageUI进行绑定
    HashMap<Item, int> MedicineItems = new HashMap<Item, int>();
    HashMap<Item, int> ExperienceItems = new HashMap<Item, int>();
    HashMap<Item, int> MaterialItems = new HashMap<Item, int>();

    private void Awake()
    {
        StartCoroutine("GetAllItems");
    }

    // Start is called before the first frame update
    void Start()
    {
        // 测试用
        PackageUI.InitializeTest(MedicineItems, MaterialItems, ExperienceItems);
        // 绑定
        PackageUI.BindGameObject(gridList, itemIntro);
        // 初始化
        PackageUI.SetPackageItem(gridList, 1, 0);
        currentGrid = PackageUI.SetItemIntro(gridList.transform.GetChild(0).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }


    public void EnhanceToggle()
    {
        PackageUI.SetPackageItem(gridList, 1, 0);
        PackageUI.SetItemIntro(currentGrid);
    }

    public void MaterialToggle()
    {
        PackageUI.SetPackageItem(gridList, 2, 0);
        PackageUI.SetItemIntro(currentGrid);
    }

    public void BookToggle()
    {
        PackageUI.SetPackageItem(gridList, 3, 0);
        PackageUI.SetItemIntro(currentGrid);
    }


    public void NextPage()
    {
        PackageUI.NextRefreshGridList();
        PackageUI.SetItemIntro(currentGrid);
    }

    public void PrevPage()
    {
        PackageUI.PrevRefreshGridList();
        PackageUI.SetItemIntro(currentGrid);
    }


    public void SetItemIntro(GameObject grid)
    {
        currentGrid = PackageUI.SetItemIntro(grid);
    }


    /**
     * 搜索获得账号中所有的物品！
     */
    IEnumerator GetAllItems()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", UserUI.GetInstance().Token);
        string url = BackEndConfig.GetUrl() + "/knapsack/my";
        HttpRequest request = new HttpRequest();
        StartCoroutine(request.Post(url, form));
        while (!request.isComplete)
        {
            yield return null;
        }

        int statusCode = int.Parse(request.value["code"].ToString());
        if (statusCode == 10000)
        {
            JsonData jsonData = request.value["data"];
            for (int i = 0; i < jsonData.Count; i++)
            {
                JsonData jsonItem = jsonData[i]["item"];
                Item item = new Item(jsonItem["name"].ToString(), int.Parse(jsonItem["id"].ToString()));
                item.Description = jsonItem["description"] == null ? "暂时为空" : jsonItem["description"].ToString();
                int number = int.Parse(jsonData[i]["num"].ToString());
                switch (jsonItem["type"].ToString())
                {
                    case "experience":
                        ExperienceItems.Add(item, number);
                        break;
                    case "material":
                        MaterialItems.Add(item, number);
                        break;
                    case "medicine":
                        MedicineItems.Add(item, number);
                        break;
                }
            }
        }
    }
}