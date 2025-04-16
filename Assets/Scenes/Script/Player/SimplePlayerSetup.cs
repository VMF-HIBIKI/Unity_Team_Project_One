using UnityEngine;

// 此脚本为设置简单长方体角色的指南
// 不需要挂载到任何对象上
public class SimplePlayerSetup : MonoBehaviour
{
    /*
    简单3D长方体角色设置指南：
    
    1. 基本设置：
       a. 创建一个3D立方体 (GameObject > 3D Object > Cube)
       b. 设置其Transform的Scale为 (0.75, 2, 0.75)
       c. 将PlayerDemo.cs脚本挂载到此对象上
    
    2. 物理组件设置：
       a. 添加Rigidbody组件 (Add Component > Physics > Rigidbody)
          - Constraints: 勾选"Freeze Rotation X, Y, Z"以防止角色旋转
          - Mass: 设置为1或适当的质量
       b. 确保有BoxCollider组件（立方体默认自带）
          - 调整Size以匹配长方体的尺寸（如需要）
    
    3. 地面检测设置：
       a. 地面检测点:
          - 在层级视图中右键创建空对象 (Create Empty)，命名为"GroundCheck"
          - 将GroundCheck设为长方体的子对象
          - 将GroundCheck的位置设在长方体底部稍下方（例如 Y = -1）
          - 在长方体的PlayerDemo组件中，将GroundCheck对象拖入"Ground Check"字段
          - 或者让脚本自动生成此点（脚本的Start方法中已包含此功能）
    
    4. 图层设置：
       a. 创建图层（Edit > Project Settings > Tags and Layers）:
          - "Ground": 用于地面和平台
       b. 将所有地面对象设置为"Ground"图层
       c. 在角色的PlayerDemo组件中，设置"Ground Layer"为"Ground"图层
    
    5. 摄像机设置：
       a. 确保场景中有一个带有"MainCamera"标签的摄像机
       b. 摄像机位置建议：
          - 位置：(0, 10, -10)
          - 旋转：(45, 0, 0)
       c. 或者使用SimpleSceneSetup.cs脚本一键生成，它会自动创建合适的摄像机
    
    6. 场景设置：
       a. 创建一些平台作为地面（可以用3D立方体缩放和拉伸）
       b. 创建一些障碍物和斜坡
       c. 将地面/平台设置为"Ground"图层
       d. 可以使用SimpleSceneSetup.cs脚本一键生成测试场景
    
    7. 移动加速度参数调整：
       - maxMoveSpeed: 最大移动速度，建议值为5-10
       - moveAcceleration: 加速度大小，建议值为30-100，值越大加速越快
       - moveDamping: 移动阻尼，建议值为0.5-1.5，值越大减速越快
       - accelerationCurve: 加速度曲线，默认为对数曲线（快速启动，缓慢接近最大速度）
    
    8. 跳跃和重力参数调整：
       - jumpForce: 跳跃力度，建议值为7-10
       - doubleJumpForce: 二段跳力度，建议值为5-8
       - jumpAcceleration: 跳跃加速度倍数，建议值为1.5-3
       - fallMultiplier: 下落加速度倍数，建议值为2-3（使下落感更强）
       - lowJumpMultiplier: 短跳加速度倍数，建议值为1.5-2.5
       - useCustomGravity: 是否使用自定义重力，建议开启以获得更好的跳跃体验
    
    9. 调试选项：
       - showDebugGizmos: 在Scene视图中显示调试图形
       - showDebugText: 在Game视图中显示调试文本
       - debugColor: 调试信息的颜色
    
    10. 控制方式：
       - 前后左右移动：WSAD键或方向键
       - 跳跃：空格键
       - 二段跳：在空中时再次按空格键
       - 角色会自动朝向移动方向
    */
    
    void Awake()
    {
        // 这个脚本仅作为参考，不需要实际功能
        Debug.Log("这是一个帮助类，请阅读脚本中的注释获取设置指南。");
        Destroy(this);
    }
} 