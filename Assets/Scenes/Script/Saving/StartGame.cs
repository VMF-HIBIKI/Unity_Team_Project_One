using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject gameStart;
    // Start is called before the first frame update
    public void Start()
    {

        if (gameStart != null)
        {
            // 实例化预制体，将其放置在场景中，这里放在当前脚本所在物体的位置
            GameObject instantiatedPrefab = Instantiate(gameStart, transform.position, transform.rotation);
            Debug.Log("场景初始化成功");
        }
        else
        {
            Debug.LogError("未能找到预制体");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
