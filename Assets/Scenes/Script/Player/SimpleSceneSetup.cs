using UnityEngine;

// 此脚本可用于快速生成一个简单的测试场景
public class SimpleSceneSetup : MonoBehaviour
{
    public GameObject playerPrefab; // 可以是一个预制体或者直接在场景中创建
    public Material groundMaterial; // 可选，地面材质
    
    [ContextMenu("生成简单测试场景")]
    public void GenerateSimpleScene()
    {
        // 创建角色
        GameObject player = playerPrefab;
        if (player == null)
        {
            player = GameObject.CreatePrimitive(PrimitiveType.Cube);
            player.name = "Player";
            player.transform.localScale = new Vector3(0.75f, 2f, 0.75f);
            player.transform.position = new Vector3(0, 2, 0);
            
            // 添加必要组件
            Rigidbody rb = player.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.mass = 1f;
            
            // 添加控制脚本
            player.AddComponent<PlayerDemo>();
        }
        
        // 创建主地面
        CreateGround("MainGround", new Vector3(0, 0, 0), new Vector3(20, 1, 20));
        
        // 创建一些平台
        CreateGround("Platform1", new Vector3(5, 3, 5), new Vector3(5, 0.5f, 5));
        CreateGround("Platform2", new Vector3(-5, 5, -5), new Vector3(5, 0.5f, 5));
        CreateGround("Platform3", new Vector3(0, 8, 0), new Vector3(6, 0.5f, 6));
        
        // 创建一些障碍物
        CreateObstacle("Obstacle1", new Vector3(3, 1.5f, 3), new Vector3(1, 1, 1));
        CreateObstacle("Obstacle2", new Vector3(-3, 1.5f, -3), new Vector3(1, 1, 1));
        CreateObstacle("Obstacle3", new Vector3(0, 1.5f, -6), new Vector3(1, 1, 1));
        
        // 创建边界墙
        CreateGround("WallNorth", new Vector3(0, 5, 10), new Vector3(20, 10, 1));
        CreateGround("WallSouth", new Vector3(0, 5, -10), new Vector3(20, 10, 1));
        CreateGround("WallEast", new Vector3(10, 5, 0), new Vector3(1, 10, 20));
        CreateGround("WallWest", new Vector3(-10, 5, 0), new Vector3(1, 10, 20));
        
        // 创建斜坡
        CreateSlope("Slope1", new Vector3(8, 1.5f, 8), new Vector3(4, 1, 4), 30);
        
        // 设置层级
        SetupLayers();
        
        // 创建主摄像机
        SetupCamera();
        
        Debug.Log("测试场景已生成！请确保已添加了地面图层（Ground Layer）。");
    }
    
    private GameObject CreateGround(string name, Vector3 position, Vector3 scale)
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = name;
        ground.transform.position = position;
        ground.transform.localScale = scale;
        
        // 应用材质（如果有）
        if (groundMaterial != null)
        {
            ground.GetComponent<Renderer>().material = groundMaterial;
        }
        
        return ground;
    }
    
    private GameObject CreateObstacle(string name, Vector3 position, Vector3 scale)
    {
        GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacle.name = name;
        obstacle.transform.position = position;
        obstacle.transform.localScale = scale;
        
        // 使用随机颜色
        Renderer renderer = obstacle.GetComponent<Renderer>();
        renderer.material.color = new Color(
            Random.Range(0.3f, 0.9f),
            Random.Range(0.3f, 0.9f),
            Random.Range(0.3f, 0.9f)
        );
        
        return obstacle;
    }
    
    private GameObject CreateSlope(string name, Vector3 position, Vector3 scale, float angle)
    {
        GameObject slope = GameObject.CreatePrimitive(PrimitiveType.Cube);
        slope.name = name;
        slope.transform.position = position;
        slope.transform.localScale = scale;
        slope.transform.Rotate(0, 0, angle);
        
        // 应用材质（如果有）
        if (groundMaterial != null)
        {
            slope.GetComponent<Renderer>().material = groundMaterial;
        }
        
        return slope;
    }
    
    private void SetupLayers()
    {
        // 检查Ground图层是否存在
        int groundLayer = LayerMask.NameToLayer("Ground");
        
        if (groundLayer == -1)
        {
            Debug.LogWarning("请创建名为'Ground'的图层。(Edit > Project Settings > Tags and Layers)");
        }
        
        // 找到所有名称包含"Ground"或"Platform"或"Wall"或"Slope"的物体，设置为Ground层
        GameObject[] grounds = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in grounds)
        {
            if (obj.name.Contains("Ground") || obj.name.Contains("Platform") || 
                obj.name.Contains("Wall") || obj.name.Contains("Slope") ||
                obj.name.Contains("Obstacle"))
            {
                if (groundLayer != -1)
                    obj.layer = groundLayer;
            }
        }
        
        // 找到Player对象并设置层级
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            PlayerDemo playerScript = player.GetComponent<PlayerDemo>();
            if (playerScript != null && groundLayer != -1)
            {
                playerScript.groundLayer = 1 << groundLayer;
            }
        }
    }
    
    private void SetupCamera()
    {
        // 查找现有的主摄像机
        Camera mainCamera = Camera.main;
        
        // 如果没有主摄像机，创建一个
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            mainCamera = cameraObj.AddComponent<Camera>();
            cameraObj.AddComponent<AudioListener>();
        }
        
        // 设置摄像机位置和角度
        mainCamera.transform.position = new Vector3(0, 10, -10);
        mainCamera.transform.rotation = Quaternion.Euler(45, 0, 0);
    }
    
    // 在编辑器中添加右键菜单项
    [UnityEditor.MenuItem("GameObject/创建简单3D测试场景", false, 10)]
    static void CreateSimpleTestScene(UnityEditor.MenuCommand menuCommand)
    {
        GameObject sceneMaker = new GameObject("SceneSetup");
        SimpleSceneSetup setup = sceneMaker.AddComponent<SimpleSceneSetup>();
        setup.GenerateSimpleScene();
        
        // 注册创建的对象到Undo系统
        UnityEditor.Undo.RegisterCreatedObjectUndo(sceneMaker, "创建测试场景");
    }
} 