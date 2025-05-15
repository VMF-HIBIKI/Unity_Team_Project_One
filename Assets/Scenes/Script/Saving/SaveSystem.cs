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
            Debug.Log("存档路径：" + Application.persistentDataPath);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // 保存游戏数据
    public void SaveGame( int count,  string scene, bool[] levels)
    {
        GameData data = new GameData(count, scene, levels);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("游戏已保存：" + savePath);
    }
    public void JumpToNextLevel(string currentScene)
    {
        switch (currentScene)
        {
            case "level1":
                // 这里实现跳转到下一个关卡的逻辑
                ActivateLevel("level2");
                break;
            case "level2":
                ActivateLevel("level3");
                break;
            case "level3":
                Console.WriteLine("关卡到顶了");  // 输出：星期三
                break;
            default:
                Console.WriteLine("没有关卡");
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
    // 加载游戏数据
    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("游戏已加载");
            return data;
        }
        else
        {
            SaveGame(0, "level1", new bool[3] { false, false, false });
            Debug.LogError("存档文件不存在,已创建");
            return null;
        }
    }

    // 删除存档
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("存档已删除");
        }
    }
}