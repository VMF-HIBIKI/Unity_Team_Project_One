using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using JetBrains.Annotations;
using UIFrameWork.Core;
using UIFramework.Core;

/// <summary>
/// 无队列无历史记录仅用于显示地图Or体力槽等
/// 根据面板的优先级，将面板设置到对应的父对象下，实现面板的层级管理
/// </summary>
public class PanelUILayer : UILayer<IPanelController>
{
    [SerializeField]
    [Tooltip("优先级并行层的设置，注册到此层的面板将根据其优先级重新归属到不同的并行层对象")]
    private PanelPriorityLayerList priorityLayers = null;

    public override void ReparentScreen(IScreenController controller,Transform screenTransform)
    {
        var ctl = controller as IPanelController;
        if (ctl != null)
        {
        ReparentToParaLayer(ctl.Priority, screenTransform);
        }else
        {
        base.ReparentScreen(controller, screenTransform);
        }    
    }

    public override void ShowScreen(IPanelController screen)
    {
        screen.Show();
    }

    public override void ShowScreen<Tprops>(IPanelController screen,Tprops properties)
    {
        screen.Show(properties);
    }

    public override void HideScreen(IPanelController screen)
    {
        screen.Hide();
    }


    public bool IsPanelVisible(string panelId)
    {
        IPanelController panel;
        if (registeredScreens.TryGetValue(panelId, out panel))
        {
            return panel.IsVisible;
        }

        return false;
    }

    private void ReparentToParaLayer(PanelPriority priority,Transform screenTransform)
    {
        Transform trans;
        if(!priorityLayers.ParaLayerLookup.TryGetValue(priority,out trans))
        {
            trans = transform;
        }
        screenTransform.SetParent(trans,false);
    }

}
