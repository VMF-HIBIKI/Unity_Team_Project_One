using System;
using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using Utils;

public class UIDemoController : MonoBehaviour
{
    [SerializeField] private UISettings defaultUISettings = null;//UI 框架配置资产（需在编辑器赋值，包含预制体和屏幕注册列表）
    //[SerializeField] private FakePlayerData fakePlayerData = null;//模拟玩家数据（如等级进度），用于测试窗口参数传递。
    [SerializeField] private Camera cam = null;//相机组件（用于 CameraProjectionWindow 的参数化测试）
    [SerializeField] private Transform transformToFollow = null;//跟随目标（用于演示相机窗口的跟随逻辑）

    private UIFrame uiFrame;//UI 框架实例（通过 UISettings 创建，管理所有屏幕生命周期）。
    /// <summary>
    /// 设计模式：信号模式（类似 EventBus）实现组件解耦，例如：
    /// 其他模块触发 StartDemoSignal，此处响应打开面板。
    /// 界面按钮触发 NavigateToWindowSignal，此处处理窗口跳转。
    /// </summary>
    private void Awake()
    {
        uiFrame = defaultUISettings.CreateUIInstance();
        Signals.Get<StartDemoSignal>().AddListener(OnStartDemo);
        Signals.Get<NavigateToWindowSignal>().AddListener(OnNavigateToWindow);

        //Signals.Get<ShowConfirmationPopupSignal>().AddListener(OnShowConfirmationPopup);
    }


    private void OnDestroy()
    {
        Signals.Get<StartDemoSignal>().RemoveListener(OnStartDemo);
        Signals.Get<NavigateToWindowSignal>().RemoveListener(OnNavigateToWindow);

        //Signals.Get<ShowConfirmationPopupSignal>().RemoveListener(OnShowConfirmationPopup);
    }
    /// <summary>
    /// 打开初始窗口
    /// </summary>
    private void Start()
    {
        uiFrame.OpenWindow(ScreenIds.StartGameWindow);
    }
    /// <summary>
    /// 演示开始时，打开常驻面板和临时消息面板
    /// </summary>
    private void OnStartDemo()
    {
        uiFrame.ShowPanel(ScreenIds.NavigationPanel);// 显示导航面板（常驻）
        //uiFrame.ShowPanel(ScreenIds.ToastPanel);// 显示提示面板（临时消息）
    }

    /// <summary>
    /// 带参数的窗口跳转
    /// </summary>
    /// <param name="windowId"></param>
    private void OnNavigateToWindow(string windowId)
    {
        uiFrame.CloseCurrentWindow();

        switch (windowId)
        {
            case ScreenIds.BackGroundWindow:
                uiFrame.OpenWindow(windowId);
                break;
            /*case ScreenIds.CountinueWindow:
                uiFrame.OpenWindow(windowId);
                break;
            case ScreenIds.ThanksWindow:
                uiFrame.OpenWindow(windowId);
                break;*/
            default:
                uiFrame.OpenWindow(windowId);
                break;
        }
    }
    /// <summary>
    /// 确认弹窗处理
    /// </summary>
    /// <param name="popupPayload"></param>
    /*private void OnShowConfirmationPopup(ConfirmationPopupProperties popupPayload)
    {
        uiFrame.OpenWindow(ScreenIds.ConfirmationPopup, popupPayload);
    }*/
}
