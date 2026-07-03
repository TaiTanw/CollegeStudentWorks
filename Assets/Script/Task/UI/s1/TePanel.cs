using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TePanel : BasePanel
{
    Text t1;
    protected override void Awake()
    {
        base.Awake();
        t1 = FindUIObj<Text>("t1");
    }

    private void OnEnable()
    {
        t1.text = "在山水相依的徽州大地上，你将以专业建筑师的身份，受邀重建徽州古村的经典民居，重现传统建筑的营造智慧与文化底蕴。本项目以2D可视化交互为载体，带你完整经历徽派民居从选址规划、结构搭建到形制装饰、地域适配的全过程，通过沉浸式营造体验，揭开徽派建筑的独特魅力。从相地选址的山水考量，到基础夯实与柱网立定；从抬梁穿斗的榫卯咬合，到天井院落的采光纳气；从马头墙的错落封火，到砖木石三雕的精巧饰面——每一个环节都严格遵循古法，每一次抉择都呼应着“天人合一”的营造哲学。在重建过程中，我们将触摸榫卯结构的环环相扣，感受天井聚水聚财的吉祥寓意，体会马头墙防风防火的实用智慧，领悟徽州三雕中寄寓的诗礼传家。古人因地就势、取材有度的智慧，藏在每一根梁架与每一块青石之中，等待你亲手唤醒。这不仅是一次建筑还原，更是一场与传统匠心的深度对话，让你在方寸屏幕之间，守护并传承属于东方的建筑美学与文化根脉";
    }

    protected override void OnClickButton(string UIname)
    {
        base.OnClickButton(UIname);
        switch (UIname)
        {
            case "b1":
                UIMgr.Instance.HideOneUI<TePanel>();
                UIMgr.Instance.ShowOneUI<BeginPanel>();
                break;
            case "b2":
                UIMgr.Instance.HideOneUI<TePanel>();
                SceneResMgr.Instance.ToChangeScence();
                break;
        }
    }
    public override void ShowMe()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true; // 如果需要交互，可设为 true
                                           // 播放淡入动画
        canvasGroup.DOFade(1f, 0.3f);
    }

    public override void HideMe()
    {
        canvasGroup.alpha = 1f;
        //禁用交互
        canvasGroup.blocksRaycasts = false;
        // 播放淡出动画，结束时禁用物体
        canvasGroup.DOFade(0f, 0.1f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
