using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


//プレイヤー関連のクラス
public class MousePlayerController : MonoBehaviour
{
    //Audio関係
    [SerializeField] private AudioClip jampSE;
    [SerializeField] private AudioClip runSE;
    [SerializeField] private AudioClip upWallSE;
    private AudioSource audioSource;

    [SerializeField] private CameraController cameraController;

    //ポールジャンプ用
    [SerializeField] private Transform poleTarget;
    [SerializeField] private float spinSpeed = 4f;

    //ダッシュゲージのスライダー
    [SerializeField] private Slider dushGaugeSlider;

    [SerializeField]private float upSpeed = 8f;　　　 //ダッシュ時のスピード
    [SerializeField] private float usuallySpeed = 5f; //通常時のスピード
    [SerializeField] private float jumpForce = 4.5f;  //ジャンプ力
    private float inputV;
    private float moveSpeed;                        　//移動速度
    private float jumpStand = 1f;                   　//ジャンプ台の倍率

    //ジャンプや走る処理の判定
    private bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool wKey = false;
    private bool poleTouch = false;
   
    //AkeyとDkeyで壁をのぼるための判定
    private bool aKey = true;
    private bool dKey = true;
    private bool upWall = false;
    private bool run = true;

    //PlayerのRigidbody
    private Rigidbody rbPlayer;
    private Animator animator;
    private EnemyController enemyController;

    //壁の当たり判定
    private bool wallTouch;


    void Start()
    {
        moveSpeed = usuallySpeed;
        rbPlayer = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GameObject enemyObj = GameObject.Find("Enemy");
        enemyController = enemyObj.GetComponent<EnemyController>();
        dushGaugeSlider.value = 3f;
        audioSource = GetComponent<AudioSource>();
    }


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
            animator.Play("Jump", 0, 0); //ジャンプのモーション

            //ジャンプ台に乗っているなら高くジャンプ
            rbPlayer.velocity = Vector3.up * jumpForce * jumpStand;
        }

        //壁に当たりながらAkeyとDkeyを交互に押すことで壁をのぼる
        if(Input.GetKeyDown(KeyCode.A) && aKey && wallTouch)　
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

        //壁をのぼる
        if (upWall)
        {
            audioSource.PlayOneShot(upWallSE);
            rbPlayer.velocity = Vector3.up*4;
            upWall = false;   
        }

        //壁キックの処理
        if (Input.GetKeyDown(KeyCode.W)&&wallTouch)
        {
            run = true;
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0); 　　　 //ジャンプのモーション
            rbPlayer.velocity = Vector3.up * 8; //反対側にジャンプする
            cameraController.InversionCamera(); //カメラを反転する

            //一度だけ敵に壁に当たった位置を送る
            if (!enemyController.GetSetwallKick)
            {
                enemyController.GetSetwallTouchPos = transform.position;
            }

            enemyController.GetSetwallKick = true;
        }
        else
        {
            wKey = false;
        }

        //ポールジャンプの処理
        if (Input.GetKeyDown(KeyCode.Space)&&poleTouch)
        {
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0); //ジャンプのモーション
            transform.RotateAround(poleTarget.position,-transform.right,spinSpeed);
            rbPlayer.velocity = Vector3.up * jumpForce*1.6f;
        }
        
        //右クリックで走るようにする（Playerの移動速度を上げる）空中では走るれないようにする
        if (Input.GetMouseButton(1) && dush && jump&&dushGaugeSlider.value > 0)
        {
            StopCoroutine("DushStop");
            
            if (coroutine)
            {
                moveSpeed = upSpeed;
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
                moveSpeed = usuallySpeed;
                dush = false;
                coroutine = true;
                
                StopCoroutine("DushCotroller");
                StartCoroutine("DushStop");
            }
        }
    }


    void FixedUpdate()
    {
        //カメラの方向からX-Z平面の単位ベクトルを取得
        Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveFoward = cameraFoward;

        RaycastHit hit;
        Vector3 runForwardR = transform.forward + new Vector3(0.7f, 0, 0);　//右方向のRayの角度
        Vector3 runForwardL = transform.forward - new Vector3(0.7f, 0, 0);  //左方向のRayの角度
       
        //Rayを照射
        if (Physics.Raycast(transform.position + (Vector3.up / 8), transform.forward, out hit, 0.3f)
            || Physics.Raycast(transform.position + (Vector3.up / 8), runForwardR, out hit, 0.3f)
            || Physics.Raycast(transform.position + (Vector3.up / 8), runForwardL, out hit, 0.3f)
            || Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 0.3f)
            || Physics.Raycast(transform.position + Vector3.up, runForwardR, out hit, 0.3f)
            || Physics.Raycast(transform.position + Vector3.up, runForwardL, out hit, 0.3f))
        {
            //hitしたTagがWallまたはStepならPlayerに移動させないようにする
            if (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("Step"))
            {
                run = false;
            }
            else
            {
                run = true;
            }
        }
        else
        {
            run = true;
        }

        if (run)
        {
            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            rbPlayer.velocity = moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);
        }
        
        //キャラクターの向きを進行方向にする
        if (moveFoward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveFoward * Time.deltaTime);
        }
    }
    

    //ダッシュ中のゲージを減らす処理
    private IEnumerator DushCotroller()
    {
        //ダッシュ中は一秒ずつゲージを減らす
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;

        dush = false; //ダッシュできないようにする
    }


    //ダッシュ終了した後のゲージ回復の処理
    private IEnumerator DushStop()
    {
        audioSource.Stop();
        int gauge = 3-(int)dushGaugeSlider.value;　//減った分のゲージを計算

        //減った分少しずつ回復させる
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
        if (collision.gameObject.CompareTag("floor"))
        {
            //ジャンプをできるようにする
            jump = true;

            animator.Play("Idle");//着地したら走るモーションに戻す

            //Playerが地面にいる間はEnemyは壁にいる時の処理をしないようにする
            enemyController.GetSetwallKick = false;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        //離れたらジャンプをできないようにする
        if (collision.gameObject.CompareTag("floor"))
        {
            jump = false;
        }
    }

  
    private void OnTriggerEnter(Collider other)
    {
        //ポールに当たった時の処理
        if (other.gameObject.CompareTag("Pole"))
        {
            poleTarget =other.transform;
           
            poleTouch = true ;
            run = false;
        }

        //ジャンプ台に当たった時の処理
        if (other.gameObject.CompareTag("JumpStand"))
        {
            jumpStand = 2.5f;
        }

        //壁に当たった時の処理
        if (other.gameObject.CompareTag("Wall"))
        {
            wallTouch = true;
        }
    }


    //離れたら戻す
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pole"))
        {
            poleTouch = false;
            run = true;
        }

        if (other.gameObject.CompareTag("JumpStand"))
        {
            jumpStand = 1;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            //壁登りする時の判定を戻す
            aKey = true;
            dKey = true;
            upWall = false;

            wallTouch = false;
        }
    }


    //アニメーションイベント用
    private void OnCallChangeFace(){}
}
