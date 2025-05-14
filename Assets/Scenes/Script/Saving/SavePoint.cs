using System;
using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    //public GameObject interactionPrompt; // ������ʾUI
    //public AudioClip saveSound;          // �浵��Ч
    public bool isAutoSave = true;      // �Ƿ��Զ��浵
    public string currentScene;
    public bool[] levels;               // �ؿ�״̬����

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

            // �Զ��浵
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
        // �ֶ��浵����E����
        if (canInteract && !isAutoSave && Input.GetKeyDown(KeyCode.E))
        {
            SaveGame();
        }
    }

    private void SaveGame()
    {
        // ��ȡ�������
        PlayerDemo player = FindObjectOfType<PlayerDemo>();
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        levelManager.SetCurrentScene(currentScene);
        levelManager.SetLevelStatus(levels);
        if (SaveSystem.Instance == null)
        {
            Debug.LogError("SaveSystemʵ��δ��ʼ������ȷ����������SaveSystem GameObject��");
            return;
        }
        if (player != null && levelManager != null)
        {
            // ������Ϸ
            SaveSystem.Instance.SaveGame(
                levelManager.GetDeadCount,
                levelManager.GetCurrentScene,
                levelManager.GetLevelStatus
            );

            // ���Ŵ浵��Ч
            //if (saveSound != null)
            //{
            //    audioSource.PlayOneShot(saveSound);
            //}

            // ��ʾ�浵�ɹ�UI
            ShowSaveSuccessUI();

            SaveSystem.Instance.JumpToNextLevel(currentScene);
        }
    }



    private void ShowSaveSuccessUI()
    {
        // ����ʵ�ִ浵�ɹ���UI��ʾ�߼�
        Debug.Log("�浵�ɹ���");
    }
}