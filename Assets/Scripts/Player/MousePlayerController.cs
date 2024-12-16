using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MousePlayerController : MoveController
{
    private int moveInversion = 1;//���������Ƃ��Ɉړ����������̂܂܂ɂ���
    private float moveSpeed = 7f;//�ړ����x
    private float inputV;
    private float jumpForce = 6f;//�W�����v��
    //�W�����v�⑖�鏈���̔���
    public  bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool coroutine2 = true;
    private bool move = true;
    private bool key = false;
    private bool wkey = false;
    //�ǂ̓����蔻��
    private bool wallTouch;
    public bool wallTouchgs { get { return wallTouch; } set { wallTouch = false; } }
    //Player�ړ��p�̍��W
    private Vector3 velocity;
    //Player��Rigidbody
    private�@Rigidbody rbPlayer;
    //public Rigidbody rbWallKick { get { return rbPlayer; } set{ rbPlayer = GetComponent<Rigidbody>(); } }//�����n���p
    private Animator animator;
    private EnemyController EnemyController;
    public CameraController CameraController;
    
    
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GameObject enemyObj = GameObject.Find("Enemy");
        EnemyController = enemyObj.GetComponent<EnemyController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        inputV = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //�J�����̕�������X-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        if (Input.GetKeyDown("w"))
        {
            StartCoroutine("WkeyDown");
        }
        
        //�����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        //+Camera.main.transform.right * inputV
        Vector3 moveFoward = cameraFoward;

        //�E�N���b�N�ő���悤�ɂ���iPlayer�̈ړ����x���グ��j�󒆂ł̓W�����v�ł��Ȃ��悤�ɂ���
        if (Input.GetMouseButton(1) && dush && jump)
        {
            moveSpeed = 12;
            //GetComponent<Renderer>().material.color = UnityEngine.Color.blue;
            if (coroutine)
            {
                coroutine = false;
                StartCoroutine("DushCotroller");
            }
        }
        else
        {
            //��莞�ԑ������瑬�x�����Ƃɖ߂�
            //GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            if (!coroutine&&coroutine2)
            {
                dush = false;
                coroutine = true;
                coroutine2 = false;
                
                StopCoroutine("DushCotroller");
                StartCoroutine("DushStop");
            }
            
            moveSpeed = 7;
        }
       
        //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
        rbPlayer.velocity = moveInversion * moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);

        //�L�����N�^�[�̌�����i�s������
        if (moveFoward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveFoward);
        }
        //}

        //�X�y�[�X�ŃW�����v
        if (Input.GetKey(KeyCode.Space) && jump)
        {
            animator.Play("Jump",0,0);//�W�����v�̃��[�V����
            rbPlayer.velocity = Vector3.up * jumpForce;
        }
    }
    private IEnumerator WkeyDown()
    {
        wkey = true;
        yield return new WaitForSeconds(0.1f);
        wkey = false;
    }
    //����̂���߂����莞�ԑ���Ȃ��悤�ɂ���
    private IEnumerator DushCotroller()
    {
        yield return new WaitForSeconds(3.0f);
        dush = false;
        
        StartCoroutine("DushStop");
    }

    private IEnumerator DushStop()
    {
        yield return new WaitForSeconds(3.0f);
        
        coroutine2 = true;
        dush = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        //��i�W�����v�ł��Ȃ��悤�ɂ���i���n�ŃW�����v�ł���悤�ɂ���j
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("Stand"))
        {
            jump = true;
            animator.Play("Idle");//���n�����瑖�郂�[�V�����ɖ߂�
        }

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�J�M�̓���
        if (collision.gameObject.CompareTag("Key"))
        {
            key = true;
        }

        //�J�M�������Ă�����S�[���ł���悤�ɂ���
        if (collision.gameObject.CompareTag("Goal") && key)
        {
            SceneManager.LoadScene(2);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            UpwardForce();
            if (wkey)
            {
                animator.Play("Jump", 0, 0);//�W�����v�̃��[�V����
                rbPlayer.velocity = Vector3.up * 10;
                CameraController.InversionCamera();
                //��x�����G�ɕǂɓ��������ʒu�𑗂�
                if (!EnemyController.GetSetwallKick)
                {
                    EnemyController.wallTouchPos = transform.position;
                }
                EnemyController.GetSetwallKick = true;
                wkey = false;
            }
        }
        else
        {
            //Player���n�ʂɂ���Ԃ�Enemy�͕ǃL�b�N���鏈�������Ȃ��悤�ɂ���
            EnemyController.GetSetwallKick = false;
        }
       
    }

    private void OnCollisionExit(Collision collision)
    {
        //���ꂽ��߂�
        if (collision.gameObject.CompareTag("Wall"))
        {
            DownwardForce();
        }

        if (collision.gameObject.CompareTag("floor")|| collision.gameObject.CompareTag("Stand"))
        {
            jump = false;
        }
    }

    //�ǂɓ������Ă���ԁA�����鑬�x�𗎂Ƃ�
    void UpwardForce()
    {
        //��R�𑝂₷
        rbPlayer.drag = 10;
    }

    void DownwardForce()
    {
        //��R��߂�
        rbPlayer.drag = 0;
    }

    void OnCallChangeFace()
    {

    }
}
