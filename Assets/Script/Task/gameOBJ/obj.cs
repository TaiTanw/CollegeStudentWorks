using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj : MonoBehaviour,I_Obj
{
    public int id;
    public void HideMe()
    {
        transform.position=new Vector3(1000,1000,transform.position.z); 
    }

    public void ShowMe()
    {
        transform.DOMove(point, 0.5f);//SetEase(Ease.OutBounce);
        //Debug.Log(name + " ShowMe");
    }
    public bool isInTrigger;
    public Vector3 point;
    void Start()
    {
        //记录终点位置
        point = transform.position;
        transform.position = Vector3.Lerp(transform.position, new Vector3(point.x, point.y - 100, point.z),0.5f);
        SceneResMgr.Instance.AddObj(this);

    }
    /// <summary>
    /// 设置到固定位置
    /// </summary>
    public void Set0Point(Vector3 nowPoint)
    {
        transform.position=nowPoint;
    }
    /// <summary>
    /// 未准备好才可拖动
    /// </summary>
    public bool isOk;
    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 改为默认层级，表示这个物体已经完成建造
    /// </summary>
    public void ChangeLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
