using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Tooltip("是否使用触发器检测")]
    public bool useTrigger = true;
    
    [Tooltip("陷阱颜色")]
    public Color trapColor = Color.red;
    
    [Tooltip("是否在碰撞后销毁陷阱")]
    public bool destroyAfterTrigger = false;
    
    private void Start()
    {
        // 设置陷阱颜色
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = trapColor;
        }
        
        // 确保有碰撞器
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.isTrigger = useTrigger;
        }
        else
        {
            collider.isTrigger = useTrigger;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (useTrigger)
        {
            CheckPlayerCollision(other.gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!useTrigger)
        {
            CheckPlayerCollision(collision.gameObject);
        }
    }
    
    private void CheckPlayerCollision(GameObject obj)
    {
        // 检查碰撞的对象是否是玩家（通过标签或查找PlayerDemo组件）
        if (obj.CompareTag("Player") || obj.GetComponent<PlayerDemo>() != null)
        {
            Debug.Log("玩家触发了陷阱！");
            
            // 获取玩家的PlayerDemo组件
            PlayerDemo playerDemo = obj.GetComponent<PlayerDemo>();
            
            if (playerDemo != null)
            {
                // 设置isDie属性为true
                playerDemo.isDie = true;
                // 也可以使用属性访问器
                // playerDemo.IsDie = true;
                
                Debug.Log("玩家死亡状态已设置为: " + playerDemo.isDie);
            }
            else
            {
                Debug.LogWarning("找不到PlayerDemo组件！");
            }
            
            // 如果设置了碰撞后销毁，则销毁陷阱
            if (destroyAfterTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}