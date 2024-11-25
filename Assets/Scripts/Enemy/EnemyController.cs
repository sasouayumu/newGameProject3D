using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MoveController
{
    Transform playerTr;
    Transform enemyTr;
    [SerializeField] float speed = 5;
    float jump = 5f;
    Rigidbody rbEnemy;
    private bool floor = false;
    private bool wall = false;
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
        //Playerの位置を取得してその方向に進む
        transform.position = 
            Vector3.MoveTowards(transform.position,
            new Vector3(playerTr.position.x,playerTr.position.y,playerTr.position.z),
            speed*Time.deltaTime);
       
        //Playerが高い場所に行き、FloorがTrueならジャンプする
        if(Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor)
        {
            rbEnemy.velocity = Vector3.up * jump;
            floor = false;
           
        }

        if (Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * 5 ;
            wall = false;

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //段差に当たったらジャンプできるようにする
        if (collision.gameObject.CompareTag("Stand"))
        {
            floor = true;
            
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            wall = true;
        }
    }
}
