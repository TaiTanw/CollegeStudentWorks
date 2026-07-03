using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Into1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIMgr.Instance.ShowOneUI<MassagePanel>(E_UILayer.Top, (a) =>
        {
            //a.SetTextShow("微派古村邀您重建经典微派居民");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        //
    }
}
