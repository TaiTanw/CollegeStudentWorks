using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{

    
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ShowMe()
    {
        // 确保物体激活，但透明度为0（不可见）
        gameObject.SetActive(true);
        //canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true; // 如果需要交互，可设为 true
                                           // 播放淡入动画
        canvasGroup.DOFade(1f, 0.3f);
        //print("11111111");
    }

    public override void HideMe()
    {
        // 确保当前完全不透明
        //canvasGroup.alpha = 1f;
        //print("22222222222");
        //禁用交互
        canvasGroup.blocksRaycasts = false;
        // 播放淡出动画，结束时禁用物体
        canvasGroup.DOFade(0f, 3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
