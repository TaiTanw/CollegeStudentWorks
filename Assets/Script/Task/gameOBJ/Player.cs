using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    /// <summary>
    /// 角色图片
    /// </summary>
    public SpriteRenderer spriteRenderer;
    public class PlayerData
    {
        public bool isGrounded;     // 物理检测
        public float speed = 10;    // 基础水平移速
        public float upSpeed = 15;//基础跳跃速度

        public float HorizontalSpeed;   //当前水平速度
        public float verticalVelocity;  //当前竖直速度
        public float gravity = -35f;    //重力速度
        public PlayerData()
        {
            isGrounded = true;
        }
    }

    /// <summary>
    /// 玩家当前数据
    /// </summary>
    PlayerData data;
    /// <summary>
    /// 玩家当前输入数据,初始化交给外部
    /// </summary>
    PlayerInputData inputData;

    #region 物理相关
    public Rigidbody2D rb; //实际速度
    /// <summary>
    /// 检测中心
    /// </summary>
    [SerializeField]
    private Transform groundV;
    /// <summary>
    /// 矩形宽高
    /// </summary>

    private Vector2 size = new Vector2(0.75f, 0.1f);
    /// <summary>
    /// 检测层级
    /// </summary>
    [SerializeField]
    private LayerMask groundLayer;
    /// <summary>
    /// 当前玩家所属平台
    /// </summary>
   // private Taijie nowtaijie;
    #endregion

    /// <summary>
    /// 玩家状态机对象
    /// </summary>
    PlayerStateMachine fsm;
    void Start()
    {
        //玩家状态信息初始化
        data = new PlayerData();
        //玩家输入信息初始化
        //inputData =new PlayerInputData();此处会导致数据引用错误，所以初始化交给外部的输入管理系统
        //玩家状态机初始化
        fsm = new PlayerStateMachine();
        //刚体初始化
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        //物理更新时序为1层
        //MonoPublicMgr.Instance.actionsLen[1] += FixFun;
        //spriteRenderer = GetComponent<SpriteRenderer>();

    }
    /// <summary>
    /// 注入玩家按键数据行为
    /// </summary>
    /// <param name="data"></param>
    public void ChangeInputAsset(PlayerInputData data)
    {
        inputData = data;
    }
    //float time;
    void Update()
    {
        //速度自检
        //time += Time.deltaTime;
        //if (time > 3)
        //{
        //    print("垂直速度"+data.verticalVelocity);
        //    print("水平速度"+data.HorizontalSpeed);
        //    time = 0;
        //}
    }

    private void LateUpdate()
    {

    }
    //碰撞器缓存
    RaycastHit2D hit;
    //void FixFun()
    //{
    //    fsm.Update(data, inputData);

    //    //col = Physics2D.OverlapBox(groundV.position, size, 0, groundLayer);
    //    hit = Physics2D.BoxCast(groundV.position, size, 0, Vector2.down, 0f, groundLayer);

    //    data.isGrounded = false;
    //    nowtaijie = null;

    //    if (hit.collider != null)
    //    {
    //        // 关键：判断是否在平台上方
    //        //if (groundV.position.y >= col.bounds.max.y - 0.05f)
    //        if (Vector2.Dot(hit.normal, Vector2.up) > 0.7f)
    //        {
    //            data.isGrounded = true;
    //            nowtaijie = hit.collider.GetComponent<Taijie>();
    //        }
    //    }

    //    // 水平速度判断
    //    if (data.HorizontalSpeed < 0)
    //        spriteRenderer.flipX = true;
    //    if (data.HorizontalSpeed > 0)
    //        spriteRenderer.flipX = false;
    //    // 重力
    //    if (!data.isGrounded)
    //    {
    //        data.verticalVelocity += data.gravity * Time.fixedDeltaTime;
    //    }
    //    else if (data.verticalVelocity < 0)
    //    {
    //        data.verticalVelocity = 0;
    //    }

    //    Vector2 velocity = new Vector2(data.HorizontalSpeed, data.verticalVelocity);
    //    Vector2 moveDelta = velocity * Time.fixedDeltaTime;

    //    //  平台补偿
    //    Vector2 platformDelta = Vector2.zero;

    //    if (nowtaijie != null && data.isGrounded)
    //    {
    //        platformDelta = nowtaijie.delta;
    //    }

    //    //  一次性统一移动
    //    rb.MovePosition(rb.position + moveDelta + platformDelta);
    //}



    private void OnDestroy()
    {
        //MonoPublicMgr.Instance.actionsLen[1] -= FixFun;
    }
}

/// <summary>
/// 玩家状态机
/// </summary>
public class PlayerStateMachine
{
    /// <summary>
    /// 当前状态
    /// </summary>
    public IBehavioralState onState;

    public IsOnGround onGround;
    public IsInAir inAir;
    /// <summary>
    /// 可跳跃次数
    /// </summary>
    public int jumpNum = 1;
    public PlayerStateMachine()
    {
        onGround = new IsOnGround();
        inAir = new IsInAir();
        //当前状态信息初始化
        onState = onGround;
    }
    /// <summary>
    /// 状态改变
    /// </summary>
    public void ChangeState(IBehavioralState state)
    {
        onState.Exit(this);
        onState = state;
        onState.Enter(this);
    }



    /// <summary>
    /// 供玩家mono执行的玩家更新逻辑
    /// </summary>
    /// <param name="playData">玩家当前状态</param>
    /// <param name="input">当前输入信息</param>
    /// <param name="stateMachine">玩家自身状态机</param>
    public void Update(Player.PlayerData playData, PlayerInputData input)
    {
        //传入执行，状态类负责实际功能
        onState.Update(playData, input, this);
        //效果已经触发，则关闭，防止持续跳跃
        input.jumpPressed = false;//触发类型按键全部统一由状态机复原
    }
}
