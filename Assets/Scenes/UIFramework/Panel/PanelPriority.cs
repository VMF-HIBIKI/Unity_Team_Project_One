using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �涨���㼶
/// </summary>
public enum PanelPriority
{
    None=0,
    Prioritray=1,
    Tutorial=2,
    Blocker=3
}

/// <summary>
/// ������ȶ�����ڣ���ʾһ��������ȼ����б����Ŀ
/// </summary>
[System.Serializable]
public class PanelPriorityLayerListEntry
{
    [SerializeField]
    [Tooltip("ָ������㼶�����ȼ�")]
    private PanelPriority priority;
    [SerializeField]
    [Tooltip("ָ������㼶�����ȼ�")]
    private Transform targetParent;

    public Transform TargetParent
    {
        get {return targetParent;}
        set { targetParent = value; }
    }

    public PanelPriority Priority
    {
        get { return priority; }
        set { priority = value; }
    }

    public PanelPriorityLayerListEntry(PanelPriority prio, Transform parent)
    {
        priority = prio;
        targetParent = parent;
    }
}

/// <summary>
/// ����������ȼ����б�
/// </summary>
[System.Serializable]
public class PanelPriorityLayerList
{
    [SerializeField]
    [Tooltip("�����������ȼ����Ҳ��洢��Ӧ��GameObject.��Ⱦ���ȼ�����ЩGameObject�ڲ㼶�ṹ�е�˳�����")]
    private List<PanelPriorityLayerListEntry> paraLayers = null;
    private Dictionary<PanelPriority, Transform> lookup;

    public Dictionary<PanelPriority, Transform> ParaLayerLookup
    {
        get
        {
            if (lookup == null || lookup.Count == 0)
            {
                CacheLookup();
            }
            
            return lookup;
        }
    }

    private void CacheLookup()
    {
        lookup = new Dictionary<PanelPriority, Transform>();
        for (int i = 0; i < paraLayers.Count; i++)
        {
            lookup.Add(paraLayers[i].Priority, paraLayers[i].TargetParent);
        }
    }

    public PanelPriorityLayerList(List<PanelPriorityLayerListEntry> entries) 
    {
        paraLayers = entries;
    }   

}

