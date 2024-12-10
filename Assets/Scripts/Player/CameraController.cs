using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject targetObj;//カメラが追従するオブジェクト
    private GameObject enemyObj;//Enemyオブジェクト
    private Vector3 targetPos;//追従するオブジェクトの場所
    private bool inversion = true; //カメラ反転
    //private bool wallKick = true;//壁キック用の判定
    //private MousePlayerController MousePlayer;//Playerのスクリプト取得用
    //private EnemyController EnemyController;//敵のスクリプト取得用

    // Start is called before the first frame update
    void Start()
    {
        //PlayerとEnemyの情報を取得
        targetObj = GameObject.Find("Player");
        //enemyObj = GameObject.Find("Enemy");
        targetPos = targetObj.transform.position;
        //MousePlayer = targetObj.GetComponent<MousePlayerController>();
        //EnemyController = enemyObj.GetComponent<EnemyController>();
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

        }//マウスホイールボタンを離したらカメラを戻す
        else if (Input.GetMouseButtonUp(2))
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
    }
    public void InversionCamera()
    {
        transform.RotateAround(targetPos, Vector3.up, 180);
    }
}
