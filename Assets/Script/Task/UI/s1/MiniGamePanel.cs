using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 指针定位小游戏面板
/// 复刻网页小游戏：移动方块在区域内停下即为成功
/// </summary>
/// AI辅助生成：DeepSeek-V3.2, 2026-4-09
public class MiniGamePanel : BasePanel
{
    [Header("UI组件引用")]
    [SerializeField] private RectTransform d1; // 背景条 (Bar)
    [SerializeField] private RectTransform d2; // 响应区域 (Zone)
    [SerializeField] private RectTransform d3; // 移动方块 (Pointer)

    [Header("速度配置")]
    [SerializeField] private float minSpeed = 150f;   // 最小移动速度 (像素/秒)
    [SerializeField] private float maxSpeed = 350f;   // 最大移动速度 (像素/秒)

    // ==================================================
    //  偏移修正配置 
    // 若发现方块移动范围/区域位置与实际显示有偏差，调整此值。
    // 正值表示整体向右偏移，负值表示向左偏移。
    // 单位：像素 (基于Canvas参考分辨率下的本地坐标)
    // ==================================================
    [Header("偏移修正")]
    [SerializeField] private float positionOffset = 0f;

    private float currentSpeed;         // 当前移动速度
    private float direction = 1f;       // 移动方向: 1 向右, -1 向左
    private bool isMoving = true;       // 是否正在移动中
    private float barWidth;             // 背景条宽度 (缓存)


