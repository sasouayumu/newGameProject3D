using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MoveController
{
    private GameObject player;
    private Transform playerTr;//Player�̍��W
    private Transform enemyTr;//�G�̍��W
    private Transform trans;//��]�p�̍��W
    //�����n���p��Player�̕ǂɓ����������W
    public Vector3 wallTouchPos;
    public Vector3 SetGetwallTouchPos { get { return wallTouchPos; } set { wallTouchPos = value; } }
    private Vector3 prevPos;//��]�p�̃v���C�x�[�g���W
    [SerializeField] float speed = 1f;//�ړ����x
    private float jump = 5f;//�W�����v�̋���
    private Rigidbody rbEnemy;//�G��Rigidbody���
    //�����蔻��p
    private bool floor = false;
    private bool wall = false;
    //Player�̕ǃL�b�N�̓����蔻��
    public  bool CheckWallKick;
    public bool GetSetwallKick { get { return CheckWallKick; } set { CheckWallKick = value; } }
    MousePlayerController mpc;
    // Start is called before the first frame update
    void Start()
    {
        //Player�̈ʒu��Enemy�̈ʒu���擾����
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
        //Player�������ꏊ�ɍs���AFloor��True�Ȃ�W�����v����
        if (Mathf.Ceil(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor)
        {
            rbEnemy.velocity = Vector3.up * jump;
            floor = false;
        }
        //Player����ɂ��ĕǂɂ������Ă���Ȃ�ǂ����
        else if (Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * jump * 0.5f;
            rbEnemy.angularVelocity = new Vector3(270f, enemyTr.localEulerAngles.y, enemyTr.localEulerAngles.z);
            //Debug.Log(enemyTr.localEulerAngles.y);
        }
        else if (Mathf.Floor(playerTr.transform.position.y) < Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            //�ǂɂ������Ă���Player�Ɠ��������ɂ���Ȃ�Player�̕����֔��
            transform.position =
                 Vector3.MoveTowards(transform.position,
                 new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
                 speed * Time.deltaTime * 2);
        }
        else if (GetSetwallKick && transform.position != SetGetwallTouchPos)
        {
            //Player���ǃL�b�N���Ă���Ԃ�Player���ǂɓ��������ꏊ�֐i�ނ悤�ɂ���
            transform.position =
               Vector3.MoveTowards(transform.position, SetGetwallTouchPos, speed * Time.deltaTime);
        }
        else// if(Mathf.Floor(playerTr.transform.position.y) <= Mathf.Floor(enemyTr.transform.position.y))
        {
            //Player�̈ʒu���擾���Ă��̕����ɐi��
            rbEnemy.position =
            Vector3.MoveTowards(transform.position,
            new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
            speed * Time.deltaTime);
        }
        //Player�Ɠ��������������悤�ɂ��鏈��
        //���݃t���[���̃��[���h�ʒu
        var pos = trans.position;

        //�ړ��ʌv�Z
        var delta = pos - prevPos;

        //����Update�Ŏg�����߂̑O�t���[���ʒu�X�V
        prevPos = pos;

        //�Î~���Ă����Ԃ��ƁA�i�s���������ł��Ȃ����߉�]���Ȃ�
        if (delta == Vector3.zero)
            return;
        
        //�i�s�����Ɍ����悤�ɃN�H�[�^�j�I�����擾
        var rotation = Quaternion.LookRotation(delta, Vector3.up);

        //�I�u�W�F�N�g�ɔ��f
        trans.rotation = rotation;
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
        else
        {
            wall = false;
        }
    }
   
}
