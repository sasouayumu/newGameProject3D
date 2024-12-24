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
    [SerializeField] float speed;//�ړ����x
    private float jump = 5f;//�W�����v�̋���
    private Rigidbody rbEnemy;//�G��Rigidbody���
    //�����蔻��p
    private bool floor = false;
    private bool wall = false;
    //Player�̕ǃL�b�N�̓����蔻��
    public  bool CheckWallKick;
    public bool GetSetwallKick { get { return CheckWallKick; } set { CheckWallKick = value; } }
    MousePlayerController mpc;
    
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

    void FixedUpdate()
    {
        int upRotation = 0;//�ʏ펞��Y�����O�ɌŒ肷��
        int wallRotation_x = 0;//�ǂɓo��Ƃ��̊p�x
        
        //Player�������ꏊ�ɍs���AFloor��True�Ȃ�W�����v����
        if (Mathf.Ceil(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor)
        {
            rbEnemy.velocity = Vector3.up * jump;
        }
        //Player����ɂ��ĕǂɂ������Ă���Ȃ�ǂ�o��
        else if (Mathf.Ceil(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * jump * 0.5f;
            upRotation = 270;//�ǂ�o��Ƃ��͏�������悤�ɂ���
            //�����Ă�������ɂ���ĕǂɌ���������ς���
            if ((trans.localEulerAngles.y >= 271f||trans.localEulerAngles.y >= -91f)&&trans.localEulerAngles.y <= 45f)
            {
                wallRotation_x = 0;
               //Debug.Log("mae");
            }
            else if (trans.localEulerAngles.y >= 46f && trans.localEulerAngles.y <= 90f)
            {
                wallRotation_x = 90;
                //Debug.Log("migi");
            }
            else if (trans.localEulerAngles.y >= 91f && trans.localEulerAngles.y <= 180f)
            {
                wallRotation_x = 180;
                //Debug.Log("usiro");
            }
            else if (trans.localEulerAngles.y >= 181f && (trans.localEulerAngles.y <= 270f||trans.localEulerAngles.y <=-90f))
            {
                wallRotation_x = 270;
                //Debug.Log("hidari");
            }
        }
        else if (Mathf.Floor(playerTr.transform.position.y) < Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            //�ǂɂ������Ă���Player�Ɠ��������ɂ���Ȃ�Player�̕����֔��
            transform.position =
                 Vector3.MoveTowards(transform.position,
                 new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
                 speed * Time.deltaTime * 2);
            
        }
        else if (GetSetwallKick && transform.position != SetGetwallTouchPos&&!wall)
        {
            //Player���ǃL�b�N���Ă���Ԃ�Player���ǂɓ��������ꏊ�֐i�ނ悤�ɂ���
            transform.position =
               Vector3.MoveTowards(transform.position, SetGetwallTouchPos, speed * Time.deltaTime);
            
        }
        else
        {
            //Player�̈ʒu���擾���Ă��̕����ɐi��
            transform.position =
            Vector3.MoveTowards(transform.position,
            new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
            speed * Time.deltaTime);
        }
        //Player�̕����������悤�ɂ��鏈��
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
        Quaternion rotation = Quaternion.LookRotation(new Vector3(delta.x, upRotation, delta.z), Vector3.up);

        if (wall)//�ǂɏ��Ƃ��ǂ̕��Ɍ���
        {
            rotation = Quaternion.LookRotation(new Vector3(wallRotation_x, upRotation,0),Vector3.up);
        }else if (!floor)//�W�����v����Ƃ��ɏ��������悤�ɂ���
        {
            rotation = Quaternion.LookRotation(delta, Vector3.up);
        }

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
    private void OnCollisionExit(Collision collision)
    {
        //�i���ɓ���������W�����v�ł���悤�ɂ���
        if (collision.gameObject.CompareTag("Stand") || collision.gameObject.CompareTag("floor"))
        {
            floor = false;
        }
    }

}
