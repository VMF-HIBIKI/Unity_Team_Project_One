﻿
namespace UIFramework.Window
{

    /// <summary>
    /// 枚举类型，用于定义窗口在打开时，在历史记录和队列中的行为
    /// </summary>
    public enum WindowPriority
    {
        ForceForeground=0,//强制将窗口置于前台显示
        Enqueue =1,//按照队列顺序依次显示
    }
}
