using System;
using System.Collections;
using System.Collections.Generic;
using UIFramework.Core;
using UIFramework.Window;
using UIFrameWork.Core;
using UnityEngine;

namespace UIFrameWork.Window
{
    /// <summary>
    /// 控制layer层所有的窗口
    /// 有显示记录和队列，一次只显示一个
    /// 负责窗口的显示和隐藏逻辑，管理窗口的历史记录和队列
    /// </summary>
    public class WindowLayer : UILayer<IWindowController>
    {
        [SerializeField] private WindowParaLayer priorityParaLayer = null;
        public IWindowController CurrentWindow {  get; private set; }
        private Queue<WindowHistoryEntry> windowQueue;//用于存储待显示的窗口
        private Stack<WindowHistoryEntry> windowHistory;//用于存储窗口的历史记录

        public event Action RequestScreenBlock;
        public event Action RequestScreenUnblock;//处理屏幕的阻塞和解除阻塞请求。

        private HashSet<IScreenController> screensTransitioning;//存储正在进行过渡的屏幕控制器

        private bool IsScreenTransitionInProgress
        {
            get { return screensTransitioning.Count != 0; }
        }

        public override void Initialize()
        {
            base.Initialize();
            windowQueue = new Queue<WindowHistoryEntry>();
            windowHistory = new Stack<WindowHistoryEntry>();
            screensTransitioning = new HashSet<IScreenController>();
        }
        /// <summary>
        /// 在屏幕注册时，将相应的事件处理方法添加到屏幕控制器的事件委托中
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        protected override void ProcessScreenRegister(string screenId, IWindowController controller)
        {
            base.ProcessScreenRegister(screenId, controller);
            controller.InTransitionFinished += OnInAnimationFinished;
            controller.OutTransitionFinished += OnOutAnimationFinished;
            controller.CloseRequest += OnCloseRequestedByWindow;
        }
        /// <summary>
        /// 在屏幕取消注册时，将相应的事件处理方法从屏幕控制器的事件委托中移除。
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        protected override void ProcessScreenUnRegister(string screenId, IWindowController controller)
        {
            base.ProcessScreenUnRegister(screenId, controller);
            controller.InTransitionFinished -= OnInAnimationFinished;
            controller.OutTransitionFinished -= OnOutAnimationFinished;
            controller.CloseRequest -= OnCloseRequestedByWindow;
        }

        public override void ShowScreen(IWindowController screen)
        {
            ShowScreen<IWindowProperties>(screen, null);
        }

        public override void ShowScreen<TProp>(IWindowController screen, TProp properties)
        {
            IWindowProperties windowProp = properties as IWindowProperties;

            if (ShouldEnqueue(screen, windowProp))
            {
                EnqueueWindow(screen, properties);
            }
            else
            {
                DoShow(screen, windowProp);
            }
        }

        public override void HideScreen(IWindowController screen)
        {
            if (screen == CurrentWindow)
            {
                windowHistory.Pop();
                AddTransition(screen);
                screen.Hide();

                CurrentWindow = null;

                if (windowQueue.Count > 0)
                {
                    ShowNextInQueue();
                }
                else if (windowHistory.Count > 0)
                {
                    ShowPreviousInHistory();
                }
            }
            else
            {
                Debug.LogError(
                    string.Format(
                        "[WindowUILayer] Hide requested on WindowId {0} but that's not the currently open one ({1})! Ignoring request.",
                        screen.ScreenId, CurrentWindow != null ? CurrentWindow.ScreenId : "current is null"));
            }
        }

        public override void HideAll(bool shouldAnimateWhenHiding = true)
        {
            base.HideAll(shouldAnimateWhenHiding);
            CurrentWindow = null;
            priorityParaLayer.RefreshDarken();
            windowHistory.Clear();
        }

