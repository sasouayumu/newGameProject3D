using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    private Transform playerTr;//Playerの座標
    private Transform enemyTr;//敵の座標
    private Transform trans;//回転用の座標
    //引き渡し用のPlayerの壁に当たった座標
    public Vector3 wallTouchPos;
    public Vector3 SetGetwallTouchPos { get { return wallTouchPos; } set { wallTouchPos = value; } }
    private Vector3 prevPos;//回転用のプライベート座標
    [SerializeField] float speed;//移動速度
    private float jump = 5f;//ジャンプの強さ
    private Rigidbody rbEnemy;//敵のRigidbody情報
    //当たり判定用
    private bool floor = false;
    private bool wall = false;
    private bool stepJamp = false;
    //Playerの壁キックの当たり判定
    public  bool CheckWallKick;
    public bool GetSetwallKick { get { return CheckWallKick; } set { CheckWallKick = value; } }
    MousePlayerController mpc;
    private int janpStand = 1;
    
    void Start()
    {
        //Playerの位置とEnemyの位置を取得する
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GameObject.FindGameObjectWithTag("Enemy").transform;
        rbEnemy = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        
        mpc = player.GetComponent<MousePlayerController>();
        trans = transform;
        prevPos = trans.position;
        
    }

    void FixedUpdate()
    {
        int upRotation = 0;//通常時はY軸を０に固定する
        int wallRotation_x = 0;//壁に登るときの角度
        
        //Playerが高い場所に行き、FloorがTrueならジャンプする
        if ((Mathf.Ceil(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor)||stepJamp)
        {
            rbEnemy.velocity = Vector3.up * jump* janpStand;
        }
        //Playerが上にいて壁にくっついているなら壁を登る
        else if (Mathf.Ceil(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * jump * 0.5f;
            upRotation = 270;//壁を登るときは上を向くようにする
            //向いている方向によって壁に向く方向を変える
            if ((trans.localEulerAngles.y >= 271f||trans.localEulerAngles.y >= -91f)&&trans.localEulerAngles.y <= 45f)
            {
                wallRotation_x = 0;
            }
            else if (trans.localEulerAngles.y >= 46f && trans.localEulerAngles.y <= 90f)
            {
                wallRotation_x = 90;
            }
            else if (trans.localEulerAngles.y >= 91f && trans.localEulerAngles.y <= 180f)
            {
                wallRotation_x = 180;
            }
            else if (trans.localEulerAngles.y >= 181f && (trans.localEulerAngles.y <= 270f||trans.localEulerAngles.y <=-90f))
            {
                wallRotation_x = 270;
            }
        }
        else if (Mathf.Floor(playerTr.transform.position.y) < Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            //壁にくっついていてPlayerと同じ高さにいるならPlayerの方向へ飛ぶ
            transform.position =
                 Vector3.MoveTowards(transform.position,
                 new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
                 speed * Time.deltaTime * 2);
            
        }
        else if (GetSetwallKick && transform.position != SetGetwallTouchPos&&!wall)
        {
            //Playerが壁キックしている間はPlayerが壁に当たった場所へ進むようにする
            transform.position =
               Vector3.MoveTowards(transform.position, SetGetwallTouchPos, speed * Time.deltaTime);
            
        }
        else if (Mathf.Floor(playerTr.transform.position.y-enemyTr.transform.position.y)>10f)
        {
            //Playerがとても高い場所にいった場合、高くジャンプするようにする
            rbEnemy.velocity = Vector3.up * 10;
        }
        else
        {
            //Playerの位置を取得してその方向に進む
            transform.position =
            Vector3.MoveTowards(transform.position,
            new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
            speed * Time.deltaTime);
        }
        //Playerの方向を向くようにする処理
        //現在フレームのワールド位置
        var pos = trans.position;

        //移動量計算
        var delta = pos - prevPos;

        //次のUpdateで使うための前フレーム位置更新
        prevPos = pos;

        //静止している状態だと、進行方向を特定できないため回転しない
        if (delta == Vector3.zero)
            return;
        
        //進行方向に向くようにクォータニオンを取得
        Quaternion rotation = Quaternion.LookRotation(new Vector3(delta.x, upRotation, delta.z), Vector3.up);
       
        if (wall)//壁に上るとき壁の方に向く
        {
            rotation = Quaternion.LookRotation(new Vector3(wallRotation_x, upRotation,0),Vector3.up);
        }else if (!floor)//ジャンプするときに上を向けるようにする
        {
            rotation = Quaternion.LookRotation(delta, Vector3.up);
        }

        //オブジェクトに反映
        trans.rotation = rotation;
    }

    private void OnCollisionStay(Collision collision)
    { 
        //段差に当たったらジャンプできるようにする
        if (collision.gameObject.CompareTag("floor"))
        {
            floor = true;
            wall = false;
        }

        if (collision.gameObject.CompareTag("JumpStand"))
        {
            janpStand = 2;
        }

        if (collision.gameObject.CompareTag("Step"))
        {
            stepJamp = true;
        }
        //壁の当たり判定
        if (collision.gameObject.CompareTag("Wall"))
        {
            wall = true;
        }
        else
        {
            wall = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //段差に当たったらジャンプできるようにする
        if (collision.gameObject.CompareTag("Step") || collision.gameObject.CompareTag("floor"))
        {
            floor = false;
        }


        if (collision.gameObject.CompareTag("Step"))
        {
            stepJamp = false;
        }

        if (collision.gameObject.CompareTag("JumpStand"))
        {
            janpStand = 1;
        }
    }

}
