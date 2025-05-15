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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        deadCount = SaveSystem.Instance.LoadGame().DeadCount;
    }

    public void AddDeadCount()
    {
        deadCount++;
    }
}
