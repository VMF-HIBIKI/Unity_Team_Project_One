using System;
using UIFramework.Window;

namespace UIFramework.Core
{
    /// <summary>
    /// 屏幕控制器接口
    /// </summary>
    public interface IScreenController
    {
        string ScreenId { get; set; }
        bool IsVisible { get;  }
        void Show(IScreenProperties props =null);
        void Hide(bool animate = true);

        Action<IScreenController> InTransitionFinished { get; set; }
        Action<IScreenController> OutTransitionFinished { get; set; }
        Action<IScreenController> CloseRequest { get; set; }
        Action<IScreenController> ScreenDestroyed { get; set; }

    }

    /// <summary>
    /// 面板控制器接口
    /// </summary>
    public interface IPanelController : IScreenController
    {
        PanelPriority Priority { get; }
    }

    public interface IWindowController : IScreenController 
    {
        bool HideOnForegroundLost {  get; }//失去焦点时是否隐藏
        bool IsPopup {  get; }//是否为弹出窗口
        WindowPriority WindowPriority {  get; }
    }

}
