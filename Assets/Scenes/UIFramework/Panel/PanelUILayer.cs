using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using JetBrains.Annotations;
using UIFrameWork.Core;
using UIFramework.Core;

/// <summary>
/// �޶�������ʷ��¼��������ʾ��ͼOr�����۵�
/// �����������ȼ�����������õ���Ӧ�ĸ������£�ʵ�����Ĳ㼶����
/// </summary>
public class PanelUILayer : UILayer<IPanelController>
{
    [SerializeField]
    [Tooltip("���ȼ����в�����ã�ע�ᵽ�˲����彫���������ȼ����¹�������ͬ�Ĳ��в����")]
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
