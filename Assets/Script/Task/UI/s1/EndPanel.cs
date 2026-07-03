using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : BasePanel
{
    Text Text;
    //float time=3f;
    private RectTransform rectTransform;
    private Vector2 originalPosition; // 记录原始位置
    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        // 记录原始位置（即你在场景中摆放好的位置）
        originalPosition = rectTransform.anchoredPosition;
        Text = FindUIObj<Text>("t1");

    }
    void Start()
    {
        //print(Text.text);
    }
    private void OnEnable()
    {
        Text.text = SceneResMgr.Instance.GetClass().end;
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
                         canvasGroup.blocksRaycasts = true;
                     }); // 可选弹性缓动
    }

    public override void HideMe()
    {
        //base.HideMe();
        canvasGroup.DOKill();
        canvasGroup.alpha = 1f;
        //禁用交互
        canvasGroup.blocksRaycasts = false;
        // 播放淡出动画，结束时禁用物体
        canvasGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            //now = 0;
            //toHide = false;
            gameObject.SetActive(false);
        });
    }
    protected override void OnClickButton(string UIname)
    {
        base.OnClickButton(UIname);
        UIMgr.Instance.HideOneUI<EndPanel>();
        SceneResMgr.Instance.ToChangeScence();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
