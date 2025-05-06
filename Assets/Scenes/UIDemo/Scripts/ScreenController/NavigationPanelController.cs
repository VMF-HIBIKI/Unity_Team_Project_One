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

    // һ����˵AddListeners��RemoveListeners���ǳɶԳ��ֵģ���add������remove
    protected override void AddListeners()
    {
        Signals.Get<NavigateToWindowSignal>().AddListener(OnExternalNavigation);
    }

    protected override void RemoveListeners()
    {
        Signals.Get<NavigateToWindowSignal>().RemoveListener(OnExternalNavigation);
    }

    /// <summary>
    /// �������ʱ���������������
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

        // Ĭ��ѡ�е�һ����ť
        OnNavigationButtonClicked(currentButtons[0]);
    }
    /// <summary>
    /// ����������ϵİ�ť�����ʱ���á�����ͨ��NavigateToWindowSignal�źŹ㲥�������ť��Ӧ��Ŀ����Ļ��ʶ��֪ͨ�������������е���������
    /// Ȼ��������е�ǰ��ť������ÿ����ť��SetCurrentNavigationTarget���������°�ť�ĵ�ǰ����Ŀ��״̬
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
    /// �ⲿ�����źŴ�����
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
