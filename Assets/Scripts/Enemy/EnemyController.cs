using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//�G�̍s���̏���������N���X
public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed; //�ړ����x
    private GameObject player;
    private Transform playerTr; �@//Player�̍��W
    private Transform enemyTr;  �@//�G�̍��W
    private Transform trans;   �@ //��]�p�̍��W

    private int jampStand = 1;�@�@//�W�����v��̃W�����v�̔{��

    private Vector3 prevPos;      //��]�p�̃v���C�x�[�g���W
    
    private float jump = 5f;      //�W�����v�̋���
    private Rigidbody rbEnemy;    //�G��Rigidbody���

    //�����蔻��p
    private bool floor = false;
    private bool wall = false;
    private bool stepJamp = false;

    //Player�̕ǃL�b�N�̓����蔻��
    private  bool CheckWallKick;
    public bool GetSetwallKick { get { return CheckWallKick; } set { CheckWallKick = value; } }
    
    //�����n���p��Player�̕ǂɓ����������W
    private Vector3 wallTouchPos;
    public Vector3 GetSetwallTouchPos { get { return wallTouchPos; } set { wallTouchPos = value; } }


    void Start()
    {
        //Player�̈ʒu��Enemy�̈ʒu���擾����
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GameObject.FindGameObjectWithTag("Enemy").transform;
        rbEnemy = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        trans = transform;
        prevPos = trans.position;
    }


    void FixedUpdate()
    {
        int upRotation = 0;       //�ʏ펞��Y�����O�ɌŒ肷��
        float wallRotation_x = 0; //�ǂɓo��Ƃ��̊p�x

        Debug.DrawRay(transform.position, (transform.forward + new Vector3(45, 0, 45)) * 0.25f);
        Debug.DrawRay(transform.position, (transform.forward + new Vector3(-45, 0, 45)) * 0.25f);
        Debug.DrawRay(transform.position, transform.forward, UnityEngine.Color.red);

        //Player�������ꏊ�ɍs���AFloor��True�Ȃ�W�����v����
        if ((Mathf.Floor(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && floor) || stepJamp)
        {
            rbEnemy.velocity = Vector3.up * jump * jampStand;
        }
        //Player����ɂ��ĕǂɂ������Ă���Ȃ�ǂ�o��
        else if (Mathf.Ceil(playerTr.transform.position.y) > Mathf.Floor(enemyTr.transform.position.y) && wall)
        {
            rbEnemy.velocity = Vector3.up * jump * 0.25f;

            upRotation = 270;//�ǂ�o��Ƃ��͏�������悤�ɂ���

            RaycastHit hit;

            //�ǂɐ����Ɍ������߂̏���
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
            //�ǂɂ������Ă���Player�Ɠ��������ɂ���Ȃ�Player�̕����֔��
            transform.position =
                 Vector3.MoveTowards(transform.position,
                 new Vector3(playerTr.position.x, playerTr.position.y, playerTr.position.z),
                 speed * Time.deltaTime * 2);
        }
        else if (GetSetwallKick && transform.position != GetSetwallTouchPos && !wall)
        {
            //Player���ǃL�b�N���Ă���Ԃ�Player���ǂɓ��������ꏊ�֐i�ނ悤�ɂ���
            transform.position =
               Vector3.MoveTowards(transform.position, GetSetwallTouchPos, speed * Time.deltaTime);
        }
        else if (Mathf.Floor(playerTr.transform.position.y - enemyTr.transform.position.y) > 15f)
        {
            //Player���ƂĂ������ꏊ�ɂ������ꍇ�A�����W�����v����悤�ɂ���
            rbEnemy.velocity = Vector3.up * 10;
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
            rotation = Quaternion.LookRotation(new Vector3(wallRotation_x, upRotation, 0), Vector3.up);
        }
        else if (!floor)//�W�����v����Ƃ��ɏ��������悤�ɂ���
        {
            rotation = Quaternion.LookRotation(delta, Vector3.up);
        }

        //�I�u�W�F�N�g�ɔ��f
        trans.rotation = rotation;
    }


    private void OnCollisionStay(Collision collision)
    { 
        //�i���ɓ���������W�����v�ł���悤�ɂ���
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
