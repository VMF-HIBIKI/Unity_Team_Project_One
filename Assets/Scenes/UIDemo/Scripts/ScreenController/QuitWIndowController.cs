using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

public class QuitWIndowController : WindowController
{
    void Start()
    {
        // �رմ���
        QuitGame();
    }
    // Start is called before the first frame update
    public void QuitGame()
    {
#if UNITY_EDITOR
        // ��Unity�༭�����˳���Ϸ
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // �ڷ����汾���˳���Ϸ
            Application.Quit();
#endif
    }
}
