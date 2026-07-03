using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 角色行为状态接口
/// </summary>
public interface IBehavioralState
{
    /// <summary>
    /// 状态进入
    /// </summary>
    /// <param name="p"></param>
    public void Enter(PlayerStateMachine p);

    /// <summary>
    /// 状态离开
    /// </summary>
    /// <param name="p"></param>
    public void Exit(PlayerStateMachine p);
    /// <summary>
    /// 状态更新
    /// </summary>
    /// <param name="p"></param>
    public void Update(Player.PlayerData playData, PlayerInputData input, PlayerStateMachine stateMachine);

}
/// <summary>
/// 是否在地面
/// </summary>
public class IsOnGround : IBehavioralState
{
    public void Enter(PlayerStateMachine p)
    {
        //Debug.Log("地面状态进入");
        p.jumpNum = 1;
    }

    public void Exit(PlayerStateMachine p)
    {
        //Debug.Log("地面状态退出");
    }



    public void Update(Player.PlayerData playData, PlayerInputData input, PlayerStateMachine stateMachine)
    {
        if (input.jumpPressed)
        {
            //可跳跃
            //playData.rb.velocity=new Vector2(playData.rb.velocity.x,playData.upSpeed);
            playData.verticalVelocity = playData.upSpeed;
            stateMachine.jumpNum -= 1;
            //触发状态复原，此处已经交给状态机
        }
        //若已经不在地面，则改变状态
        if (!playData.isGrounded)
        {
            //因为此类型状态没有成员变量，所以，直接靠成员变量来传入，避免频繁GC
            stateMachine.ChangeState(stateMachine.inAir);
        }
        //水平速度的持续更新
        playData.HorizontalSpeed = input.moveInput * playData.speed;

    }
}

public class IsInAir : IBehavioralState
{
    float nowTime;
    /// <summary>
    /// 跳跃缓冲时间
    /// </summary>
    float maxTime = 0.1f;
    public void Enter(PlayerStateMachine p)
    {
        //Debug.Log("空中状态进入");
        nowTime = maxTime;
    }

    public void Exit(PlayerStateMachine p)
    {
        //Debug.Log("空中状态退出");
        nowTime = 0;
    }


    public void Update(Player.PlayerData playData, PlayerInputData input, PlayerStateMachine stateMachine)
    {
        if (nowTime > 0 && stateMachine.jumpNum > 0)
        {
            //可跳跃
            if (input.jumpPressed)
            {
                playData.verticalVelocity = playData.upSpeed;
                stateMachine.jumpNum -= 1;
                nowTime = 0;
            }
            nowTime -= Time.deltaTime;

        }
        if (playData.isGrounded)
        {
            //因为此类型状态没有成员变量，所以，直接靠成员变量来传入，避免频繁GC
            stateMachine.ChangeState(stateMachine.onGround);
        }
        //水平速度的持续更新
        playData.HorizontalSpeed = input.moveInput * playData.speed;
    }
}