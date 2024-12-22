using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject targetObj;//カメラが追従するオブジェクト
    private GameObject enemyObj;//Enemyオブジェクト
    [SerializeField]
    private GameObject SecondCamera;
    private Vector3 targetPos;//追従するオブジェクトの場所
    private bool inversion = true; //カメラ反転
    

    // Start is called before the first frame update
    void Start()
    {
        //Playerの情報を取得
        targetObj = GameObject.Find("Player");
        targetPos = targetObj.transform.position;
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
            SecondCamera.gameObject.SetActive(true);

        }//マウスホイールボタンを離したらカメラを戻す
        else if (Input.GetMouseButtonUp(2))
        {
            inversion = true;
            SecondCamera.gameObject.SetActive(false);
        }

        float mouseInputX;
        //マウスの左クリックを押している間
        if (Input.GetMouseButton(0))
        {
            //マウスの移動分公転させる
            mouseInputX = Input.GetAxis("Mouse X");

            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 50f);
        }
    }
    public void InversionCamera()
    {
        transform.RotateAround(targetPos, Vector3.up, 180);
    }
}
