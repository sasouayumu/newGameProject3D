using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject targetObj;
    Vector3 targetPos;
    private bool inversion = true;
    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットの移動分、移動する
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        float mouseInputX;
        //マウスの右クリックを押している間
        if (Input.GetMouseButton(0))
        {
            //マウスの移動分公転させる
            mouseInputX = Input.GetAxis("Mouse X");

            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 50f);
        }

        if (Input.GetMouseButton(2)&& inversion)
        {
            inversion = false;
            transform.RotateAround(targetPos, Vector3.up, 180);
        }
        else if (!Input.GetMouseButton(2) && !inversion)
        {
            inversion = true;
        }
    }
}
