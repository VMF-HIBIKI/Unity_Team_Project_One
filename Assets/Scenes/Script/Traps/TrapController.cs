using UnityEngine;

public class TrapController : MonoBehaviour
{
    [Header("陷阱设置")]
    public GameObject trap; // 陷阱物体
    public float speedY = 5f; // Y 轴速度，可在 Inspector 中调整
    public float speedZ = 3f; // Z 轴速度，可在 Inspector 中调整
    public Collider triggerCollider; // 触发器碰撞体
    public LayerMask ignoreLayer;

    private int Triggercount = 0;

    private void Start()
    {
        int currentLayer = gameObject.layer;

        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & ignoreLayer.value) != 0)
            {
                // 忽略当前物体所在Layer与指定Layer的碰撞
                Physics.IgnoreLayerCollision(currentLayer, i, true);
            }
        }

        if (trap == null)
        {
            Debug.LogError("未找到陷阱物体，请在 Inspector 中指定或确保子物体中有陷阱物体。");
            return;
        }

        // 查找触发器碰撞体
        if (triggerCollider == null)
        {
            Debug.LogError("未找到触发器碰撞体，请确保子物体中有一个带有 BoxCollider 且 isTrigger 为 true 的物体。");
            return;
        }
        if(triggerCollider.isTrigger == false)
        {
            Debug.LogError("触发器碰撞体的 isTrigger 属性未设置为 true，请检查。");
            return;
        }else Debug.Log("触发器碰撞体的 isTrigger 属性已经设置为 true");
        triggerCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("触发器被触发！");
        Debug.Log("进入物体名称: " + other.gameObject.name);
        Debug.Log("进入物体标签: " + other.tag);
        Debug.Log("进入物体层: " + LayerMask.LayerToName(other.gameObject.layer));
        Debug.Log("触发器位置: " + transform.position);
        Debug.Log("进入物体位置: " + other.transform.position);

        if (other.CompareTag("Player"))
        {
            Debug.Log("检测到玩家进入触发器");
            // 激活陷阱（如果未激活）
            if (!trap.activeSelf&&Triggercount==0)
            {
                trap.SetActive(true);
                Triggercount++;
            }

            // 给陷阱添加刚体组件（如果没有）
            Rigidbody rb = trap.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = trap.AddComponent<Rigidbody>();
                // 禁用重力，防止影响速度设置
                rb.useGravity = false;
            }

            // 设置陷阱的速度
            rb.velocity = new Vector3(0, speedY, speedZ);
            Debug.Log("陷阱已激活，速度设置为: " + rb.velocity);
        }
    }
}