using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    private RectTransform rectTransform;
    private Vector2 originalPosition; // 记录原始位置
    Text title;
    Button A;
    Button B;
    Button C;
    Button D;
    Button bt1;
    Text t2;
    Text tA;
    Text tB;
    Text tC;
    Text tD;

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        // 记录原始位置（即你在场景中摆放好的位置）
        originalPosition = rectTransform.anchoredPosition;
        title = FindUIObj<Text>("t1");
        t2 = FindUIObj<Text>("t2");
        A = FindUIObj<Button>("A");
        B = FindUIObj<Button>("B");
        C = FindUIObj<Button>("C");
        D = FindUIObj<Button>("D");
        bt1 = FindUIObj<Button>("bt1");
        tA = FindUIObj<Text>("ta");
        tB = FindUIObj<Text>("tb");
        tC = FindUIObj<Text>("tc");
        tD = FindUIObj<Text>("td");
        
        
    }
    private void OnEnable()
    {
        //print("开始");
        A.gameObject.SetActive(true);
        B.gameObject.SetActive(true);
        C.gameObject.SetActive(true);
        D.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        t2.gameObject.SetActive(true);
        SceneInt si = SceneResMgr.Instance.GetClass();
        int i = si.xxiang;
        if (i == 1)
        {
            SetHide();
            title.gameObject.SetActive(false);
        }
        else if (i == 2)
        {
            C.gameObject.SetActive(false);
            D.gameObject.SetActive(false);
            bt1.gameObject.SetActive(false);
            t2.gameObject.SetActive(false);
        }
        else if (i == 3)
        {
            D.gameObject.SetActive(false);
            bt1.gameObject.SetActive(false);
            t2.gameObject.SetActive(false);
        }
        else if (i == 4)
        {
            bt1.gameObject.SetActive(false);
            t2.gameObject.SetActive(false);
        }

        title.text=si.question;
        t2.text = si.analysis;
        tA.text=si.tA; tB.text=si.tB; tC.text=si.tC;tD.text=si.tD;
    }
    void SetHide()
    {
        A.gameObject.SetActive(false );
        B.gameObject.SetActive(false );
        C.gameObject.SetActive(false );
        D.gameObject.SetActive(false );
    }

    protected override void OnClickButton(string UIname)
    {
        base.OnClickButton(UIname);
        switch (UIname) 
        {
            case "A":
                //title.text = "正确答案：...";
                bt1.gameObject.SetActive(true);
                title.gameObject.SetActive(false);
                t2.gameObject.SetActive(true);
                SetHide();
                break;
            case "B":
                //title.text = "正确答案：...";
                bt1.gameObject.SetActive(true);
                title.gameObject.SetActive(false);
                t2.gameObject.SetActive(true);
                SetHide();
                break;
            case "C":
                //title.text = "正确答案：...";
                bt1.gameObject.SetActive(true);
                title.gameObject.SetActive(false);
                t2.gameObject.SetActive(true);
                SetHide();
                break;
            case "D":
                //title.text = "正确答案：...";
                bt1.gameObject.SetActive(true);
                title.gameObject.SetActive(false);
                t2.gameObject.SetActive(true);
                SetHide();
                break;
            case "bt1":
                UIMgr.Instance.HideOneUI<GamePanel>();
                UIMgr.Instance.ShowOneUI<Massage2Panel>(E_UILayer.Top, (a) =>
                {
                    a.SetText("开始建造：将合适的物品拖入框内");
                });
                SceneResMgr.Instance.StartToShow();
                InputControlMgr.Instance.BuffChange(E_InputType.tuo);
                break;
        }

        
        //UIMgr.Instance.HideOneUI<GamePanel>();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        canvasGroup.alpha = 1f;          // 恢复透明度
        canvasGroup.blocksRaycasts = true;
        rectTransform.DOKill();
        canvasGroup.DOKill();
        // 1. 先将 UI 移动到屏幕外上方（例如 Y 轴偏移 500）
        rectTransform.anchoredPosition = originalPosition + new Vector2(0, 500);

        // 2. 播放动画，移动到原始位置
        rectTransform.DOAnchorPos(originalPosition, 0.5f)
                     .SetEase(Ease.OutBack).OnComplete(() =>
                     {
                         //回调
                     }); // 可选弹性缓动
    }

    public override void HideMe()
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 1f;
        //禁用交互
        canvasGroup.blocksRaycasts = false;
        // 播放淡出动画，结束时禁用物体
        canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {

            gameObject.SetActive(false);
        });
    }
}

