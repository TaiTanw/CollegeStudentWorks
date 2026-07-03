using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换管理器
/// </summary>
public class SceneChangeMgr : BaseMgr<SceneChangeMgr>
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    SceneImageCollllll nowA = null;
    /// <summary>
    /// 异步场景切换
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="callBack"></param>
    public void LoadSceneAsync(string sceneName,UnityAction callBack=null)
    {
        PoolMgr.Instance.ClearPoolObj();
        //清空此场景所有无特殊标记的物体
        SceneResMgr.Instance.CleanAll();
        UIMgr.Instance.ShowOneUI<LoadingPanel>();
        AsyncOperation tion = SceneManager.LoadSceneAsync(sceneName);
        MonoPublicMgr.Instance.StartCoroutine(Load(tion,callBack));
        //nowA=null;
    }

    IEnumerator Load(AsyncOperation ao,UnityAction action)
    {
        while (!ao.isDone)
        {
            //事件分发，用于外部获取加载进度
            EventCenterSystem.Instance.EventTrigger<float>(E_EventEnum.E_LoadScene,ao.progress);
            yield return 0;
        }
        UIMgr.Instance.HideOneUI<LoadingPanel>();
        EventCenterSystem.Instance.EventTrigger<float>(E_EventEnum.E_LoadScene, 1);
        //加载场景完成后执行逻辑
        //得到当前场景的    a
        //
        nowA = GameObject.Find("a").GetComponent<SceneImageCollllll>();
        
        action?.Invoke();
    }
    /// <summary>
    /// 执行！！！！！！！
    /// </summary>
    public void ZZZZZZZZZ()
    {
        if (nowA == null)
        {
            //Debug.Log("只是我还在");
        }
        else
        {
            nowA.ShowImage();
        }
    }

    public void LoadResAndScene()
    {

    }
}
