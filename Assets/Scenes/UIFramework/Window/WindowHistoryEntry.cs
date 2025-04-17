using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Core;

namespace UIFramework.Window
{
    /// <summary>
    /// 用于记录窗口的历史信息
    /// </summary>
    public struct WindowHistoryEntry
    {
        public readonly IWindowController Screen;
        public readonly IWindowProperties Properties;
        
        public WindowHistoryEntry(IWindowController screen,IWindowProperties properties)
        {
            Screen = screen;
            Properties = properties;
        }
        public void Show()
        {
            Screen.Show(Properties);
        }
    }
}
