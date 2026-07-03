using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        UIMgr.Instance.ShowOneUI<BeginPanel>();
        MusicMgr.Instance.PlayBkMusic("BkMusic");
        MusicMgr.Instance.SoundPoolStart(5);
        GameObject.DontDestroyOnLoad(this.gameObject);
        //UIMgr.Instance.ShowOneUI<MiniGamePanel>(E_UILayer.System);
        
    }
    private void Awake()
    {
        DataAndInitMgr.Instance.Init();
        UIMgr.Instance.Init();
        MusicMgr.Instance.Init();

        InputControlMgr.Instance.Init();
        //缓存加载界面
        UIMgr.Instance.ShowOneUI<LoadingPanel>(E_UILayer.Top, (UI) =>
        {
            UI.gameObject.SetActive(false);
        });

        //关卡管理器初始化
        SceneResMgr.Instance.Init();

        //print(Application.persistentDataPath);
    }
}
