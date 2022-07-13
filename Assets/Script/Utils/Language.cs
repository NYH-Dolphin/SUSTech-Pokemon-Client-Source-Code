using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Utils
{
    public class Language : MonoBehaviour
    {
        public string CN; // 中文
        public string EN; // 英文
        public int CNFontSize; // 中文字体大小
        public int ENFontSize; // 英文字体大小
        public bool bChangeSize; // 选中了会更改字体大小


        private static Dictionary<GameObject, string> CNDic = new Dictionary<GameObject, string>();
        private static Dictionary<GameObject, string> ENDic = new Dictionary<GameObject, string>();

        /// <summary>
        /// 注册语言
        /// </summary>
        private void Start()
        {
            var o = gameObject;
            CNDic[o] = CN;
            ENDic[o] = EN;
            if (PlayerPrefs.GetString("language", "EN") == "CN")
            {
                if (o.transform.TryGetComponent(out Text text))
                {
                    text.text = CN;
                    if (bChangeSize)
                    {
                        text.fontSize = CNFontSize;
                    }
                }
            }
            else
            {
                if (o.transform.TryGetComponent(out Text text))
                {
                    text.text = EN;
                }
                if (bChangeSize)
                {
                    text.fontSize = ENFontSize;
                }
            }
        }

        private void OnDestroy()
        {
            var o = gameObject;
            CNDic.Remove(o);
            ENDic.Remove(o);
        }


        /// <summary>
        /// 统一设置成中文
        /// </summary>
        public static void SetChineseLanguage()
        {
            foreach (var obj in CNDic.Keys)
            {
                if (obj.transform.TryGetComponent(out Text text))
                {
                    if (obj.GetComponent<Language>().bChangeSize)
                    {
                        text.fontSize = obj.GetComponent<Language>().CNFontSize;
                    }
                    text.text = CNDic[obj];
                }
            }

            PlayerPrefs.SetString("language", "CN");
        }

        /// <summary>
        /// 统一设置成英文
        /// </summary>
        public static void SetEnglishLanguage()
        {
            foreach (var obj in CNDic.Keys)
            {
                if (obj.transform.TryGetComponent(out Text text))
                {
                    if (obj.GetComponent<Language>().bChangeSize)
                    {
                        text.fontSize = obj.GetComponent<Language>().ENFontSize;
                    }
                    text.text = ENDic[obj];
                }
            }

            PlayerPrefs.SetString("language", "EN");
        }
    }
}