using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MousePlayerController : MonoBehaviour
{
    private float moveSpeed = 8f;//移動速度
    private float inputV;
    private float jumpForce = 4.5f;//ジャンプ力
    private float jumpStand = 1;//ジャンプ台の当たり判定
    //ジャンプや走る処理の判定
    public  bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool wkey = false;
    private bool poleTouch = false;
    //壁の当たり判定
    private bool wallTouch;
    public bool wallTouchgs { get { return wallTouch; } set { wallTouch = false; } }
    //AkeyとDkeyで壁をのぼるための判定
    private bool aKey = true;
    private bool dKey = true;
    private bool upWall = false;
    private bool run = true;
    //ポールジャンプ用
    [SerializeField] private Transform poleTarget;
    [SerializeField] private float spinSpeed =4f;
    //PlayerのRigidbody
    private Rigidbody rbPlayer;
    private Animator animator;
    private EnemyController EnemyController;
    [SerializeField] private CameraController CameraController;
    //ダッシュゲージのスライダー
    [SerializeField] private Slider dushGaugeSlider;
    //Audio関係
    public AudioClip jampSE;
    public AudioClip runSE;
    public AudioClip upWallSE;
    private AudioSource audioSource;
    
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GameObject enemyObj = GameObject.Find("Enemy");
        EnemyController = enemyObj.GetComponent<EnemyController>();
        dushGaugeSlider.value = 3f;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //タイムスケールがゼロの場合は処理をしない
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        inputV = Input.GetAxisRaw("Vertical");

        //スペースでジャンプ
        if (Input.GetKey(KeyCode.Space) && jump)
        {
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0);//ジャンプのモーション
            //ジャンプ台に乗っているなら高くジャンプ
            rbPlayer.velocity = Vector3.up * jumpForce*jumpStand;
            
        }

        //壁に当たりながらAkeyとDkeyを交互に押すことで壁をのぼる
        if(Input.GetKeyDown(KeyCode.A)&&aKey&&wallTouch)
        {
            aKey = false;
            dKey = true;
            upWall = true;
        }

        if (Input.GetKeyDown(KeyCode.D) && dKey && wallTouch)
        {
            aKey =true; 
            dKey = false;
            upWall = true;
        }

        if (upWall)
        {
            audioSource.PlayOneShot(upWallSE);
            transform.position += new Vector3(0,0.5f,0);
            upWall = false;   
        }

        //壁キックの処理
        if (Input.GetKeyDown(KeyCode.W)&&wallTouch)
        {
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0);//ジャンプのモーション
            rbPlayer.velocity = Vector3.up * 7;
            CameraController.InversionCamera();

            //一度だけ敵に壁に当たった位置を送る
            if (!EnemyController.GetSetwallKick)
            {
                EnemyController.wallTouchPos = transform.position;
            }

            EnemyController.GetSetwallKick = true;
        }
        else
        {
            wkey = false;
        }
        //ポールジャンプの処理
        if (Input.GetKey(KeyCode.S)&&poleTouch)
        {
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0);//ジャンプのモーション
            transform.RotateAround(poleTarget.position,-transform.right,spinSpeed);
            rbPlayer.velocity = Vector3.up * jumpForce;
        }
        
        //右クリックで走るようにする（Playerの移動速度を上げる）空中では走るれないようにする
        if (Input.GetMouseButton(1) && dush && jump&&dushGaugeSlider.value > 0)
        {
            StopCoroutine("DushStop");
            moveSpeed = 15;
            
            if (coroutine)
            {
                audioSource.PlayOneShot(runSE);
                coroutine = false;
                StartCoroutine("DushCotroller");
            }
        }
        else
        {
            //一定時間走ったら速度をもとに戻す
            if (!coroutine)
            {
                dush = false;
                coroutine = true;
                
                StopCoroutine("DushCotroller");
                StartCoroutine("DushStop");
            }

            moveSpeed = 8;
        }
    }

    void FixedUpdate()
    {
        if (run)
        {
            //カメラの方向からX-Z平面の単位ベクトルを取得
            Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            //方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3  moveFoward = cameraFoward;

            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            rbPlayer.velocity = moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);

            //キャラクターの向きを進行方向に
            if (moveFoward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveFoward * Time.deltaTime);
            }
        }
    }
    
    //走るのをやめたら一定時間走れないようにする
    private IEnumerator DushCotroller()
    {
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        dush = false;
    }

    private IEnumerator DushStop()
    {
        audioSource.Stop();
        int gauge = 3-(int)dushGaugeSlider.value;

        for (int i = gauge; i >= 0; i--)
        {
            yield return new WaitForSeconds(1.0f);
            dushGaugeSlider.value += 1f;
            dush = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //二段ジャンプできないようにする（着地でジャンプできるようにする）
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("JumpStand"))
        {
            jump = true;
            aKey = true;
            dKey = true;
            upWall = false;
            animator.Play("Idle");//着地したら走るモーションに戻す
        }

        if (collision.gameObject.CompareTag("Step")||collision.gameObject.CompareTag("Wall"))
        {
            run = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallTouch = true;
            UpwardForce();
        }
        else
        {
            //Playerが地面にいる間はEnemyは壁にいる時の処理をしないようにする
            EnemyController.GetSetwallKick = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //離れたら戻す
        if (collision.gameObject.CompareTag("Wall"))
        {
            DownwardForce();
            wallTouch = false;
        }

        if (collision.gameObject.CompareTag("floor")||collision.gameObject.CompareTag("JumpStand"))
        {
            jump = false;
        }

        if (collision.gameObject.CompareTag("Step") || collision.gameObject.CompareTag("Wall"))
        {
            run = true;
        }
    }

    //ポールに当たった時の処理
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pole"))
        {
            poleTarget =other.transform;
           
            poleTouch = true ;
        }

        if (other.gameObject.CompareTag("JumpStand"))
        {
            jumpStand = 3;
        }
    }

    //離れたら戻す
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pole"))
        {
            poleTouch = false;
        }

        if (other.gameObject.CompareTag("JumpStand"))
        {
            jumpStand = 1;
        }
    }

    //壁に当たっている間、落ちる速度を落とす
    void UpwardForce()
    {
        //抵抗を増やす
        rbPlayer.drag = 1;
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
