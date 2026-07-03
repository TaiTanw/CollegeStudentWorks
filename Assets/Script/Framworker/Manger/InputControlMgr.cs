using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputData
{
    public float moveInput;    //水平输入
    public bool jumpPressed;   // 这一帧是否按下
}
public enum E_InputType
{
    tuo,
    key,
    nil
}

public class InputControlMgr : BaseAutoMonoMgr<InputControlMgr>
{
    /// <summary>
    /// 运行时输入信息
    /// </summary>
    PlayerInputData inputData;
    public PlayerInputData PlayerInputData => inputData;

    E_InputType type= E_InputType.nil;
    /// <summary>
    /// 控制转换,返回值为上一次状态
    /// </summary>
    public E_InputType BuffChange(E_InputType d)
    {
        E_InputType last=type;
        type = d;
        return last;
    }
    /// <summary>
    /// 配置时输入绑定
    /// </summary>
    PlayerInput input;
    /// <summary>
    /// 所控制玩家的实例
    /// </summary>
    Player player;
    /// <summary>
    /// 玩家美术资源所指向路径（名称）
    /// </summary>
    string playerModle = "player";
    /// <summary>
    /// 主摄像机
    /// </summary>
    MainCamera mainCamera;
    void HideLayerByName(Camera cam, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1)
        {
            Debug.LogWarning($"层 '{layerName}' 不存在，请检查拼写或是否已在设置中添加。");
            return;
        }
        cam.cullingMask &= ~(1 << layer);
    }
    /// <summary>
    /// 毁灭吧，这个痛苦的世界！！！！！
    /// </summary>
    public void JueSHu()
    {
        HideLayerByName(mainCamera.GetComponent<Camera>(), "kuang");
        HideLayerByName(mainCamera.GetComponent<Camera>(), "obj");
    }
    Mouse mouse = Mouse.current;
    /// <summary>
    /// 关联键位绑定数据
    /// </summary>
    public void BindInputAsset()
    {
        input.actions = DataAndInitMgr.Instance.asset;
    }
    /// <summary>
    /// Awake保证数据结构存在
    /// </summary>
    private void Awake()
    {
        // 获取或添加 PlayerInput 组件
        //input = GetComponent<PlayerInput>();

        if (input == null)
        {
            input = gameObject.AddComponent<PlayerInput>();
        }

        // 设置 Behavior 为 Invoke CSharp Events
        input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

        //创建运行时输入信息
        inputData = new PlayerInputData();
    }
    /// <summary>
    /// 显式初始化，精准控制
    /// </summary>
    public void Init()
    {
        BindInputAsset();//关联action唯一入口,关联数据管理器内已经初始化完成的数据

        //注册按键响应
        input.onActionTriggered += (callBack) =>
        {
            //确保是触发下响应
            if (callBack.phase == InputActionPhase.Performed)
            {
                //用于测试当前触发的action名称
                //Debug.Log("Action Triggered: " + callBack.action.name);
                switch (callBack.action.name)
                {
                    case "Move":
                        //print("移动开始");
                        //print(callBack.ReadValue<float>());
                        inputData.moveInput = callBack.ReadValue<float>();
                        break;

                    case "Jump":

                        //print("跳跃");
                        inputData.jumpPressed = true;//new Vector2(rb.velocity.x, jumpForce);
                        break;
                }
            }
            //按键抬起逻辑，数据复原
            else if (callBack.phase == InputActionPhase.Canceled)
            {
                switch (callBack.action.name)
                {
                    case "Move":
                        //print("移动取消");
                        //print(callBack.ReadValue<float>());
                        inputData.moveInput = callBack.ReadValue<float>();
                        break;

                    case "Jump":
                        //print("跳跃松开");
                        break;
                }
            }
        };
        //绑定后关闭输入响应
        InputOpenOrClose(false);
    }
    /// <summary>
    /// 输入控制开关
    /// </summary>
    /// <param name="isOpen"></param>
    public void InputOpenOrClose(bool isOpen)
    {
        if (isOpen)
        {
            input.actions.Enable();
        }
        else
        {
            input.actions.Disable();
        }
    }
    /// <summary>
    /// 关联玩家
    /// </summary>
    /// <param name="player"></param>
    public void BindPlayer(Player player)
    {
        this.player = player;
        this.player.ChangeInputAsset(inputData);
    }
    /// <summary>
    /// 设置玩家模型指向
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerModle(string name)
    {
        playerModle = name;
    }
    /// <summary>
    /// 设置主摄像机
    /// </summary>
    /// <param name="camera"></param>
    public void SetMainCamera(MainCamera camera)
    {
        mainCamera = camera;
        mainCamera.SetPoint(player.transform.position);
    }
    //private Transform nowSelObj;
    RaycastHit2D hit;
    obj nowObj;
    /// <summary>
    /// 是否确认
    /// </summary>
    public bool isOnOk;

    public bool keyDown;
    void Update()
    {
        //if (mainCamera != null)
        //{
            

        //}
        switch (type)
        {
            case E_InputType.key:
                if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    // 你的空格按下逻辑
                    keyDown = true;
                }
                break;
            case E_InputType.tuo:
                mouse = Mouse.current;
                if (mouse == null) return;
                // 2. 检测“持续按住”（Hold）
                if (mouse.leftButton.isPressed)
                {
                    isOnOk = false;
                    //Debug.Log("鼠标左键正在被按住...");
                    Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());

                    // 从鼠标位置发射一条长度为 0 的射线（相当于“点检测”）
                    hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 0f, LayerMask.GetMask("obj"));

                    if (hit.collider != null)
                    {
                        //nowSelObj = hit.transform;
                        // 注意：2D 物体的 position 是 Vector3，但 hit.point 是 Vector2，需要隐式转换
                        if (nowObj == null)
                        {
                            nowObj = hit.transform.GetComponent<obj>();

                            //nowSelObj.gameObject.GetComponent<OutlineController>().ShowOutline();
                        }
                        //if (!nowObj.isOk)
                        nowObj.transform.position = new Vector3(hit.point.x, hit.point.y, nowObj.transform.position.z);
                        // 在这里执行拖拽、持续射击等持续性操作
                    }



                }
                // 3. 检测“抬起瞬间”（Up）
                if (mouse.leftButton.wasReleasedThisFrame)
                {
                    isOnOk = true;

                    //Debug.Log("鼠标左键刚刚被松开！");
                    // 在这里执行松开操作
                    if (nowObj != null)
                    {
                        if (nowObj.isInTrigger)
                        {
                            // 吸附
                            //nowObj.isOk = true;
                            //nowObj.transform.position = nowObj.point;
                            EventCenterSystem.Instance.EventTrigger(E_EventEnum.E_MouseUp);
                        }
                        else
                        {
                            // 回原位
                            nowObj.Set0Point(nowObj.point);
                        }

                        nowObj = null;
                        //nowSelObj.gameObject.GetComponent<OutlineController>().HideOutline();
                    }
                }
                break;
            case E_InputType.nil:
                break;
        }
        
    }
}
