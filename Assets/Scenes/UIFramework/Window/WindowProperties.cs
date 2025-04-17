using TMPro;
using UIFramework.Core;
using UnityEngine;
namespace UIFramework.Window
{
    /// <summary>
    /// 窗口的通用属性
    /// </summary>
    [System.Serializable]
    public class WindowProperties:IWindowProperties
    {
        [SerializeField]
        protected bool hideOnForegroundLost = true;
        [SerializeField]
        protected WindowPriority windowQueuePriority = WindowPriority.ForceForeground;
        [SerializeField]
        protected bool isPopup = false;

        public WindowProperties()
        {
            hideOnForegroundLost = true;
            windowQueuePriority = WindowPriority.ForceForeground;
            isPopup = false;
        }

        /// <summary>
        /// 如果另一个窗口已经打开，该窗口如何表现
        /// </summary>
        /// <value>Force Foreground 会立刻打开他，Enqueue会操纵他排队，方便在当前窗口关闭后立刻打开</value>
        public WindowPriority WindowQueuePriority
        { 
            get {return windowQueuePriority;}
            set {windowQueuePriority = value;}
        }

        /// <summary>
        /// 其他窗口被前置的时候，自己是否隐藏
        /// </summary>
        public bool HideOnForegroundLost
        {
            get { return hideOnForegroundLost; }
            set { hideOnForegroundLost = value;}
        }

        /// <summary>
        /// 当在Open()中调用传递属性的时候，是否应该覆盖在ViewPrefab中配置的属性
        /// 抑制 预制体 属性
        /// </summary>
        public bool SuppressPrefabProperties { get; set; }

        /// <summary>
        /// 弹出窗口在他们后面显示一个黑色背景，并在其他所有窗口前面显示
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }

        public WindowProperties(bool suppressPrefabProperties = false)
        {
            WindowQueuePriority = WindowPriority.ForceForeground;
            HideOnForegroundLost = false;
            SuppressPrefabProperties = suppressPrefabProperties;
        }

        public WindowProperties(WindowPriority priority, bool hideOnForegroundLost = false, bool suppressPrefabProperties = false)
        {
            WindowQueuePriority = priority;
            HideOnForegroundLost = hideOnForegroundLost;
            SuppressPrefabProperties = suppressPrefabProperties;
        }
    }
}
