using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PackageManager : MonoBehaviour
{
    public GameObject gridList; // 背包面板

    // 三个触发器
    public Toggle inEnhance;
    public Toggle inMaterial;
    public Toggle inBook;


    // Start is called before the first frame update
    void Start()
    {
        // 初始化
        SwitchToggle();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchToggle();
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    private void SwitchToggle()
    {
        if (inEnhance.isOn)
        {
            PackageUI.SetPackageItem(gridList, 1, 0);
        }else if (inMaterial.isOn)
        {
            PackageUI.SetPackageItem(gridList, 2, 0);
        }else if (inBook.isOn)
        {
            PackageUI.SetPackageItem(gridList, 3, 0);
        }
    }
}