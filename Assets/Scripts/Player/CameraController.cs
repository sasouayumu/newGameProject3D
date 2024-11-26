using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject targetObj;
    Vector3 targetPos;
    private bool inversion = true; //カメラ反転
    MousePlayerController MousePlayer;
    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
        MousePlayer = targetObj.GetComponent<MousePlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットの移動分、移動する
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        //マウスホイールボタンを押している間カメラを反対に移動する
        if (Input.GetMouseButtonDown(2) && inversion)
        {
            //Debug.Log(MousePlayer.wallTouchgs);
            inversion = false;
            transform.RotateAround(targetPos, Vector3.up, 180);
            
        }
        
        //マウスホイールボタンを離したらカメラを戻す
        if (Input.GetMouseButtonUp(2))
        {
            inversion = true;
            transform.RotateAround(targetPos, Vector3.up, 180);
        }

        float mouseInputX;
        //マウスの左クリックを押している間
        if (Input.GetMouseButton(0))
        {
            //マウスの移動分公転させる
            mouseInputX = Input.GetAxis("Mouse X");

            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 50f);
        }

        //壁キックの判定　壁に当たりながら"W"Keyを押すとカメラを反転させ反対方向に飛ぶ
        if (Input.GetKey("w") && MousePlayer.wallTouchgs)
        {
            MousePlayer.rbWallKick.velocity = - 1 * Vector3.right +Vector3.up * 7;
            transform.RotateAround(targetPos, Vector3.up, 180);

            
        }
    }
}
