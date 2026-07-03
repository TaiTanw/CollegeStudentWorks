using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Obj
{
    /// <summary>
    /// 显示
    /// </summary>
    public void  ShowMe();

    public void HideMe();
}

public class SceneResMgr : BaseMgr<SceneResMgr>
{
    /// <summary>
    /// 专属场景的物体，运行时数据
    /// </summary>
    List<I_Obj> sceneObj = new List<I_Obj>();
    /// <summary>
    /// 场景的顺序编号，-1表示无序
    /// </summary>
    List<int> serialNumber = new List<int>() { -1,3,3,-1 ,3435};
    /// <summary>
    /// 场景名称
    /// </summary>
    List<string> sceneNames = new List<string>() { "TeskScenes", "T1" ,"T2","T3","T4"};
    /// <summary>
    /// 场景配置
    /// </summary>
    ScenesRes re;
    /// <summary>
    /// 当前场景索引
    /// </summary>
    int nowIndex = -1;
    /// <summary>
    /// 当前已经完成的建筑数目
    /// </summary>
    int nowOk = 0;
    public void HideAllObj()
    {

    }
    public void Init()
    {
        re=new ScenesRes();
    }
    /// <summary>
    /// 开启全部物体
    /// </summary>
    public void StartToShow()
    {
        foreach (I_Obj obj in sceneObj)
        {
            obj.ShowMe();
        }
    }

    public void AddObj(I_Obj obj)
    {
        sceneObj.Add(obj);
    }

    public void CleanAll()
    {
        sceneObj.Clear();
        nowOk = 0;
    }

    public string GetName()
    {
        return sceneNames[nowIndex];
    }
    /// <summary>
    /// 获得选项数目
    /// </summary>
    /// <returns></returns>
    public int GetSceneInt()
    {
        return re.SceneIntDic[nowIndex].xxiang;
    }
    public SceneInt GetClass()
    {
        return re.SceneIntDic[nowIndex];
    }

    /// <summary>
    /// 判断是否可以继续(用于物品拖入是否全部成功）
    /// </summary>
    /// <param name="id">顺序判断id</param>
    /// <returns></returns>
    public bool IsOkorNo(int id,bool isTool)
    {
        //不等于-1才需要判断顺序
        if (serialNumber[nowIndex] != -1)
        {
            if (nowOk == id)
            {
                //Debug.Log("顺序id相符");
            }
            else if (id==-10)
            {
                //Debug.Log("顺序id虽然不相符，但前置相符");
            }
            else
            {
                return false;
            }
        }
        if(isTool)
        {
            return true;
        }
        //先加
        nowOk += 1;
        SceneChangeMgr.Instance.ZZZZZZZZZ();
        if (nowOk == re.SceneIntDic[nowIndex].wuNum)
        {
            //Debug.Log("执行下一步逻辑");
            foreach (I_Obj a in sceneObj)
            {
                a.HideMe();
            }
            //InputControlMgr.Instance.JueSHu();
            UIMgr.Instance.ShowOneUI<Massage3Panel>();
            return true;
        }
        return true;
    }
    public bool isGame;
    //bool isStart;
    public void End()
    {
        nowIndex = -1;
    }
    public obj teshu;
    /// <summary>
    /// 开启场景切换
    /// </summary>
    public void ToChangeScence()
    {
        nowIndex++;
        //SceneChangeMgr.Instance.LoadSceneAsync(sceneNames[nowIndex]);
        if (nowIndex >= 5)
        {
            isGame = false;
            SceneChangeMgr.Instance.LoadSceneAsync("EndScenes", () =>
            {
                MusicMgr.Instance.StopBkMusic();
                MusicMgr.Instance.PlayBkMusic("BkMusic", (mu) =>
                {
                    //循环音乐
                    mu.loop = true;
                    MusicMgr.Instance.StartBkMusic();
                    //MusicMgr.Instance.PauseBKMusic();
                  
                });
            });
            UIMgr.Instance.ShowOneUI<AllEndPanel>();
        }
        else
        {

            int task = 0;
            MainCamera mainCamera = null;
            Player player = null;
            GameObject tobj = null;
            void ToStart()
            {
                if (task == 3)
                {
                    if (mainCamera == null)
                    {
                        Debug.LogError("请为场景主摄像机挂载MainCamera脚本");
                        return;
                    }
                    player = GameObject.Instantiate(tobj).GetComponent<Player>();
                    player.transform.position = new Vector3(-3, -1, -1);
                    InputControlMgr.Instance.BindPlayer(player);
                    InputControlMgr.Instance.SetMainCamera(mainCamera);
                    //InputControlMgr.Instance.InputOpenOrClose(true);
                    //UIMgr.Instance.HideOneUI<LoadingPanel>();

                    MusicMgr.Instance.StartBkMusic();
                }
            }
            UIMgr.Instance.HideOneUI<EndPanel>();
            //UIMgr.Instance.ShowOneUI<LoadingPanel>();
            UIMgr.Instance.HideOneUI<BeginPanel>();
         
            SceneChangeMgr.Instance.LoadSceneAsync(sceneNames[nowIndex], () =>
            {

                mainCamera = GameObject.Find("Main Camera").GetComponent<MainCamera>();

                task += 1;
                ToStart();
            });
            if (!isGame)
            {
                isGame = true;
                //主页面音乐资源停止
                MusicMgr.Instance.StopBkMusic();
                MusicMgr.Instance.PlayBkMusic("DrumLoop_1", (mu) =>
                {
                    //循环音乐
                    mu.loop = true;
                    MusicMgr.Instance.StartBkMusic();
                    //MusicMgr.Instance.StartBkMusic();
                    //MusicMgr.Instance.PauseBKMusic();
                    task += 1;
                    ToStart();
                });
            }
            else
            {
                task += 1;
                
            }
            
            //玩家加载
            AddresableMge.Instance.LoadAssetAsyncI<GameObject>("player", (obj) =>
            {
                tobj = obj.Result;

                task += 1;
                ToStart();
            });
        }
    }
}
