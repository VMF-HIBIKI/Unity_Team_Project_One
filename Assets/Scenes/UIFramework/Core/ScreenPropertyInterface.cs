﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Window;

namespace UIFramework.Core
{
    /// <summary>
    /// 界面属性的接口
    /// </summary>
    public interface IScreenProperties
    {

    }
    /// <summary>
    /// 面板属性的接口
    /// </summary>
    public interface IPanelProperties : IScreenProperties
    {
        PanelPriority Priority { get; set; }
    }
    /// <summary>
    /// 窗口属性的接口
    /// </summary>
    public interface IWindowProperties : IScreenProperties
    {
        WindowPriority WindowQueuePriority { get; set; }
        bool HideOnForegroundLost { get; set; }
        bool IsPopup {  get; set; }
        bool SuppressPrefabProperties { get; set; }
    }
}
