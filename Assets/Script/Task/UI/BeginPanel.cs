using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    protected override void Awake()
    {
        //注意：若初始化写在start函数内，会后于show方法调用，逻辑上有误，可能导致show在调用组件时候未初始化
        base.Awake();
        AddEventAction();
        //添加场景加载监听，获取加载进度
        //EventCenterSystem.Instance.AddEventListener<float>(E_EventEnum.E_LoadScene, LoadChange);
        //print("2，开始面板初始化成功");

    }
    void Start()
    {
        
    }
    private void LoadChange(float value)
    {
        print(value);
    }
    
    protected override void OnClickButton(string UIname)
    {
        base.OnClickButton(UIname);
        switch(UIname)
        {
            case "t1":
                UIMgr.Instance.HideOneUI<BeginPanel>();
                UIMgr.Instance.ShowOneUI<TePanel>();
                break;
            case "t2":
                //print("t2触发");
                UIMgr.Instance.ShowOneUI<MusicSetPanel>();
                break;
            case "t3":
                //print("t3触发");
                Application.Quit();
                break;
        }
    }

  

    public override void ShowMe()
    {
        // 确保物体激活，但透明度为0（不可见）
        //print("11111");
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true; // 如果需要交互，可设为 true
                                           // 播放淡入动画
        canvasGroup.DOFade(1f, 0.3f);
    }

    public override void HideMe()
    {
        // 确保当前完全不透明
        //print("222222");
        //Debug.Log("HideMe 被调用，调用堆栈：\n" + Environment.StackTrace);
        canvasGroup.alpha = 1f;
        //禁用交互
        canvasGroup.blocksRaycasts = false;
        // 播放淡出动画，结束时禁用物体
        canvasGroup.DOFade(0f, 0.1f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    //private void OnDestroy()
    //{
    //    //事件监听增减需配对
    //    //EventCenterSystem.Instance.RemoveEventListener<float>(E_EventEnum.E_LoadScene, LoadChange);
    //}

    public void TeskPanel()
    {
        print("获取面板。执行逻辑");
    }
    
    /// <summary>
    /// 自定义事件添加
    /// </summary>
    public void AddEventAction()
    {
        //AddEventTriggerListener<Button>("t1", UnityEngine.EventSystems.EventTriggerType.PointerEnter, (a) =>
        //{
        //    print("鼠标进入");
        //});
        //AddEventTriggerListener<Button>("t1", UnityEngine.EventSystems.EventTriggerType.PointerExit, (a) =>
        //{
        //    print("鼠标离开");
        //});
    }
}
