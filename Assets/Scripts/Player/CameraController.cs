using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


//プレイヤーに追従するカメラの管理するクラス
public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject secondCamera; //プレイヤーの後ろを見るためのカメラ
    private GameObject targetObj;                     //カメラが追従するオブジェクト
    private Vector3 targetPos;                        //追従するオブジェクトの場所
    private bool inversion = true;                    //カメラ反転

    //マウス感度の調整
    private float mouseSensitivity = 5;
    public float GetSetMouseSensitivity { set { mouseSensitivity = value; } }

    string sceneName;
   

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "SettingScene") 
        {
            //Playerの情報を取得
            targetObj = GameObject.Find("Player");
            targetPos = targetObj.transform.position;
            
        }
    }


    void Update()
    {
        //タイムスケールがゼロの場合は処理しない
        if (Mathf.Approximately(Time.timeScale, 0f)|| sceneName == "SettingScene")
        {
            return;
        }

        //ターゲットの移動分、移動する
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        //マウスホイールボタンを押している間プレイヤーの前にあるカメラを起動する
        if (Input.GetMouseButtonDown(2) && inversion)
        {
            inversion = false;
            secondCamera.gameObject.SetActive(true);

        }//マウスホイールボタンを離したらカメラを戻す
        else if (Input.GetMouseButtonUp(2))
        {
            inversion = true;
            secondCamera.gameObject.SetActive(false);
        }

        float mouseInputX;

        if (Input.GetMouseButton(0))//マウスの左クリックを押している間
        {
            //マウスの移動分公転させる
            mouseInputX = Input.GetAxis("Mouse X");

            transform.RotateAround(targetPos, Vector3.up, mouseInputX * 2f*mouseSensitivity);
        }
    }


    //カメラを反転させる処理
    public void InversionCamera()
    {
        transform.RotateAround(targetPos, Vector3.up, 180);
    }
}
