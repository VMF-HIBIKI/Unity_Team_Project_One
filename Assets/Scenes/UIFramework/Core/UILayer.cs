using UnityEngine;
using UIFramework.Core;
using System.Collections.Generic;

namespace UIFrameWork.Core
{
    public abstract class UILayer<Tscreen> :MonoBehaviour where Tscreen : IScreenController
    {
        protected Dictionary<string, Tscreen> registeredScreens;
        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="screen">界面类型参数</param>
        public abstract void ShowScreen(Tscreen screen);
        /// <summary>
        /// 带参显示界面
        /// </summary>
        /// <typeparam name="Tprops">属性类型</typeparam>
        /// <param name="screen">界面类型参数</param>
        /// <param name="properties">属性类型参数</param>
        public abstract void ShowScreen<Tprops>(Tscreen screen, Tprops properties) where Tprops : IScreenProperties;
        /// <summary>
        /// 隐藏界面
        /// </summary>
        /// <param name="screen">界面类型参数</param>
        public abstract void HideScreen(Tscreen screen);
        /// <summary>
        /// 初始化Layer层
        /// </summary>
        public virtual void Initialize()
        {
            registeredScreens = new Dictionary<string, Tscreen>(); 
        }

        /// <summary>
        /// 将传入的界面当做层的子节点
        /// </summary>
        /// <param name="controller">界面controller</param>
        /// <param name="screenTransform">界面节点</param>
        public virtual void ReparentScreen(IScreenController controller,Transform screenTransform) 
        {
            screenTransform.SetParent(transform,false);
        }

        /// <summary>
        /// 注册界面的controller带上明确的界面id
        /// </summary>
        /// <param name="screenId">界面Id</param>
        /// <param name="controller">界面controller</param>
        public void RegisterScreen(string screenId,Tscreen controller)
        {
            if (!registeredScreens.ContainsKey(screenId))
            {
                ProcessScreenRegister(screenId, controller);
            }
            else {
                Debug.LogError("[AUILayerController] Screen controller already registered for id:" + screenId);    
            }
        }
        /// <summary>
        /// 根据id取消注册界面的controller
        /// </summary>
        /// <param name="screenId"></param>
        /// <param name="controller"></param>
        public void UnregisterScreen(string screenId,Tscreen controller)
        {
            if (registeredScreens.ContainsKey(screenId))
            {
                ProcessScreenUnRegister(screenId, controller);
            }
            else
            {
                Debug.LogError("[AUILayerController] Screen controller not registered for id:" + screenId);
            }
        }

        /// <summary>
        /// 根据id显示界面的controller
        /// </summary>
        /// <param name="screenId"></param>
        public void ShowScreenById(string screenId)
        {
            Tscreen ctl;
            if(registeredScreens.TryGetValue(screenId,out ctl))
            {
                ShowScreen(ctl);
            }
            else
            {
                Debug.LogError("[AUILayerController] Screen Id:" + screenId + "not registered to this layer!");
            }
        }

        /// <summary>
        /// 根据id显示界面的controller,带上具体属性参数
        /// </summary>
        /// <typeparam name="Tprops"></typeparam>
        /// <param name="screenId"></param>
        /// <param name="properties"></param>
        public void ShowScreenById<Tprops>(string screenId,Tprops properties) where Tprops : IScreenProperties
        {
            Tscreen ctl;
            if (registeredScreens.TryGetValue(screenId, out ctl))
            {
                ShowScreen(ctl,properties);
            }
            else
            {
                Debug.LogError("[AUILayerController] Screen Id:" + screenId + "not registered!");
            }
        }
        /// <summary>
        /// 根据id隐藏界面的controller
        /// </summary>
        /// <param name="screenId"></param>
        public void HideScreenById(string screenId)
        {
            Tscreen ctl;
            if (registeredScreens.TryGetValue(screenId, out ctl))
            {
                HideScreen(ctl);
            }
            else
            {
                Debug.LogError("[AUILayerController] Could not hide Screen Id:" + screenId + "as it is not registered!");
            }
        }


        /// <summary>
        /// 查看id是否已经注册过了
        /// </summary>
        /// <param name="screenId"></param>
        /// <returns></returns>
        public bool IsScreenRegistered(string screenId)
        {
            return registeredScreens.ContainsKey(screenId);
        }

        /// <summary>
        /// 隐藏所有界面
        /// </summary>
        /// <param name="shouldAnimateWhenHiding">隐藏时是否需要动画</param>
        public virtual void HideAll(bool shouldAnimateWhenHiding = true)
        {
            foreach(var screen in registeredScreens)
            {
                screen.Value.Hide(shouldAnimateWhenHiding);
            }
        }

        protected virtual void ProcessScreenRegister(string screenId,Tscreen controller)
        {
            controller.ScreenId = screenId;
            registeredScreens.Add(screenId,controller);
            controller.ScreenDestroyed +=OnScreenDestroyed;
        }

        protected virtual void ProcessScreenUnRegister(string screenId, Tscreen controller)
        {
            controller.ScreenDestroyed -=OnScreenDestroyed;
            registeredScreens.Remove(screenId);
        }

        private void OnScreenDestroyed(IScreenController screen)
        {
            if (!string.IsNullOrEmpty(screen.ScreenId) && registeredScreens.ContainsKey(screen.ScreenId))
            {
                UnregisterScreen(screen.ScreenId, (Tscreen)screen);
            }
        }
    }
}
