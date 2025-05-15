using System;
using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using Utils;

public class UIDemoController : MonoBehaviour
{
    [SerializeField] private UISettings defaultUISettings = null;//UI ��������ʲ������ڱ༭����ֵ������Ԥ�������Ļע���б�
    //[SerializeField] private FakePlayerData fakePlayerData = null;//ģ��������ݣ���ȼ����ȣ������ڲ��Դ��ڲ������ݡ�
    [SerializeField] private Camera cam = null;//������������ CameraProjectionWindow �Ĳ��������ԣ�
    [SerializeField] private Transform transformToFollow = null;//����Ŀ�꣨������ʾ������ڵĸ����߼���

    private UIFrame uiFrame;//UI ���ʵ����ͨ�� UISettings ����������������Ļ�������ڣ���
    /// <summary>
    /// ���ģʽ���ź�ģʽ������ EventBus��ʵ�����������磺
    /// ����ģ�鴥�� StartDemoSignal���˴���Ӧ����塣
    /// ���水ť���� NavigateToWindowSignal���˴���������ת��
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
    /// �򿪳�ʼ����
    /// </summary>
    private void Start()
    {
        uiFrame.OpenWindow(ScreenIds.StartGameWindow);
    }
    /// <summary>
    /// ��ʾ��ʼʱ���򿪳�פ������ʱ��Ϣ���
    /// </summary>
    private void OnStartDemo()
    {
        uiFrame.ShowPanel(ScreenIds.NavigationPanel);// ��ʾ������壨��פ��
        //uiFrame.ShowPanel(ScreenIds.ToastPanel);// ��ʾ��ʾ��壨��ʱ��Ϣ��
    }

    /// <summary>
    /// �������Ĵ�����ת
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
    /// ȷ�ϵ�������
    /// </summary>
    /// <param name="popupPayload"></param>
    /*private void OnShowConfirmationPopup(ConfirmationPopupProperties popupPayload)
    {
        uiFrame.OpenWindow(ScreenIds.ConfirmationPopup, popupPayload);
    }*/
}
