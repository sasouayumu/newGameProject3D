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
    private float moveSpeed = 5f;//移動速度
    //private float inputH;
    private float inputV;
    private float jumpForce = 5f;//ジャンプ力
    //[SerializeField] private float wallKickHS = 3f;
    //[SerializeField] private float wallKickVS = 3f;
    //[SerializeField] private float maxStickWallKickFS = 1f;
    //ジャンプや走る処理の判定
    public  bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool coroutine2 = true;
    private bool move = true;
    private bool key = false;
    //壁の当たり判定
    private bool wallTouch;
    public bool wallTouchgs { get { return wallTouch; } set { wallTouch = false; } }
    //Player移動用の座標
    private Vector3 velocity;
    //PlayerのRigidbody
    private　Rigidbody rbPlayer;
    public Rigidbody rbWallKick { get { return rbPlayer; } set{ rbPlayer = GetComponent<Rigidbody>(); } }//引き渡し用
    //public Vector3 NomalOfStickingWall { get; private set; } = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
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

        //マウスホイールを押すと背後を見れる
        if (Input.GetMouseButton(2) && move)
        {
            move = false;
            moveInversion *= -1;
        }
        //離れると戻る
        if (!Input.GetMouseButton(2) && !move)
        {
            move = true;
            moveInversion *= -1;

        }

        //方向キーの入力値とカメラの向きから、移動方向を決定
        //+Camera.main.transform.right * inputV
        Vector3 moveFoward = cameraFoward;

        //右クリックで走るようにする（Playerの移動速度を上げる）空中ではジャンプできないようにする
        if (Input.GetMouseButton(1) && dush && jump)
        {
            moveSpeed = 10f;
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
            
            moveSpeed = 5f;
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
            //yPos = rbPlayer.velocity.y;
            rbPlayer.velocity = Vector3.up * jumpForce;
        }
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

        //壁に当たっている間は落下速度を落とす
        if (collision.gameObject.CompareTag("Wall"))
        {
            UpwardForce();
            wallTouch = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //離れたら戻す
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallTouch = false;
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
}
