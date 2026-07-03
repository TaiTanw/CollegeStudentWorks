using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试用例
/// </summary>
public class TaskSp : MonoBehaviour
{
    //需要注意public变量会被编辑器持久化，且编辑器有更高优先级
    //当UI数值与成员变量关联时，private更加合适

    //此处数据后续用于对接本地持久化



    void Start()
    {
        //UIMgr.Instance.Init();
        //print(DataAndInitMgr.Instance.asset.FindAction("Move").bindings[1].path);
        //BaseCollection a = new BaseCollection();
        //GameObjData d = new GameObjData();
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }
    private void OnGUI()
    {

        if (GUILayout.Button("返回开始界面"))
        {
            UIMgr.Instance.CleanPanle();
            UIMgr.Instance.ShowOneUI<LoadingPanel>();
            //PoolMgr.Instance.ClearPoolObj();
            SceneChangeMgr.Instance.LoadSceneAsync("EndScenes", () =>
            {
                SceneResMgr.Instance.isGame=false;
                UIMgr.Instance.HideOneUI<LoadingPanel>();
                SceneResMgr.Instance.End();
                UIMgr.Instance.ShowOneUI<BeginPanel>();
                MusicMgr.Instance.PlayBkMusic("BkMusic");
                //PoolMgr.Instance.ClearPoolObj();
                //print("返回开始界面1");
            });
        }

    }

}
