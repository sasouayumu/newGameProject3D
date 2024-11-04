using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject targetObj;
    Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;


        //マウスの右クリックを押している間
        if (Input.GetMouseButton(0))
        {
            float mouseInputX = Input.GetAxis("Mouse X");


            transform.RotateAround(targetPos, Vector3.up, mouseInputX *50f);
        }
        

        

        
    }
}
