using UnityEngine;
using UnityEngine.UI;

// 本脚本用于引导用户如何设置PlayerDemo脚本及虚拟摇杆
// 此脚本不需要挂载，仅作为参考
public class PlayerControllerSetup : MonoBehaviour
{
    /*
    角色控制器设置指南：
    
    1. 角色设置：
       - 给角色添加Rigidbody2D组件，设置Type为Dynamic，Freeze Rotation Z轴打钩
       - 给角色添加适当的Collider2D组件（如BoxCollider2D、CapsuleCollider2D等）
       - 将PlayerDemo.cs脚本挂载到角色上
    
    2. 地面检测设置：
       - 创建一个空对象作为GroundCheck，放置在角色脚部
       - 或者让脚本自动创建（脚本的Start方法中已包含）
       - 设置地面图层（Layer）用于检测，并在角色的PlayerDemo组件上的groundLayer中选择对应图层
    
    3. 虚拟摇杆设置（仅限移动端或需要触屏控制时）：
       a. 创建UI Canvas（UI > Canvas）
       b. 在Canvas下创建一个Panel作为摇杆背景
          - 设置Panel的Image组件，使用圆形图片
          - 调整尺寸和位置，通常放在屏幕左下角
       c. 在Panel下创建一个Image作为摇杆手柄
          - 使用圆形图片，适当调整大小
       d. 给Panel添加Joystick脚本
          - 设置background为Panel的RectTransform
          - 设置handle为手柄Image的RectTransform
       e. 将Joystick引用拖拽到角色PlayerDemo组件的joystick字段中
    
    4. 跳跃按钮设置（移动端）：
       a. 在Canvas下创建一个Button
          - 设置Button的位置，通常放在屏幕右下角
          - 可选择添加跳跃图标
       b. 将这个Button引用拖拽到角色PlayerDemo组件的jumpButton字段中
    
    5. 角色的物理材质设置（可选）：
       - 创建一个Physics2D Material，减少摩擦
       - 将这个材质应用到角色的Collider2D组件上
    
    6. 图层设置：
       - 在项目设置中确保已定义地面图层（例如"Ground"）
       - 将所有地面物体设置为这个图层
       - 在角色PlayerDemo组件的groundLayer中选择这个图层
    
    7. 参数调整：
       - moveSpeed：角色移动速度，建议5-10
       - jumpForce：第一段跳跃力度，建议8-15
       - doubleJumpForce：二段跳力度，通常略小于jumpForce
       - groundCheckRadius：地面检测范围，根据角色尺寸调整，通常0.1-0.3
    */
    
    void Awake()
    {
        // 这个脚本仅作为参考，不需要实际功能
        Debug.Log("这是一个帮助类，请阅读脚本中的注释获取设置指南。");
        Destroy(this);
    }
} 