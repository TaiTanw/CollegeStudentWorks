using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class kuang : MonoBehaviour, I_Obj
{
    Vector3 point;
    public int id;

    private obj currentObj;
    private bool isIn;
    private bool isKOK = false;
    private bool isAnimating = false;
    private Coroutine delayedClearCoroutine;

    public bool isTool;

    void Start()
    {
        point = transform.position;
        transform.position = new Vector3(point.x, point.y + 100, point.z);
        EventCenterSystem.Instance.AddEventListener(E_EventEnum.E_MouseUp, MouseUpToOk);
        SceneResMgr.Instance.AddObj(this);
    }

    void MouseUpToOk()
    {
        if (isKOK || isAnimating) return;
        if (currentObj == null) return;

        // 如果物体已标记为可吸附（ID匹配且仍在触发器内）
        if (currentObj.isOk && isIn)
        {
            if (!SceneResMgr.Instance.IsOkorNo(id, isTool))
            {
                // 匹配失败或物体已离开触发器（因为延迟清空所以currentObj仍有值）
                isAnimating = true;

                MusicMgr.Instance.StartSound("错误1");

                // 取消任何待处理的清空协程
                if (delayedClearCoroutine != null)
                {
                    StopCoroutine(delayedClearCoroutine);
                    delayedClearCoroutine = null;
                }

                obj tempObj = currentObj;
                currentObj = null;
                isIn = false;

                tempObj.transform.DOMove(tempObj.point, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    isAnimating = false;
                });

                tempObj.isInTrigger = false;
                tempObj.isOk = false;

                UIMgr.Instance.ShowOneUI<Massage2Panel>(E_UILayer.Top, (a) =>
                {
                    a.SetText("顺序错误");
                });
                return;
            }

            if (!isTool)
            {


                MusicMgr.Instance.StartSound("正确音效");

                //==========================================================================================

            }
            else
            {
                UIMgr.Instance.ShowOneUI<Massage2Panel>(E_UILayer.Top, (a) =>
                {
                    a.SetText("按空格让准星对准区域，完成建造");

                });
                UIMgr.Instance.ShowOneUI<MiniGamePanel>();
                InputControlMgr.Instance.BuffChange(E_InputType.key);
                SceneResMgr.Instance.teshu = currentObj;
            }

            // 成功吸附
            isKOK = true;
            isAnimating = true;

            // 如果有待执行的延迟清空协程，立即停止（防止清空currentObj）
            if (delayedClearCoroutine != null)
            {
                StopCoroutine(delayedClearCoroutine);
                delayedClearCoroutine = null;
            }

            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, currentObj.point.z);
            currentObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                currentObj.ChangeLayer();
                isAnimating = false;



                // 完成后清空引用
                currentObj = null;
                isIn = false;
            });
        }
        else
        {
            // 匹配失败或物体已离开触发器（因为延迟清空所以currentObj仍有值）
            isAnimating = true;

            MusicMgr.Instance.StartSound("错误1");

            // 取消任何待处理的清空协程
            if (delayedClearCoroutine != null)
            {
                StopCoroutine(delayedClearCoroutine);
                delayedClearCoroutine = null;
            }

            obj tempObj = currentObj;
            currentObj = null;
            isIn = false;

            tempObj.transform.DOMove(tempObj.point, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                isAnimating = false;
            });

            tempObj.isInTrigger = false;
            tempObj.isOk = false;

            UIMgr.Instance.ShowOneUI<Massage2Panel>(E_UILayer.Top, (a) =>
            {
                a.SetText("匹配错误");
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isKOK || isAnimating) return;

        obj o = collision.GetComponent<obj>();
        if (o != null)
        {
            // 如果之前有延迟清空协程，立即停止
            if (delayedClearCoroutine != null)
            {
                StopCoroutine(delayedClearCoroutine);
                delayedClearCoroutine = null;
            }

            o.isInTrigger = true;
            currentObj = o;
            isIn = true;
            o.isOk = (id == o.id);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isKOK || isAnimating) return;

        obj exiting = collision.GetComponent<obj>();
        if (exiting != null && exiting == currentObj)
        {
            if (delayedClearCoroutine != null)
                StopCoroutine(delayedClearCoroutine);
            // 在这里加判断 ↓↓↓
            if (gameObject.activeInHierarchy)
            {
                delayedClearCoroutine = StartCoroutine(DelayedClear(exiting));
            }
            else
            {
                // 物体已经失活，直接清空引用，不需要启动协程
                exiting.isInTrigger = false;
                exiting.isOk = false;
                currentObj = null;
                isIn = false;
            }
        }

    }

    IEnumerator DelayedClear(obj exiting)
    {
        yield return new WaitForEndOfFrame();
        // 延迟后，若当前物体仍是这个物体且未被成功吸附，则清空
        if (currentObj == exiting && !isKOK)
        {
            exiting.isInTrigger = false;
            exiting.isOk = false;
            currentObj = null;
            isIn = false;
        }
        delayedClearCoroutine = null;
    }

    private void OnDestroy()
    {
        EventCenterSystem.Instance.RemoveEventListener(E_EventEnum.E_MouseUp, MouseUpToOk);
    }

    public void ShowMe()
    {
        transform.DOMove(point, 0.5f);
    }

    public void HideMe()
    {
        transform.position = new Vector3(1000, 1000, transform.position.z);
    }
}