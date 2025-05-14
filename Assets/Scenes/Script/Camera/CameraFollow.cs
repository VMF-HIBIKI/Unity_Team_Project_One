using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 玩家的Transform
    public float smoothSpeed = 0.125f; // 跟随平滑度
    public Vector3 offset; // 相机与玩家的偏移量

    void FixedUpdate()
    {
        // 计算相机的目标位置
        Vector3 desiredPosition = target.position + offset;
        // 使用Lerp平滑过渡到目标位置
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 让相机始终看向玩家
        transform.LookAt(target);
    }
}