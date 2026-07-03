using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SceneImageCollllll : MonoBehaviour
{
    public GameObject[] imageOBJ;

    int nowIndex = 0;
    void Start()
    {
        
    }

    public void ShowImage()
    {
        if (nowIndex+1 > imageOBJ.Length)
        {
            print("正常吗？？");
            return;
        } 
        else
        {
            imageOBJ[nowIndex].gameObject.SetActive(true);
            nowIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
