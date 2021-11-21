using UnityEngine;

class MusicPlayer : MonoBehaviour
{
    private static GameObject _bgm;

    void Start()
    {
        DontDestroyOnLoad(this);
    }


    public static GameObject GetBgm(string musicName)
    {
        // 实例化对象并设置各类属性
        return Resources.Load("Music/" + musicName) as GameObject;
    }
}