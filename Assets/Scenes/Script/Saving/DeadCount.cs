using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCount : MonoBehaviour
{
    public static DeadCount instance;
    public int deadCount = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddDeadCount()
    {
        deadCount++;
    }
}
