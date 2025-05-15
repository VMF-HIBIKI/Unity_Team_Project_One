using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string savePath => Path.Combine(Application.persistentDataPath, "savegame.json");

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("�浵·����" + Application.persistentDataPath);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // ������Ϸ����
    public void SaveGame( int count,  string scene, bool[] levels)
    {
        GameData data = new GameData(count, scene, levels);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("��Ϸ�ѱ��棺" + savePath);
    }
    public void JumpToNextLevel(string currentScene)
    {
        switch (currentScene)
        {
            case "level1":
                // ����ʵ����ת����һ���ؿ����߼�
                ActivateLevel("level2");
                break;
            case "level2":
                ActivateLevel("level3");
                break;
            case "level3":
                Console.WriteLine("�ؿ�������");  // �����������
                break;
            default:
                Console.WriteLine("û�йؿ�");
                break;
        }
    }

    public void ActivateLevel(string levelName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name == levelName)
            {
                child.SetActive(true);
            }
            else
            {
                child.SetActive(false);
            }
        }
    }
    // ������Ϸ����
    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("��Ϸ�Ѽ���");
            return data;
        }
        else
        {
            SaveGame(0, "level1", new bool[3] { false, false, false });
            Debug.LogError("�浵�ļ�������,�Ѵ���");
            return null;
        }
    }

    // ɾ���浵
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("�浵��ɾ��");
        }
    }
}