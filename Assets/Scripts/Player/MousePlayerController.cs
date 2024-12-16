using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MousePlayerController : MoveController
{
    private int moveInversion = 1;//後ろを向くときに移動方向をそのままにする
    private float moveSpeed = 7f;//移動速度
    private float inputV;
    private float jumpForce = 6f;//ジャンプ力
    //ジャンプや走る処理の判定
    public  bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool coroutine2 = true;
    private bool move = true;
    private bool key = false;
    private bool wkey = false;
    //壁の当たり判定
    private bool wallTouch;
    public bool wallTouchgs { get { return wallTouch; } set { wallTouch = false; } }
    //Player移動用の座標
    private Vector3 velocity;
    //PlayerのRigidbody
    private　Rigidbody rbPlayer;
    //public Rigidbody rbWallKick { get { return rbPlayer; } set{ rbPlayer = GetComponent<Rigidbody>(); } }//引き渡し用
    private Animator animator;
    private EnemyController EnemyController;
    public CameraController CameraController;
    
    
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GameObject enemyObj = GameObject.Find("Enemy");
        EnemyController = enemyObj.GetComponent<EnemyController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        inputV = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //カメラの方向からX-Z平面の単位ベクトルを取得
        Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        if (Input.GetKeyDown("w"))
        {
            StartCoroutine("WkeyDown");
        }
        
        //方向キーの入力値とカメラの向きから、移動方向を決定
        //+Camera.main.transform.right * inputV
        Vector3 moveFoward = cameraFoward;

        //右クリックで走るようにする（Playerの移動速度を上げる）空中ではジャンプできないようにする
        if (Input.GetMouseButton(1) && dush && jump)
        {
            moveSpeed = 12;
            //GetComponent<Renderer>().material.color = UnityEngine.Color.blue;
            if (coroutine)
            {
                coroutine = false;
                StartCoroutine("DushCotroller");
            }
        }
        else
        {
            //一定時間走ったら速度をもとに戻す
            //GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            if (!coroutine&&coroutine2)
            {
                dush = false;
                coroutine = true;
                coroutine2 = false;
                
                StopCoroutine("DushCotroller");
                StartCoroutine("DushStop");
            }
            
            moveSpeed = 7;
        }
       
        //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rbPlayer.velocity = moveInversion * moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);

        //キャラクターの向きを進行方向に
        if (moveFoward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveFoward);
        }
        //}

        //スペースでジャンプ
        if (Input.GetKey(KeyCode.Space) && jump)
        {
            animator.Play("Jump",0,0);//ジャンプのモーション
            rbPlayer.velocity = Vector3.up * jumpForce;
        }
    }
    private IEnumerator WkeyDown()
    {
        wkey = true;
        yield return new WaitForSeconds(0.1f);
        wkey = false;
    }
    //走るのをやめたら一定時間走れないようにする
    private IEnumerator DushCotroller()
    {
        yield return new WaitForSeconds(3.0f);
        dush = false;
        
        StartCoroutine("DushStop");
    }

    private IEnumerator DushStop()
    {
        yield return new WaitForSeconds(3.0f);
        
        coroutine2 = true;
        dush = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        //二段ジャンプできないようにする（着地でジャンプできるようにする）
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("Stand"))
        {
            jump = true;
            animator.Play("Idle");//着地したら走るモーションに戻す
        }

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        //カギの入手
        if (collision.gameObject.CompareTag("Key"))
        {
            key = true;
        }

        //カギを持っていたらゴールできるようにする
        if (collision.gameObject.CompareTag("Goal") && key)
        {
            SceneManager.LoadScene(2);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            UpwardForce();
            if (wkey)
            {
                animator.Play("Jump", 0, 0);//ジャンプのモーション
                rbPlayer.velocity = Vector3.up * 10;
                CameraController.InversionCamera();
                //一度だけ敵に壁に当たった位置を送る
                if (!EnemyController.GetSetwallKick)
                {
                    EnemyController.wallTouchPos = transform.position;
                }
                EnemyController.GetSetwallKick = true;
                wkey = false;
            }
        }
        else
        {
            //Playerが地面にいる間はEnemyは壁キックする処理をしないようにする
            EnemyController.GetSetwallKick = false;
        }
       
    }

    private void OnCollisionExit(Collision collision)
    {
        //離れたら戻す
        if (collision.gameObject.CompareTag("Wall"))
        {
            DownwardForce();
        }

        if (collision.gameObject.CompareTag("floor")|| collision.gameObject.CompareTag("Stand"))
        {
            jump = false;
        }
    }

    //壁に当たっている間、落ちる速度を落とす
    void UpwardForce()
    {
        //抵抗を増やす
        rbPlayer.drag = 10;
    }

    void DownwardForce()
    {
        //抵抗を戻す
        rbPlayer.drag = 0;
    }

    void OnCallChangeFace()
    {

    }
}
