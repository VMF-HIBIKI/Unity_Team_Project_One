using System;
using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;
using Utils;
[Serializable]
public class CountinueWindowEntry
{
    [SerializeField] private Text buttonText1 ;
    [SerializeField] private Text buttonText2 ;



    public Text ButtonText1
    {
        get { return buttonText1; }
        set { buttonText1 = value; }
    }
    public Text ButtonText2
    {
        get { return buttonText2; }
        set { buttonText2 = value; }
    }

}
public class CountinueWindowSignal : ASignal { }
public class CountinueWindowController : WindowController
{
    private GameData gameData = null;
    private static SaveSystem Instance;
    public CountinueWindowEntry CountinueWindowEntry = new CountinueWindowEntry();
    int count = 1;

    protected override void OnPropertiesSet()
    {
        SetSaveCard();
    }

    private void SetSaveCard()
    {
        if (Instance == null)
        { Instance = new SaveSystem(); }
        gameData = Instance.LoadGame();
        
        foreach (bool level in gameData.levelUnlocked)
        {
            if (level == true)
            {
                count++;
                count%=4;
            }
        }
        CountinueWindowEntry.ButtonText1.text = "死亡次数：" + gameData.DeadCount.ToString();
        CountinueWindowEntry.ButtonText2.text = "当前关卡：" + count.ToString();
    }


}
