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
        //Player�̈ʒu��Enemy�̈ʒu���擾����
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GameObject.FindGameObjectWithTag("Enemy").transform;
        rbEnemy = GetComponent<Rigidbody>();
        GameObject player = GameObject.Find("Player");
        mpc = player.GetComponent<MousePlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
        //Player�������ꏊ�ɍs���AFloor��True�Ȃ�W�����v����
        if(Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor)
        {
            rbEnemy.velocity = Vector3.up * jump;
            floor = false;
           
        }

        //Player����ɂ��ĕǂɂ������Ă���Ȃ�ǂ����
        if (Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * jump*0.25f;
            wall = false;

        }else if(Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            //�ǂɂ������Ă���Player�Ɠ��������ɂ���Ȃ甽�Α��փW�����v����
            rbEnemy.velocity = Vector3.right*10;
        }
        else if (GetSetwallKick)
        {
            //Player���ǃL�b�N���Ă���Ԃ͕ʂ̕��@�ŏꏊ���擾
            transform.position =
               Vector3.MoveTowards(transform.position,SetGetwallTouchPos,speed * Time.deltaTime);
        }
        else 
        {
            //Player�̈ʒu���擾���Ă��̕����ɐi��
            transform.position =
                Vector3.MoveTowards(transform.position,
                new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
                speed * Time.deltaTime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //�i���ɓ���������W�����v�ł���悤�ɂ���
        if (collision.gameObject.CompareTag("Stand")|| collision.gameObject.CompareTag("floor"))
        {
            floor = true;
            
        }

        //�ǂ̓����蔻��
        if (collision.gameObject.CompareTag("Wall"))
        {
            wall = true;
        }
    }
}
