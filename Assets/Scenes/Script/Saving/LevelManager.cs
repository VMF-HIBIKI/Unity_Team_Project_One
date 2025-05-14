using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private string currentScene; // ��ǰ��������
    private bool[] levels; // �ؿ�״̬����
    private DeadCount dc; // ��������

    void Start()
    {
        // ��ʼ��DeadCount����
        if (DeadCount.instance != null)
        {
            dc = DeadCount.instance;
        }
        else
        {
            Debug.LogError("DeadCount����δ��ʼ����");
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
