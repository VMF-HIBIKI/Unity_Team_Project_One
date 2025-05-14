using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public int DeadCount;
    public string currentScene;
    public bool[] levelUnlocked;
    // ������Ҫ���������...

    public GameData( int count, string scene, bool[] levels)
    {
        DeadCount = count;
        currentScene = scene;
        levelUnlocked = levels;
    }
}