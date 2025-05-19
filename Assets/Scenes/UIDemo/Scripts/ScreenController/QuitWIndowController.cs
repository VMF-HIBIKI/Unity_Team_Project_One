using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

public class QuitWIndowController : WindowController
{
    void Start()
    {
        // 关闭窗口
        QuitGame();
    }
    // Start is called before the first frame update
    public void QuitGame()
    {
#if UNITY_EDITOR
        // 在Unity编辑器中退出游戏
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在发布版本中退出游戏
            Application.Quit();
#endif
    }
}
