using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Vector3 toPoint;
    Vector3 rV=new Vector3(0,2,0);
    float followSpeed = 15f;
    void Start()
    {
        
    }
    public void SetPoint(Vector3 vector3)
    {
        toPoint = new Vector3(vector3.x, vector3.y, -10);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, toPoint+rV, followSpeed*Time.deltaTime);
    }
}
