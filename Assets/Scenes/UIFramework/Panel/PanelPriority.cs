using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 规定面板层级
/// </summary>
public enum PanelPriority
{
    None=0,
    Prioritray=1,
    Tutorial=2,
    Blocker=3
}

/// <summary>
/// 面板优先队列入口，表示一个面板优先级层列表的条目
/// </summary>
[System.Serializable]
public class PanelPriorityLayerListEntry
{
    [SerializeField]
    [Tooltip("指定下面层级的优先级")]
    private PanelPriority priority;
    [SerializeField]
    [Tooltip("指定下面层级的优先级")]
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
/// 管理面板优先级层列表
/// </summary>
[System.Serializable]
public class PanelPriorityLayerList
{
    [SerializeField]
    [Tooltip("根据面板的优先级查找并存储对应的GameObject.渲染优先级由这些GameObject在层级结构中的顺序决定")]
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

