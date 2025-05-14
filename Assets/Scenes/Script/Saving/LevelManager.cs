using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private string currentScene; // 当前场景名称
    private bool[] levels; // 关卡状态数组
    private DeadCount dc; // 死亡次数

    void Start()
    {
        // 初始化DeadCount引用
        if (DeadCount.instance != null)
        {
            dc = DeadCount.instance;
        }
        else
        {
            Debug.LogError("DeadCount单例未初始化！");
        }
    }
    public string GetCurrentScene
    {
         get { return currentScene; }
              
    }
    public void SetCurrentScene(string scene)
    {
         currentScene = scene;
    }
    public int GetDeadCount
    {
         get { return dc.deadCount; }
    }
    public bool[] GetLevelStatus
    {
         get { return levels; }
    }
    public void SetLevelStatus(bool[] leveles)
    {
         levels = leveles;
    }
}
