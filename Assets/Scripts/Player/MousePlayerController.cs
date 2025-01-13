using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MousePlayerController : MonoBehaviour
{
    private float moveSpeed = 8f;//�ړ����x
    private float inputV;
    private float jumpForce = 4.5f;//�W�����v��
    private float jumpStand = 1;//�W�����v��̓����蔻��
    //�W�����v�⑖�鏈���̔���
    public  bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool wkey = false;
    private bool poleTouch = false;
    //�ǂ̓����蔻��
    private bool wallTouch;
    public bool wallTouchgs { get { return wallTouch; } set { wallTouch = false; } }
    //Akey��Dkey�ŕǂ��̂ڂ邽�߂̔���
    private bool aKey = true;
    private bool dKey = true;
    private bool upWall = false;
    private bool run = true;
    //�|�[���W�����v�p
    [SerializeField] private Transform poleTarget;
    [SerializeField] private float spinSpeed =4f;
    //Player��Rigidbody
    private Rigidbody rbPlayer;
    private Animator animator;
    private EnemyController EnemyController;
    [SerializeField] private CameraController CameraController;
    //�_�b�V���Q�[�W�̃X���C�_�[
    [SerializeField] private Slider dushGaugeSlider;
    //Audio�֌W
    public AudioClip jampSE;
    public AudioClip runSE;
    public AudioClip upWallSE;
    private AudioSource audioSource;
    
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GameObject enemyObj = GameObject.Find("Enemy");
        EnemyController = enemyObj.GetComponent<EnemyController>();
        dushGaugeSlider.value = 3f;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //�^�C���X�P�[�����[���̏ꍇ�͏��������Ȃ�
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        inputV = Input.GetAxisRaw("Vertical");

        //�X�y�[�X�ŃW�����v
        if (Input.GetKey(KeyCode.Space) && jump)
        {
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0);//�W�����v�̃��[�V����
            //�W�����v��ɏ���Ă���Ȃ獂���W�����v
            rbPlayer.velocity = Vector3.up * jumpForce*jumpStand;
            
        }

        //�ǂɓ�����Ȃ���Akey��Dkey�����݂ɉ������Ƃŕǂ��̂ڂ�
        if(Input.GetKeyDown(KeyCode.A)&&aKey&&wallTouch)
        {
            aKey = false;
            dKey = true;
            upWall = true;
        }

        if (Input.GetKeyDown(KeyCode.D) && dKey && wallTouch)
        {
            aKey =true; 
            dKey = false;
            upWall = true;
        }

        if (upWall)
        {
            audioSource.PlayOneShot(upWallSE);
            transform.position += new Vector3(0,0.5f,0);
            upWall = false;   
        }

        //�ǃL�b�N�̏���
        if (Input.GetKeyDown(KeyCode.W)&&wallTouch)
        {
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0);//�W�����v�̃��[�V����
            rbPlayer.velocity = Vector3.up * 7;
            CameraController.InversionCamera();

            //��x�����G�ɕǂɓ��������ʒu�𑗂�
            if (!EnemyController.GetSetwallKick)
            {
                EnemyController.wallTouchPos = transform.position;
            }

            EnemyController.GetSetwallKick = true;
        }
        else
        {
            wkey = false;
        }
        //�|�[���W�����v�̏���
        if (Input.GetKey(KeyCode.S)&&poleTouch)
        {
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0);//�W�����v�̃��[�V����
            transform.RotateAround(poleTarget.position,-transform.right,spinSpeed);
            rbPlayer.velocity = Vector3.up * jumpForce;
        }
        
        //�E�N���b�N�ő���悤�ɂ���iPlayer�̈ړ����x���グ��j�󒆂ł͑����Ȃ��悤�ɂ���
        if (Input.GetMouseButton(1) && dush && jump&&dushGaugeSlider.value > 0)
        {
            StopCoroutine("DushStop");
            moveSpeed = 15;
            
            if (coroutine)
            {
                audioSource.PlayOneShot(runSE);
                coroutine = false;
                StartCoroutine("DushCotroller");
            }
        }
        else
        {
            //��莞�ԑ������瑬�x�����Ƃɖ߂�
            if (!coroutine)
            {
                dush = false;
                coroutine = true;
                
                StopCoroutine("DushCotroller");
                StartCoroutine("DushStop");
            }

            moveSpeed = 8;
        }
    }

    void FixedUpdate()
    {
        if (run)
        {
            //�J�����̕�������X-Z���ʂ̒P�ʃx�N�g�����擾
            Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            //�����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
            Vector3  moveFoward = cameraFoward;

            //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
            rbPlayer.velocity = moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);

            //�L�����N�^�[�̌�����i�s������
            if (moveFoward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveFoward * Time.deltaTime);
            }
        }
    }
    
    //����̂���߂����莞�ԑ���Ȃ��悤�ɂ���
    private IEnumerator DushCotroller()
    {
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        dush = false;
    }

    private IEnumerator DushStop()
    {
        audioSource.Stop();
        int gauge = 3-(int)dushGaugeSlider.value;

        for (int i = gauge; i >= 0; i--)
        {
            yield return new WaitForSeconds(1.0f);
            dushGaugeSlider.value += 1f;
            dush = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //��i�W�����v�ł��Ȃ��悤�ɂ���i���n�ŃW�����v�ł���悤�ɂ���j
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("JumpStand"))
        {
            jump = true;
            aKey = true;
            dKey = true;
            upWall = false;
            animator.Play("Idle");//���n�����瑖�郂�[�V�����ɖ߂�
        }

        if (collision.gameObject.CompareTag("Step")||collision.gameObject.CompareTag("Wall"))
        {
            run = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallTouch = true;
            UpwardForce();
        }
        else
        {
            //Player���n�ʂɂ���Ԃ�Enemy�͕ǂɂ��鎞�̏��������Ȃ��悤�ɂ���
            EnemyController.GetSetwallKick = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //���ꂽ��߂�
        if (collision.gameObject.CompareTag("Wall"))
        {
            DownwardForce();
            wallTouch = false;
        }

        if (collision.gameObject.CompareTag("floor")||collision.gameObject.CompareTag("JumpStand"))
        {
            jump = false;
        }

        if (collision.gameObject.CompareTag("Step") || collision.gameObject.CompareTag("Wall"))
        {
            run = true;
        }
    }

    //�|�[���ɓ����������̏���
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pole"))
        {
            poleTarget =other.transform;
           
            poleTouch = true ;
        }

        if (other.gameObject.CompareTag("JumpStand"))
        {
            jumpStand = 3;
        }
    }

    //���ꂽ��߂�
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pole"))
        {
            poleTouch = false;
        }

        if (other.gameObject.CompareTag("JumpStand"))
        {
            jumpStand = 1;
        }
    }

    //�ǂɓ������Ă���ԁA�����鑬�x�𗎂Ƃ�
    void UpwardForce()
    {
        //��R�𑝂₷
        rbPlayer.drag = 1;
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
