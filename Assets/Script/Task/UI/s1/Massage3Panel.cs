using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Massage3Panel : BasePanel
{
    Text Text;
    Text Text2;
    //float time=3f;
    private RectTransform rectTransform;
    private Vector2 originalPosition; // 记录原始位置
    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        // 记录原始位置（即你在场景中摆放好的位置）
        originalPosition = rectTransform.anchoredPosition;
        Text = FindUIObj<Text>("tt1");
        Text2 = FindUIObj<Text>("tt2");
    }
    private void OnEnable()
    {
        canvasGroup.blocksRaycasts = true;
    }
    private void Start()
    {

    }
    /// <summary>
    /// 显示文本
    /// </summary>
    /// <param name="text"></param>
    public void SetTextShow(string text, int id = 1)
    {
        if (id == 1)
            Text.text = text;
        else
            Text2.text = text;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //now = 0;
        //toHide = false;
        rectTransform.DOKill();
        canvasGroup.DOKill();
        canvasGroup.alpha = 1f;
        // 1. 先将 UI 移动到屏幕外下方（例如 Y 轴偏移 -500）
        rectTransform.anchoredPosition = originalPosition + new Vector2(0, -500);

        // 2. 播放动画，移动到原始位置
        rectTransform.DOAnchorPos(originalPosition, 0.5f)
                     .SetEase(Ease.OutBack).OnComplete(() =>
                     {
                         //回调
                     }); // 可选弹性缓动
    }

    //float now = 0;
    //sbool toHide;
    private void Update()
    {
        //now += Time.deltaTime;
        //if (now > time&&!toHide)
        //{
        //    HideMe();
        //    toHide = true;
        //}
    }

    public override void HideMe()
    {
        //base.HideMe();
        canvasGroup.DOKill();
        canvasGroup.alpha = 1f;
        //禁用交互
        canvasGroup.blocksRaycasts = false;
        // 播放淡出动画，结束时禁用物体
        canvasGroup.DOFade(0f, 2f).OnComplete(() =>
        {
            //now = 0;
            //toHide = false;
            gameObject.SetActive(false);
        });
    }

    protected override void OnClickButton(string UIname)
    {
        base.OnClickButton(UIname);
        UIMgr.Instance.ShowOneUI<EndPanel>();
        HideMe();


    }
}
