using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MoveController
{
    Transform playerTr;
    Transform enemyTr;
    public Vector3 wallTouchPos;
    public Vector3 SetGetwallTouchPos { get { return wallTouchPos; } set { wallTouchPos = value; } }
    [SerializeField] float speed = 4;
    float jump = 5f;
    Rigidbody rbEnemy;
    private bool floor = false;
    private bool wall = false;
    private bool CheckWallKick;
    public bool GetSetwallKick { get { return CheckWallKick; } set { CheckWallKick = value; } }
    MousePlayerController mpc;
    private bool canJump;
    // Start is called before the first frame update
    void Start()
    {
        //Playerの位置とEnemyの位置を取得する
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GameObject.FindGameObjectWithTag("Enemy").transform;
        rbEnemy = GetComponent<Rigidbody>();
        GameObject player = GameObject.Find("Player");
        mpc = player.GetComponent<MousePlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
        //Playerが高い場所に行き、FloorがTrueならジャンプする
        if(Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor)
        {
            rbEnemy.velocity = Vector3.up * jump;
            floor = false;
           
        }

        //Playerが上にいて壁にくっついているなら壁を上る
        if (Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * jump*0.25f;
            wall = false;

        }else if(Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            //壁にくっついていてPlayerと同じ高さにいるなら反対側へジャンプする
            rbEnemy.velocity = Vector3.right*10;
        }
        else if (GetSetwallKick)
        {
            //Playerが壁キックしている間は別の方法で場所を取得
            transform.position =
               Vector3.MoveTowards(transform.position,SetGetwallTouchPos,speed * Time.deltaTime);
        }
        else 
        {
            //Playerの位置を取得してその方向に進む
            transform.position =
                Vector3.MoveTowards(transform.position,
                new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
                speed * Time.deltaTime);
        }
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
    }
}
