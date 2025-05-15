using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance; // ����ʵ��
    private string currentScene; // ��ǰ��������
    private bool[] levels; // �ؿ�״̬����
    private DeadCount dc; // ��������

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        // ��ʼ��DeadCount����
        if (DeadCount.instance != null)
        {
            dc = DeadCount.instance;
        }
        else
        {
            Debug.LogError("DeadCount����δ��ʼ����");
        }
        currentScene = SaveSystem.Instance.LoadGame().currentScene;
        SaveSystem.Instance.JumpToNextLevel(currentScene);
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
