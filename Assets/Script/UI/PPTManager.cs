using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PPTManager : MonoBehaviour
{
    public List<GameObject> helpManualPages;

    private int _pageNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        _pageNum = 0;
        OpenPage(_pageNum);
    }


    private void OpenPage(int num)
    {
        for (int i = 0; i < helpManualPages.Count; i++)
        {
            helpManualPages[i].SetActive(i == num);
        }
    }


    // Update is called once per frame
    void Update()
    {
    }


    public void OnClickPrevPage()
    {
        _pageNum = _pageNum - 1 < 0 ? _pageNum : _pageNum - 1;
        OpenPage(_pageNum);
    }
    
    public void OnClickNextPage()
    {
        _pageNum = _pageNum + 1 >= helpManualPages.Count ? _pageNum : _pageNum + 1;
        OpenPage(_pageNum);
    }


    public void BackToTheMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}