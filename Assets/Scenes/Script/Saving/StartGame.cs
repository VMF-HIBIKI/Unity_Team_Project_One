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
            // ʵ����Ԥ���壬��������ڳ����У�������ڵ�ǰ�ű����������λ��
            GameObject instantiatedPrefab = Instantiate(gameStart, transform.position, transform.rotation);
            Debug.Log("������ʼ���ɹ�");
        }
        else
        {
            Debug.LogError("δ���ҵ�Ԥ����");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
