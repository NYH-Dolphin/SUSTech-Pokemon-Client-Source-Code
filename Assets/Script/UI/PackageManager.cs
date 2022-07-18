using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using Org.BouncyCastle.Crypto.Tls;
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


    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
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
        int curValidGrid = PackageUI.SetPackageItem(gridList, 1, 0);
        ActivateGrid(curValidGrid);
    }

    public void MaterialToggle()
    {
        int curValidGrid = PackageUI.SetPackageItem(gridList, 2, 0);
        ActivateGrid(curValidGrid);
    }

    public void BookToggle()
    {
        int curValidGrid = PackageUI.SetPackageItem(gridList, 3, 0);
        ActivateGrid(curValidGrid);
    }


    public void NextPage()
    {
        int curValidGrid = PackageUI.NextRefreshGridList();
        ActivateGrid(curValidGrid);
    }


    private void ActivateGrid(int curValidGrid)
    {
        if (curValidGrid < int.Parse(currentGrid.name.Replace("grid", "")))
        {
            PackageUI.SetItemIntro(gridList.transform.GetChild(0).gameObject);
            gridList.transform.GetChild(0).transform.GetChild(0).GetComponent<Toggle>().isOn = true;
            currentGrid = gridList.transform.GetChild(0).gameObject;
        }
        else
        {
            PackageUI.SetItemIntro(currentGrid);
        }
        
        for (int i = 0; i < gridList.transform.childCount; i++)
        {
            gridList.transform.GetChild(i).transform.GetChild(0).GetComponent<Toggle>().enabled =  i + 1 <= curValidGrid;
        }
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
}