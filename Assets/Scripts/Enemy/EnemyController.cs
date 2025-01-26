using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//敵の行動の処理をするクラス
public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed; //移動速度
    private GameObject player;
    private Transform playerTr; 　//Playerの座標
    private Transform enemyTr;  　//敵の座標
    private Transform trans;   　 //回転用の座標

    private int jampStand = 1;　　//ジャンプ台のジャンプの倍率

    private Vector3 prevPos;      //回転用のプライベート座標
    
    private float jump = 5f;      //ジャンプの強さ
    private Rigidbody rbEnemy;    //敵のRigidbody情報

    //当たり判定用
    private bool floor = false;
    private bool wall = false;
    private bool stepJamp = false;

    //Playerの壁キックの当たり判定
    private  bool CheckWallKick;
    public bool GetSetwallKick { get { return CheckWallKick; } set { CheckWallKick = value; } }
    
    //引き渡し用のPlayerの壁に当たった座標
    private Vector3 wallTouchPos;
    public Vector3 GetSetwallTouchPos { get { return wallTouchPos; } set { wallTouchPos = value; } }


    void Start()
    {
        //Playerの位置とEnemyの位置を取得する
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GameObject.FindGameObjectWithTag("Enemy").transform;
        rbEnemy = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        trans = transform;
        prevPos = trans.position;
    }


    void FixedUpdate()
    {
        int upRotation = 0;       //通常時はY軸を０に固定する
        float wallRotation_x = 0; //壁に登るときの角度

        Debug.DrawRay(transform.position, (transform.forward + new Vector3(45, 0, 45)) * 0.25f);
        Debug.DrawRay(transform.position, (transform.forward + new Vector3(-45, 0, 45)) * 0.25f);
        Debug.DrawRay(transform.position, transform.forward, UnityEngine.Color.red);

        //Playerが高い場所に行き、FloorがTrueならジャンプする
        if ((Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor) || stepJamp)
        {
            rbEnemy.velocity = Vector3.up * jump * jampStand;
        }
        //Playerが上にいて壁にくっついているなら壁を登る
        else if (Mathf.Ceil(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * jump * 0.25f;

            upRotation = 270;//壁を登るときは上を向くようにする

            RaycastHit hit;

            //壁に垂直に向くための処理
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f)
                || Physics.Raycast(transform.position, transform.forward + new Vector3(-45, 0, 45), out hit, 0.25f)
                || Physics.Raycast(transform.position, transform.forward + new Vector3(45, 0, -45), out hit, 0.25f))
            {
                Quaternion rota = Quaternion.LookRotation(-hit.normal);

                wallRotation_x = rota.x;
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
        else if (GetSetwallKick && transform.position != GetSetwallTouchPos && !wall)
        {
            //Playerが壁キックしている間はPlayerが壁に当たった場所へ進むようにする
            transform.position =
               Vector3.MoveTowards(transform.position, GetSetwallTouchPos, speed * Time.deltaTime);
        }
        else if (Mathf.Floor(playerTr.transform.position.y - enemyTr.transform.position.y) > 15f)
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
            rotation = Quaternion.LookRotation(new Vector3(wallRotation_x, upRotation, 0), Vector3.up);
        }
        else if (!floor)//ジャンプするときに上を向けるようにする
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
            jampStand = 2;
        }

        if (collision.gameObject.CompareTag("Step"))
        {
            stepJamp = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            wall = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            wall = false;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
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
            jampStand = 1;
        }
    }
}
