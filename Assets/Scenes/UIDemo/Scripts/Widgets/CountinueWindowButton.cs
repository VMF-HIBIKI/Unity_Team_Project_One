using System;
using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CountinueWindowButton : MonoBehaviour
{
    public Button button = null;
    private StartGame sg;



    public void UI_Click()
    {
        SceneManager.LoadScene(1);
    }
}
