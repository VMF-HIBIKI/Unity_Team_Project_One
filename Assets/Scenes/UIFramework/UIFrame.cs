using System;
using UIFramework.Core;
using UIFrameWork.Window;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    /// <summary>
    /// ���еĶ���ӿڶ����⣬�൱��UIManager֮���
    /// </summary>
    public class UIFrame : MonoBehaviour
    {
        [Tooltip("��������ֶ���ʼ����UI��ܣ��뽫������Ϊfalse")]
        [SerializeField] private bool initializeOnAwake = true;

        private PanelUILayer panelLayer;
        private WindowLayer windowLayer;

        private Canvas mainCanvas;
        private GraphicRaycaster graphicRaycaster;

        /// <summary>
        /// �����أ��õ�ʱ���ȡ��Canvas
        /// </summary>
        public Canvas MainCanvas
        {
            get
            {
                if (mainCanvas == null)
                {
                    mainCanvas = GetComponent<Canvas>();
                }

                return mainCanvas;
            }
        }

        /// <summary>
        /// �����أ��õ�ʱ���ȡ��Canvas�������
        /// </summary>
        public Camera UICamera
        {
            get { return MainCanvas.worldCamera; }
        }

        private void Awake()
        {
            if (initializeOnAwake)
            {
                Initialize();
            }
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        public virtual void Initialize()
        {
            if (panelLayer == null)
            {
                panelLayer = gameObject.GetComponentInChildren<PanelUILayer>(true);
                if (panelLayer == null)
                {
                    Debug.LogError("[UI Frame] UI Frame lacks Panel Layer!");
                }
                else
                {
                    panelLayer.Initialize();
                }
            }

            if (windowLayer == null)
            {
                windowLayer = gameObject.GetComponentInChildren<WindowLayer>(true);
                if (windowLayer == null)
                {
                    Debug.LogError("[UI Frame] UI Frame lacks Window Layer!");
                }
                else
                {
                    windowLayer.Initialize();
                    windowLayer.RequestScreenBlock += OnRequestScreenBlock;
                    windowLayer.RequestScreenUnblock += OnRequestScreenUnblock;
                }
            }

            graphicRaycaster = MainCanvas.GetComponent<GraphicRaycaster>();
        }

        /// <summary>
        /// ��ͨ��id��ʾһ�����
        /// </summary>
        public void ShowPanel(string screenId)
        {
            panelLayer.ShowScreenById(screenId);
        }

        /// <summary>
        /// ͨ��id��������ʾ���
        /// </summary>
        public void ShowPanel<T>(string screenId, T properties) where T : IPanelProperties
        {
            panelLayer.ShowScreenById<T>(screenId, properties);
        }

        /// <summary>
        /// ��ͨ��id�������
        /// </summary>
        public void HidePanel(string screenId)
        {
            panelLayer.HideScreenById(screenId);
        }

        /// <summary>
        /// ��ͨ��id��ʾ����
        /// </summary>
        public void OpenWindow(string screenId)
        {
            windowLayer.ShowScreenById(screenId);
        }

        /// <summary>
        /// ��ͨ��id�رմ���
        /// </summary>
        public void CloseWindow(string screenId)
        {
            windowLayer.HideScreenById(screenId);
        }

        /// <summary>
        /// �رյ�ǰ�Ĵ���
        /// </summary>
        public void CloseCurrentWindow()
        {
            if (windowLayer.CurrentWindow != null)
            {
                CloseWindow(windowLayer.CurrentWindow.ScreenId);
            }
        }

        /// <summary>
        /// ����id�򿪴��ڲ��Ҵ������Բ���
        /// </summary>
        public void OpenWindow<T>(string screenId, T properties) where T : IWindowProperties
        {
            windowLayer.ShowScreenById<T>(screenId, properties);
        }

        /// <summary>
        /// ���ΰ�װ�˷�������id����ʾ���ѵ�ʲô��ʲô��
        /// </summary>
        /// <param name="screenId">The Screen id.</param>
        public void ShowScreen(string screenId)
        {
            Type type;
            if (IsScreenRegistered(screenId, out type))
            {
                if (type == typeof(IWindowController))
                {
                    OpenWindow(screenId);
                }
                else if (type == typeof(IPanelController))
                {
                    ShowPanel(screenId);
                }
            }
            else
            {
                Debug.LogError(string.Format("Tried to open Screen id {0} but it's not registered as Window or Panel!",
                    screenId));
            }
        }

        /// <summary>
        /// ע��һ�����棬�������������Ϊ���������ӽڵ�
        /// </summary>
        public void RegisterScreen(string screenId, IScreenController controller, Transform screenTransform)
        {
            IWindowController window = controller as IWindowController;
            if (window != null)
            {
                windowLayer.RegisterScreen(screenId, window);
                if (screenTransform != null)
                {
                    windowLayer.ReparentScreen(controller, screenTransform);
                }

                return;
            }

            IPanelController panel = controller as IPanelController;
            if (panel != null)
            {
                panelLayer.RegisterScreen(screenId, panel);
                if (screenTransform != null)
                {
                    panelLayer.ReparentScreen(controller, screenTransform);
                }
            }
        }

        /// <summary>
        /// ע��һ����壬��ע������ʾ��������
        /// </summary>
        public void RegisterPanel<TPanel>(string screenId, TPanel controller) where TPanel : IPanelController
        {
            panelLayer.RegisterScreen(screenId, controller);
        }

        /// <summary>
        /// ע��һ�����
        /// </summary>
        public void UnregisterPanel<TPanel>(string screenId, TPanel controller) where TPanel : IPanelController
        {
            panelLayer.UnregisterScreen(screenId, controller);
        }

        /// <summary>
        /// ע��һ�����ڣ�ͬ����ע����ʾ������
        /// </summary>
        public void RegisterWindow<TWindow>(string screenId, TWindow controller) where TWindow : IWindowController
        {
            windowLayer.RegisterScreen(screenId, controller);
        }

        /// <summary>
        /// ע������
        /// </summary>
        public void UnregisterWindow<TWindow>(string screenId, TWindow controller) where TWindow : IWindowController
        {
            windowLayer.UnregisterScreen(screenId, controller);
        }

        /// <summary>
        /// �������id����Ƿ�����
        /// </summary>
        public bool IsPanelOpen(string panelId)
        {
            return panelLayer.IsPanelVisible(panelId);
        }

        /// <summary>
        /// �������н���
        /// </summary>
        public void HideAll(bool animate = true)
        {
            CloseAllWindows(animate);
            HideAllPanels(animate);
        }

        /// <summary>
        /// ������������Ľ���
        /// </summary>
        public void HideAllPanels(bool animate = true)
        {
            panelLayer.HideAll(animate);
        }

        /// <summary>
        /// �������д��ڲ�Ľ���
        /// </summary>
        public void CloseAllWindows(bool animate = true)
        {
            windowLayer.HideAll(animate);
        }

        /// <summary>
        /// �������Ƿ�ע�����
        /// </summary>
        public bool IsScreenRegistered(string screenId)
        {
            if (windowLayer.IsScreenRegistered(screenId))
            {
                return true;
            }

            if (panelLayer.IsScreenRegistered(screenId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ������һ����ֻ�������˸����͵ķ���
        /// </summary>
        public bool IsScreenRegistered(string screenId, out Type type)
        {
            if (windowLayer.IsScreenRegistered(screenId))
            {
                type = typeof(IWindowController);
                return true;
            }

            if (panelLayer.IsScreenRegistered(screenId))
            {
                type = typeof(IPanelController);
                return true;
            }

            type = null;
            return false;
        }

        private void OnRequestScreenBlock()
        {
            if (graphicRaycaster != null)
            {
                graphicRaycaster.enabled = false;
            }
        }

        private void OnRequestScreenUnblock()
        {
            if (graphicRaycaster != null)
            {
                graphicRaycaster.enabled = true;
            }
        }
    }
}
