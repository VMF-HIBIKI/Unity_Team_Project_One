using System;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    //public GameObject interactionPrompt; // 交互提示UI
    //public AudioClip saveSound;          // 存档音效
    public bool isAutoSave = true;      // 是否自动存档
    public string currentScene;
    public bool[] levels;               // 关卡状态数组

    private bool canInteract = false;
    //private AudioSource audioSource;

    private void Start()
    {
        //interactionPrompt.SetActive(false);
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            //interactionPrompt.SetActive(true);

            // 自动存档
            if (isAutoSave) SaveGame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            //interactionPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        // 手动存档（按E键）
        if (canInteract && !isAutoSave && Input.GetKeyDown(KeyCode.E))
        {
            SaveGame();
        }
    }

    private void SaveGame()
    {
        // 获取玩家数据
        PlayerDemo player = FindObjectOfType<PlayerDemo>();
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.SetCurrentScene(currentScene);
        levelManager.SetLevelStatus(levels);
        if (SaveSystem.Instance == null)
        {
            Debug.LogError("SaveSystem实例未初始化！请确保场景中有SaveSystem GameObject。");
            return;
        }
        if (player != null && levelManager != null)
        {
            // 保存游戏
            SaveSystem.Instance.SaveGame(
                levelManager.GetDeadCount,
                levelManager.GetCurrentScene,
                levelManager.GetLevelStatus
            );

            // 播放存档音效
            //if (saveSound != null)
            //{
            //    audioSource.PlayOneShot(saveSound);
            //}

            // 显示存档成功UI
            ShowSaveSuccessUI();

            SaveSystem.Instance.JumpToNextLevel(currentScene);
        }
    }



    private void ShowSaveSuccessUI()
    {
        // 这里实现存档成功的UI显示逻辑
        Debug.Log("存档成功！");
    }
}