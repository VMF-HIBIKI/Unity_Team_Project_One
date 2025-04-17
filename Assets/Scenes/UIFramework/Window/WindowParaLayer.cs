using System.Collections.Generic;
using UnityEngine;

namespace UIFramework.Window
{
    /// <summary>
    /// 辅助层级显示二次确认的小弹窗，方便显示更高优先级的窗口
    /// 默认情况下包含任何标记为弹出窗口的窗口，由WindowUILayer控制
    /// </summary>
    public class WindowParaLayer : MonoBehaviour
    {
        [SerializeField]
        private GameObject darkenBgObject = null;
        
        private List<GameObject> containedScreens = new List<GameObject>();//存储包含的屏幕对象

        public void AddScreen(Transform screenRectTransform)
        {
            screenRectTransform.SetParent(transform, false);
            containedScreens.Add(screenRectTransform.gameObject);
        }

        /// <summary>
        /// 检查是否有激活的屏幕对象。如果有，则激活暗黑层
        /// </summary>
        public void RefreshDarken()
        {
            for (int i = 0; i < containedScreens.Count; i++)
            {
                if (containedScreens[i] != null)
                {
                    if (containedScreens[i].activeSelf)
                    {
                        darkenBgObject.SetActive(true);
                        return;
                    }
                }
            }
            darkenBgObject.SetActive(false);
        }

        public void DarkenBG()
        {
            darkenBgObject.SetActive(true);
            darkenBgObject.transform.SetAsLastSibling();
        }

    }
}
