using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEndPanel : BasePanel
{
    protected override void OnClickButton(string UIname)
    {
        base.OnClickButton(UIname);
        UIMgr.Instance.HideOneUI<AllEndPanel>();
        //SceneChangeMgr.Instance.LoadSceneAsync("BeginScenes");
        SceneResMgr.Instance.End();
        UIMgr.Instance.ShowOneUI<BeginPanel>();
        //MusicMgr.Instance.PlayBkMusic("BkMusic");
    }
}