        public override void ReparentScreen(IScreenController controller, Transform screenTransform)
        {
            IWindowController window = controller as IWindowController;

            if (window == null)
            {
                Debug.LogError("[WindowUILayer] Screen " + screenTransform.name + " is not a Window!");
            }
            else
            {
                if (window.IsPopup)
                {
                    priorityParaLayer.AddScreen(screenTransform);
                    return;
                }
            }

            base.ReparentScreen(controller, screenTransform);
        }

        private void EnqueueWindow<TProp>(IWindowController screen, TProp properties) where TProp : IScreenProperties
        {
            windowQueue.Enqueue(new WindowHistoryEntry(screen, (IWindowProperties)properties));
        }
        /// <summary>
        /// 判断屏幕是否要进入队列
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="windowProp"></param>
        /// <returns></returns>
        private bool ShouldEnqueue(IWindowController controller, IWindowProperties windowProp)
        {
            if (CurrentWindow == null && windowQueue.Count == 0)
            {
                return false;
            }

            if (windowProp != null && windowProp.SuppressPrefabProperties)
            {
                return windowProp.WindowQueuePriority != WindowPriority.ForceForeground;
            }

            if (controller.WindowPriority != WindowPriority.ForceForeground)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 展示先前在历史记录中的窗口
        /// </summary>
        private void ShowPreviousInHistory()
        {
            if (windowHistory.Count > 0)
            {
                WindowHistoryEntry window = windowHistory.Pop();
                DoShow(window);
            }
        }
        /// <summary>
        /// 展示队列中的下一个
        /// </summary>
        private void ShowNextInQueue()
        {
            if (windowQueue.Count > 0)
            {
                WindowHistoryEntry window = windowQueue.Dequeue();
                DoShow(window);
            }
        }

        private void DoShow(IWindowController screen, IWindowProperties properties)
        {
            DoShow(new WindowHistoryEntry(screen, properties));
        }

        private void DoShow(WindowHistoryEntry windowEntry)
        {
            //检查是否需要隐藏当前窗口
            if (CurrentWindow == windowEntry.Screen)
            {
                Debug.LogWarning(
                    string.Format(
                        "[WindowUILayer] The requested WindowId ({0}) is already open! This will add a duplicate to the " +
                        "history and might cause inconsistent behaviour. It is recommended that if you need to open the same" +
                        "screen multiple times (eg: when implementing a warning message pop-up), it closes itself upon the player input" +
                        "that triggers the continuation of the flow."
                        , CurrentWindow.ScreenId));
            }
            else if (CurrentWindow != null
                     && CurrentWindow.HideOnForegroundLost
                     && !windowEntry.Screen.IsPopup)
            {
                CurrentWindow.Hide();
            }

            windowHistory.Push(windowEntry);//更新窗口历史记录
            AddTransition(windowEntry.Screen);//添加过渡效果
            //处理弹出窗口的暗黑背景
            if (windowEntry.Screen.IsPopup)
            {
                priorityParaLayer.DarkenBG();
            }

            windowEntry.Show();

            CurrentWindow = windowEntry.Screen;// 更新当前窗口
        }
        //在进入动画完成后移除过渡效果
        private void OnInAnimationFinished(IScreenController screen)
        {
            RemoveTransition(screen);
        }
        //处理屏幕退出动画完成后的相关操作
        private void OnOutAnimationFinished(IScreenController screen)
        {
            RemoveTransition(screen);
            var window = screen as IWindowController;
            if (window.IsPopup)
            {
                priorityParaLayer.RefreshDarken();
            }
        }
        //响应窗口主动发起的关闭请求
        private void OnCloseRequestedByWindow(IScreenController screen)
        {
            HideScreen(screen as IWindowController);
        }
        //添加过渡效果
        private void AddTransition(IScreenController screen)
        {
            screensTransitioning.Add(screen);
            if (RequestScreenBlock != null)
            {
                RequestScreenBlock();
            }
        }
        //移除过渡效果
        private void RemoveTransition(IScreenController screen)
        {
            screensTransitioning.Remove(screen);
            if (!IsScreenTransitionInProgress)
            {
                if (RequestScreenUnblock != null)
                {
                    RequestScreenUnblock();
                }
            }
        }

    }
}
