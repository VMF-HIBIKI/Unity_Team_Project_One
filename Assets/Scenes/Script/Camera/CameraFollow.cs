using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ��ҵ�Transform
    public float smoothSpeed = 0.125f; // ����ƽ����
    public Vector3 offset; // �������ҵ�ƫ����

    void FixedUpdate()
    {
        // ���������Ŀ��λ��
        Vector3 desiredPosition = target.position + offset;
        // ʹ��Lerpƽ�����ɵ�Ŀ��λ��
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // �����ʼ�տ������
        transform.LookAt(target);
    }
}