using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemo : MonoBehaviour
{
    [Header("移动设置")]
    public float maxMoveSpeed = 5f;       // 最大移动速度
    public float moveAcceleration = 50f;  // 移动加速度
    public float moveDamping = 0.8f;      // 移动阻尼（数值越小，速度越快降低）
    public AnimationCurve accelerationCurve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(0.2f, 0.8f),
        new Keyframe(1, 1)
    );                                    // 加速度曲线，默认为对数曲线

    [Header("跳跃设置")]
    public float jumpForce = 7f;          // 跳跃力
    public float doubleJumpForce = 5f;    // 二段跳力
    public float jumpAcceleration = 4f;   // 跳跃加速度倍数
    public float fallMultiplier = 2.5f;   // 下落加速度倍数
    public float lowJumpMultiplier = 2f;  // 短跳加速度倍数

    [Header("重力设置")]
    public float gravityScale = 2f;       // 重力缩放
    public bool useCustomGravity = true;  // 是否使用自定义重力
    
    [Header("旋转设置")]
    public float rotationSpeed = 120f;    // 旋转速度
    
    [Header("地面检测")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;

    [Header("调试显示")]
    public bool showDebugGizmos = true;   // 是否显示调试图形
    public bool showDebugText = true;     // 是否显示调试文本
    public Color debugColor = Color.green; // 调试颜色

    private Rigidbody rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Vector3 currentVelocity = Vector3.zero;
    private float currentSpeed = 0f;
    private float velocityXZMagnitude = 0f;
    private float accelerationRatio = 0f;

    void Start()
    {
        // 获取或添加必要组件
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation; // 冻结旋转
            rb.mass = 1f;
        }
        
        // 如果没有设置地面检测点，则自动在脚部创建一个
        if (groundCheck == null)
        {
            GameObject check = new GameObject("GroundCheck");
            check.transform.parent = transform;
            check.transform.localPosition = new Vector3(0, -1f, 0); // 根据长方体尺寸调整位置
            groundCheck = check.transform;
        }
    }

    void Update()
    {
        // 地面检测
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        
        // 重置二段跳状态
        if (isGrounded)
        {
            canDoubleJump = true;
        }
        
        // 获取WSAD或箭头键输入
        horizontalInput = Input.GetAxisRaw("Horizontal"); // AD或左右箭头
        verticalInput = Input.GetAxisRaw("Vertical");     // WS或上下箭头
        
        // 根据相机方向计算移动方向
        moveDirection = CalculateMoveDirection();
        
        // 跳跃控制（空格键）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        // 应用自定义重力
        if (useCustomGravity)
        {
            ApplyCustomGravity();
        }
    }
    
    void FixedUpdate()
    {
        // 在FixedUpdate中处理物理移动
        Move();
    }
    
    // 计算基于摄像机朝向的移动方向
    Vector3 CalculateMoveDirection()
    {
        // 获取摄像机的前方和右方向量
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        
        // 确保方向向量在水平面上
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        // 根据输入和摄像机方向计算移动方向
        return cameraForward * verticalInput + cameraRight * horizontalInput;
    }

    // 自定义重力
    void ApplyCustomGravity()
    {
        if (rb.velocity.y < 0)
        {
            // 下落时应用更大的重力
            rb.AddForce(Vector3.up * Physics.gravity.y * gravityScale * (fallMultiplier - 1) * Time.deltaTime, ForceMode.Acceleration);
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            // 松开跳跃键时应用更大的重力实现短跳
            rb.AddForce(Vector3.up * Physics.gravity.y * gravityScale * (lowJumpMultiplier - 1) * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    // 移动逻辑
    void Move()
    {
        // 只有在有输入时才应用对数加速
        if (moveDirection.magnitude > 0.1f)
        {
            // 计算当前水平移动速度
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            velocityXZMagnitude = horizontalVelocity.magnitude;
            
            // 计算加速比例（当前速度与最大速度的比值）
            accelerationRatio = Mathf.Clamp01(velocityXZMagnitude / maxMoveSpeed);
            
            // 根据加速度曲线评估当前应用的加速度比例
            float accelerationFactor = accelerationCurve.Evaluate(accelerationRatio);
            
            // 计算目标速度，应用对数加速
            Vector3 targetVelocity = moveDirection.normalized * maxMoveSpeed;
            
            // 只影响水平移动，保留Y轴速度
            Vector3 desiredVelocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
            
            // 应用基于当前加速度比例的加速力
            Vector3 accelerationForce = (targetVelocity - horizontalVelocity) * 
                                       moveAcceleration * 
                                       (1 - accelerationFactor);
            
            // 应用加速力
            rb.AddForce(new Vector3(accelerationForce.x, 0, accelerationForce.z), ForceMode.Acceleration);
            
            // 应用阻尼
            if (velocityXZMagnitude > 0.1f)
            {
                // 只对水平速度应用阻尼
                Vector3 dampingForce = -horizontalVelocity * moveDamping;
                rb.AddForce(dampingForce, ForceMode.Acceleration);
            }
            
            // 限制最大速度
            if (horizontalVelocity.magnitude > maxMoveSpeed)
            {
                Vector3 limitedVelocity = horizontalVelocity.normalized * maxMoveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
            
            // 更新当前速度（用于调试）
            currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
            
            // 朝向移动方向
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // 没有输入时，应用更强的阻尼使角色停下
            Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (horizontalVelocity.magnitude > 0.1f)
            {
                Vector3 dampingForce = -horizontalVelocity * (moveDamping * 2);
                rb.AddForce(dampingForce, ForceMode.Acceleration);
            }
            else if (horizontalVelocity.magnitude <= 0.1f && isGrounded)
            {
                // 当几乎静止时，直接设置水平速度为零（防止滑动）
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }
            
            // 更新当前速度（用于调试）
            currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        }
    }
    
    void Jump()
    {
        if (isGrounded)
        {
            // 第一段跳跃
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // 重置垂直速度
            rb.AddForce(Vector3.up * jumpForce * jumpAcceleration, ForceMode.Impulse);
        }
        else if (canDoubleJump)
        {
            // 二段跳
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // 重置垂直速度
            rb.AddForce(Vector3.up * doubleJumpForce * jumpAcceleration, ForceMode.Impulse);
            canDoubleJump = false;
        }
    }
    
    // 在Editor中可视化检测范围和调试信息
    void OnDrawGizmosSelected()
    {
        if (!showDebugGizmos) return;
        
        if (groundCheck != null)
        {
            Gizmos.color = debugColor;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            
            // 绘制移动方向
            if (Application.isPlaying && moveDirection.magnitude > 0.1f)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(transform.position, moveDirection.normalized * 2);
                
                Gizmos.color = Color.red;
                Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                Gizmos.DrawRay(transform.position, horizontalVelocity.normalized * 2);
            }
        }
    }
    
    // 在屏幕上显示调试信息
    void OnGUI()
    {
        if (!showDebugText || !Application.isPlaying) return;
        
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = debugColor;
        
        GUI.Label(new Rect(10, 10, 300, 20), "地面状态: " + (isGrounded ? "接触地面" : "空中"), style);
        GUI.Label(new Rect(10, 30, 300, 20), "当前速度: " + currentSpeed.ToString("F2"), style);
        GUI.Label(new Rect(10, 50, 300, 20), "加速比例: " + accelerationRatio.ToString("F2"), style);
        GUI.Label(new Rect(10, 70, 300, 20), "加速系数: " + accelerationCurve.Evaluate(accelerationRatio).ToString("F2"), style);
        GUI.Label(new Rect(10, 90, 300, 20), "二段跳可用: " + canDoubleJump, style);
    }
}
