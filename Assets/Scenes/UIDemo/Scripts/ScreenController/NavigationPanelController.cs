using System;
using System.Collections.Generic;
using UIFramework;
using UIFramework.Examples;
using Utils;
using UnityEngine;

[Serializable]
public class NavigationPanelEntry
{
    [SerializeField] private Sprite sprite = null;
    [SerializeField] private string buttonText = "";
    [SerializeField] private string targetScreen = "";

    public Sprite Sprite
    {
        get { return sprite; }
    }

    public string ButtonText
    {
        get { return buttonText; }
    }

    public string TargetScreen
    {
        get { return targetScreen; }
    }
}

public class NavigateToWindowSignal : ASignal<string> { }
public class NavigationPanelController : PanelController
{
    [SerializeField]
    private List<NavigationPanelEntry> navigationTargets = new List<NavigationPanelEntry>();
    [SerializeField]
    private NavigationPanelButton templateButton = null;

    private readonly List<NavigationPanelButton> currentButtons = new List<NavigationPanelButton>();

    // 一般来说AddListeners和RemoveListeners都是成对出现的，别add完忘记remove
    protected override void AddListeners()
    {
        Signals.Get<NavigateToWindowSignal>().AddListener(OnExternalNavigation);
    }

    protected override void RemoveListeners()
    {
        Signals.Get<NavigateToWindowSignal>().RemoveListener(OnExternalNavigation);
    }

    /// <summary>
    /// 当界面打开时候，这个函数被调用
    /// </summary>
    protected override void OnPropertiesSet()
    {
        ClearEntries();
        foreach (var target in navigationTargets)
        {
            var newBtn = Instantiate(templateButton);
            newBtn.transform.SetParent(templateButton.transform.parent, false);
            newBtn.SetData(target);
            newBtn.gameObject.SetActive(true);
            newBtn.ButtonClicked += OnNavigationButtonClicked;
            currentButtons.Add(newBtn);
        }

        // 默认选中第一个按钮
        OnNavigationButtonClicked(currentButtons[0]);
    }
    /// <summary>
    /// 当导航面板上的按钮被点击时调用。首先通过NavigateToWindowSignal信号广播被点击按钮对应的目标屏幕标识，通知其他相关组件进行导航操作；
    /// 然后遍历所有当前按钮，调用每个按钮的SetCurrentNavigationTarget方法，更新按钮的当前导航目标状态
    /// </summary>
    /// <param name="currentlyClickedButton"></param>
    private void OnNavigationButtonClicked(NavigationPanelButton currentlyClickedButton)
    {
        Signals.Get<NavigateToWindowSignal>().Dispatch(currentlyClickedButton.Target);
        foreach (var button in currentButtons)
        {
            button.SetCurrentNavigationTarget(currentlyClickedButton);
        }   
    }
    /// <summary>
    /// 外部导航信号处理方法
    /// </summary>
    /// <param name="screenId"></param>
    private void OnExternalNavigation(string screenId)
    {
        foreach (var button in currentButtons)
        {
            button.SetCurrentNavigationTarget(screenId);
        }
    }

    private void ClearEntries()
    {
        foreach (var button in currentButtons)
        {
            button.ButtonClicked -= OnNavigationButtonClicked;
            Destroy(button.gameObject);
        }
    }
}
