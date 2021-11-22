using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler: MonoBehaviour
{
    public void Awake()
    {
        DestroyBgm();
    }

    private void DestroyBgm()
    {
        foreach (var bgm in GetDontDestroyOnLoadGameObjects())
            if(bgm.name != "~LeanTween")
                Destroy(bgm);
    }

    private GameObject[] GetDontDestroyOnLoadGameObjects()
    {
        var allGameObjects = new List<GameObject>();
        allGameObjects.AddRange(FindObjectsOfType<GameObject>());
        //移除所有场景包含的对象
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            for (var j = 0; j < objs.Length; j++)
            {
                allGameObjects.Remove(objs[j]);
            }
        }

        //移除父级不为null的对象
        int k = allGameObjects.Count;
        while (--k >= 0)
        {
            if (allGameObjects[k].transform.parent != null)
            {
                allGameObjects.RemoveAt(k);
            }
        }

        return allGameObjects.ToArray();
    }
}