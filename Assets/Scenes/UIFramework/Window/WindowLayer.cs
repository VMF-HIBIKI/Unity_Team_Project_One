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
    /// ����layer�����еĴ���
    /// ����ʾ��¼�Ͷ��У�һ��ֻ��ʾһ��
    /// ���𴰿ڵ���ʾ�������߼��������ڵ���ʷ��¼�Ͷ���
    /// </summary>
    public class WindowLayer : UILayer<IWindowController>
    {
        [SerializeField] private WindowParaLayer priorityParaLayer = null;
        public IWindowController CurrentWindow {  get; private set; }
        private Queue<WindowHistoryEntry> windowQueue;//���ڴ洢����ʾ�Ĵ���
        private Stack<WindowHistoryEntry> windowHistory;//���ڴ洢���ڵ���ʷ��¼

        public event Action RequestScreenBlock;
        public event Action RequestScreenUnblock;//������Ļ�������ͽ����������

        private HashSet<IScreenController> screensTransitioning;//�洢���ڽ��й��ɵ���Ļ������

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
        /// ����Ļע��ʱ������Ӧ���¼���������ӵ���Ļ���������¼�ί����
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
        /// ����Ļȡ��ע��ʱ������Ӧ���¼�����������Ļ���������¼�ί�����Ƴ���
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
        /// �ж���Ļ�Ƿ�Ҫ�������
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
        /// չʾ��ǰ����ʷ��¼�еĴ���
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
        /// չʾ�����е���һ��
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
            //����Ƿ���Ҫ���ص�ǰ����
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

            windowHistory.Push(windowEntry);//���´�����ʷ��¼
            AddTransition(windowEntry.Screen);//��ӹ���Ч��
            //���������ڵİ��ڱ���
            if (windowEntry.Screen.IsPopup)
            {
                priorityParaLayer.DarkenBG();
            }

            windowEntry.Show();

            CurrentWindow = windowEntry.Screen;// ���µ�ǰ����
        }
        //�ڽ��붯����ɺ��Ƴ�����Ч��
        private void OnInAnimationFinished(IScreenController screen)
        {
            RemoveTransition(screen);
        }
        //������Ļ�˳�������ɺ����ز���
        private void OnOutAnimationFinished(IScreenController screen)
        {
            RemoveTransition(screen);
            var window = screen as IWindowController;
            if (window.IsPopup)
            {
                priorityParaLayer.RefreshDarken();
            }
        }
        //��Ӧ������������Ĺر�����
        private void OnCloseRequestedByWindow(IScreenController screen)
        {
            HideScreen(screen as IWindowController);
        }
        //��ӹ���Ч��
        private void AddTransition(IScreenController screen)
        {
            screensTransitioning.Add(screen);
            if (RequestScreenBlock != null)
            {
                RequestScreenBlock();
            }
        }
        //�Ƴ�����Ч��
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
