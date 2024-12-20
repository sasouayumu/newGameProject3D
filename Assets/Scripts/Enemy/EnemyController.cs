using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MoveController
{
    private GameObject player;
    private Transform playerTr;//Playerの座標
    private Transform enemyTr;//敵の座標
    private Transform trans;//回転用の座標
    //引き渡し用のPlayerの壁に当たった座標
    public Vector3 wallTouchPos;
    public Vector3 SetGetwallTouchPos { get { return wallTouchPos; } set { wallTouchPos = value; } }
    private Vector3 prevPos;//回転用のプライベート座標
    [SerializeField] float speed = 1f;//移動速度
    private float jump = 5f;//ジャンプの強さ
    private Rigidbody rbEnemy;//敵のRigidbody情報
    //当たり判定用
    private bool floor = false;
    private bool wall = false;
    //Playerの壁キックの当たり判定
    public  bool CheckWallKick;
    public bool GetSetwallKick { get { return CheckWallKick; } set { CheckWallKick = value; } }
    MousePlayerController mpc;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void FixedUpdate()
    {
        //Playerが高い場所に行き、FloorがTrueならジャンプする
        if (Mathf.Ceil(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor)
        {
            rbEnemy.velocity = Vector3.up * jump;
            floor = false;
        }
        //Playerが上にいて壁にくっついているなら壁を上る
        else if (Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * jump * 0.5f;
            rbEnemy.angularVelocity = new Vector3(270f, enemyTr.localEulerAngles.y, enemyTr.localEulerAngles.z);
            //Debug.Log(enemyTr.localEulerAngles.y);
        }
        else if (Mathf.Floor(playerTr.transform.position.y) < Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            //壁にくっついていてPlayerと同じ高さにいるならPlayerの方向へ飛ぶ
            transform.position =
                 Vector3.MoveTowards(transform.position,
                 new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
                 speed * Time.deltaTime * 2);
        }
        else if (GetSetwallKick && transform.position != SetGetwallTouchPos)
        {
            //Playerが壁キックしている間はPlayerが壁に当たった場所へ進むようにする
            transform.position =
               Vector3.MoveTowards(transform.position, SetGetwallTouchPos, speed * Time.deltaTime);
        }
        else// if(Mathf.Floor(playerTr.transform.position.y) <= Mathf.Floor(enemyTr.transform.position.y))
        {
            //Playerの位置を取得してその方向に進む
            rbEnemy.position =
            Vector3.MoveTowards(transform.position,
            new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
            speed * Time.deltaTime);
        }
        //Playerと同じ方向を向くようにする処理
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
        var rotation = Quaternion.LookRotation(delta, Vector3.up);

        //オブジェクトに反映
        trans.rotation = rotation;
    }

    private void OnCollisionStay(Collision collision)
    {
        //段差に当たったらジャンプできるようにする
        if (collision.gameObject.CompareTag("Stand")|| collision.gameObject.CompareTag("floor"))
        {
            floor = true;
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
   
}