    private RectTransform rectTransform;
    private Vector2 originalPosition; // 记录原始位置
    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        // 记录原始位置（即你在场景中摆放好的位置）
        originalPosition = rectTransform.anchoredPosition;
        // 缓存背景条宽度，用于边界计算
        barWidth = d1.rect.width;
    }

    private void OnEnable()
    {
        // --- 每次面板显示时执行初始化 ---

        // 1. 随机化移动速度
        // 你可以在此处直接修改 minSpeed / maxSpeed 来调整速度范围
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        // 2. 随机响应区域的位置和宽度
        RandomizeZone();

        // 3. 重置方块位置到起点 (比如最左侧) —— 应用偏移修正
        d3.anchoredPosition = new Vector2(0 + positionOffset, d3.anchoredPosition.y);
        direction = 1f;     // 默认向右移动
        isMoving = true;
    }

    private void Update()
    {
        // 移动方块逻辑
        if (isMoving)
        {
            MovePointer();
        }

        // --- 输入检测 ---
        // 假设 InputControlMgr 已正确实现单例，且 keyDown 属性可读写
        if (InputControlMgr.Instance.keyDown)
        {
            // 立即重置输入标志，防止重复触发
            InputControlMgr.Instance.keyDown = false;

            // 执行触发判断
            bool isHit = CheckHit();

            // ============================================
            //  在此处根据 isHit 编写成功/失败分支 
            // ============================================
            if (isHit)
            {
                // TODO: 成功逻辑 —— 例如播放成功音效、显示成功文本、进入下一阶段等
                //Debug.Log("命中区域！");
                // 停止移动
                isMoving = false;
                SceneResMgr.Instance.IsOkorNo(-10,false);
                MusicMgr.Instance.StartSound("正确音效");
                UIMgr.Instance.HideOneUI<MiniGamePanel>();
                UIMgr.Instance.ShowOneUI<Massage2Panel>(E_UILayer.Top, (a) =>
                {
                    a.SetText("完成！");
                });
                InputControlMgr.Instance.BuffChange(E_InputType.tuo);
                // 可以在这里调用后续流程，例如 Invoke("NextStep", 0.5f);
            }
            else
            {
                // TODO: 失败逻辑 —— 例如播放失败动画、重置游戏、显示失败提示等
                //Debug.Log("未命中区域...");
                MusicMgr.Instance.StartSound("错误1");
                // 通常失败后需要重置或结束游戏
                //isMoving = false;
                // 例如延迟重新开始：Invoke("ResetGame", 0.8f);
            }
            // ============================================
            // 分支结束，请在上方填写你的逻辑
            // ============================================
        }
    }

    /// <summary>
    /// 移动方块 (d3) 在背景条内左右反弹
    /// </summary>
    private void MovePointer()
    {
        // 计算本帧位移量 (与时间相关，保证不同帧率下速度一致)
        float delta = currentSpeed * direction * Time.deltaTime;
        float newX = d3.anchoredPosition.x + delta;

        // 边界反弹检测 —— 应用偏移修正
        // 方块宽度由 RectTransform 决定，通常 d3 有固定宽度
        float pointerWidth = d3.rect.width;
        float minX = 0 + positionOffset;
        float maxX = barWidth - pointerWidth + positionOffset;

        if (newX <= minX)
        {
            newX = minX;
            direction = 1f; // 改为向右
        }
        else if (newX >= maxX)
        {
            newX = maxX;
            direction = -1f; // 改为向左
        }

        d3.anchoredPosition = new Vector2(newX, d3.anchoredPosition.y);
    }

    /// <summary>
    /// 随机化响应区域 (d2) 的位置和宽度
    /// 区域高度与背景条一致，只改变水平位置和宽度
    /// </summary>
    private void RandomizeZone()
    {
        // 背景条总宽度
        float totalWidth = barWidth;

        // --- 随机区域宽度 (你可以在此处调整范围) ---
        // 例如：最小宽度 50，最大宽度 150
        float minZoneWidth = 60f;
        float maxZoneWidth = 120f;
        float zoneWidth = Random.Range(minZoneWidth, maxZoneWidth);

        // --- 随机区域水平起始位置 (保证区域不超出背景条) —— 应用偏移修正 ---
        float maxStartX = totalWidth - zoneWidth;
        float startX = Random.Range(0f, maxStartX) + positionOffset;

        // 设置 d2 的尺寸和位置
        d2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, zoneWidth);
        d2.anchoredPosition = new Vector2(startX, d2.anchoredPosition.y);

        // 可选：保持高度与背景条一致 (通常已在预制体中设置好)
    }

    /// <summary>
    /// 触发方法：判断移动方块(d3)是否与响应区域(d2)有重叠
    /// </summary>
    /// <returns>true: 方块在区域内 (成功); false: 不在区域内 (失败)</returns>
    public bool CheckHit()
    {
        // 获取方块的世界坐标范围 (使用 RectTransform 的 GetWorldCorners 更准确)
        Vector3[] pointerCorners = new Vector3[4];
        d3.GetWorldCorners(pointerCorners);
        float pointerLeft = pointerCorners[0].x;
        float pointerRight = pointerCorners[2].x;

        // 获取响应区域的世界坐标范围
        Vector3[] zoneCorners = new Vector3[4];
        d2.GetWorldCorners(zoneCorners);
        float zoneLeft = zoneCorners[0].x;
        float zoneRight = zoneCorners[2].x;

        // ==================================================
        //  重叠判断逻辑 (只要有重叠部分即算命中) 
        // ==================================================
        // 两个区间 [pointerLeft, pointerRight] 与 [zoneLeft, zoneRight] 是否有交集
        bool isOverlap = (pointerLeft < zoneRight) && (pointerRight > zoneLeft);
        // 注释说明：只要方块右边界大于区域左边界，且方块左边界小于区域右边界，即存在重叠。
        // 由于高度一致，仅判断水平方向即可。
        // ==================================================

        return isOverlap;
    }

    // 可选：重置游戏状态 (供外部调用或内部延时调用)
    public void ResetGame()
    {
        OnEnable();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        rectTransform.anchoredPosition = originalPosition + new Vector2(0, -500);
        rectTransform.DOAnchorPos(originalPosition, 0.5f)
                     .SetEase(Ease.OutBack).OnComplete(() =>
                     {
                         //回调
                     }); // 可选弹性缓动
    }

    public override void HideMe()
    {
        //base.HideMe();
        rectTransform.DOAnchorPos(originalPosition + new Vector2(0, -500), 0.5f)
                     .SetEase(Ease.OutBack).OnComplete(() =>
                     {
                         base.HideMe();//回调
                     }); // 可选弹性缓动
    }
}