using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;


//�v���C���[�֘A�̃N���X
public class MousePlayerController : MonoBehaviour
{
    //Audio�֌W
    [SerializeField] private AudioClip jampSE;
    [SerializeField] private AudioClip runSE;
    [SerializeField] private AudioClip upWallSE;
    private AudioSource audioSource;

    [SerializeField] private CameraController cameraController;

    //�|�[���W�����v�p
    [SerializeField] private Transform poleTarget;
    [SerializeField] private float spinSpeed = 4f;

    //�_�b�V���Q�[�W�̃X���C�_�[
    [SerializeField] private Slider dushGaugeSlider;

    [SerializeField]private float upSpeed = 8f;�@�@�@ //�_�b�V�����̃X�s�[�h
    [SerializeField] private float usuallySpeed = 5f; //�ʏ펞�̃X�s�[�h
    [SerializeField] private float jumpForce = 4.5f;  //�W�����v��
    private float inputV;
    private float moveSpeed;                        �@//�ړ����x
    private float jumpStand = 1f;                   �@//�W�����v��̔{��

    //�W�����v�⑖�鏈���̔���
    private bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool wKey = false;
    private bool poleTouch = false;
   
    //Akey��Dkey�ŕǂ��̂ڂ邽�߂̔���
    private bool aKey = true;
    private bool dKey = true;
    private bool upWall = false;
    private bool run = true;

    //Player��Rigidbody
    private Rigidbody rbPlayer;
    private Animator animator;
    private EnemyController enemyController;

    //�ǂ̓����蔻��
    private bool wallTouch;


    void Start()
    {
        moveSpeed = usuallySpeed;
        rbPlayer = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        GameObject enemyObj = GameObject.Find("Enemy");
        enemyController = enemyObj.GetComponent<EnemyController>();
        dushGaugeSlider.value = 3f;
        audioSource = GetComponent<AudioSource>();
    }


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
            animator.Play("Jump", 0, 0); //�W�����v�̃��[�V����

            //�W�����v��ɏ���Ă���Ȃ獂���W�����v
            rbPlayer.velocity = Vector3.up * jumpForce * jumpStand;
        }

        //�ǂɓ�����Ȃ���Akey��Dkey�����݂ɉ������Ƃŕǂ��̂ڂ�
        if(Input.GetKeyDown(KeyCode.A) && aKey && wallTouch)�@
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

        //�ǂ��̂ڂ�
        if (upWall)
        {
            audioSource.PlayOneShot(upWallSE);
            rbPlayer.velocity = Vector3.up*4;
            upWall = false;   
        }

        //�ǃL�b�N�̏���
        if (Input.GetKeyDown(KeyCode.W)&&wallTouch)
        {
            run = true;
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0); �@�@�@ //�W�����v�̃��[�V����
            rbPlayer.velocity = Vector3.up * 8; //���Α��ɃW�����v����
            cameraController.InversionCamera(); //�J�����𔽓]����

            //��x�����G�ɕǂɓ��������ʒu�𑗂�
            if (!enemyController.GetSetwallKick)
            {
                enemyController.GetSetwallTouchPos = transform.position;
            }

            enemyController.GetSetwallKick = true;
        }
        else
        {
            wKey = false;
        }

        //�|�[���W�����v�̏���
        if (Input.GetKeyDown(KeyCode.Space)&&poleTouch)
        {
            audioSource.PlayOneShot(jampSE);
            animator.Play("Jump", 0, 0); //�W�����v�̃��[�V����
            transform.RotateAround(poleTarget.position,-transform.right,spinSpeed);
            rbPlayer.velocity = Vector3.up * jumpForce*1.6f;
        }
        
        //�E�N���b�N�ő���悤�ɂ���iPlayer�̈ړ����x���グ��j�󒆂ł͑����Ȃ��悤�ɂ���
        if (Input.GetMouseButton(1) && dush && jump&&dushGaugeSlider.value > 0)
        {
            StopCoroutine("DushStop");
            
            if (coroutine)
            {
                moveSpeed = upSpeed;
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
                moveSpeed = usuallySpeed;
                dush = false;
                coroutine = true;
                
                StopCoroutine("DushCotroller");
                StartCoroutine("DushStop");
            }
        }
    }


    void FixedUpdate()
    {
        //�J�����̕�������X-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //�����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveFoward = cameraFoward;

        RaycastHit hit;
        Vector3 runForwardR = transform.forward + new Vector3(0.7f, 0, 0);�@//�E������Ray�̊p�x
        Vector3 runForwardL = transform.forward - new Vector3(0.7f, 0, 0);  //��������Ray�̊p�x
       
        //Ray���Ǝ�
        if (Physics.Raycast(transform.position + (Vector3.up / 8), transform.forward, out hit, 0.3f)
            || Physics.Raycast(transform.position + (Vector3.up / 8), runForwardR, out hit, 0.3f)
            || Physics.Raycast(transform.position + (Vector3.up / 8), runForwardL, out hit, 0.3f)
            || Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 0.3f)
            || Physics.Raycast(transform.position + Vector3.up, runForwardR, out hit, 0.3f)
            || Physics.Raycast(transform.position + Vector3.up, runForwardL, out hit, 0.3f))
        {
            //hit����Tag��Wall�܂���Step�Ȃ�Player�Ɉړ������Ȃ��悤�ɂ���
            if (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("Step"))
            {
                run = false;
            }
            else
            {
                run = true;
            }
        }
        else
        {
            run = true;
        }

        if (run)
        {
            //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
            rbPlayer.velocity = moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);
        }
        
        //�L�����N�^�[�̌�����i�s�����ɂ���
        if (moveFoward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveFoward * Time.deltaTime);
        }
    }
    

    //�_�b�V�����̃Q�[�W�����炷����
    private IEnumerator DushCotroller()
    {
        //�_�b�V�����͈�b���Q�[�W�����炷
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;
        yield return new WaitForSeconds(1.0f);
        dushGaugeSlider.value -= 1f;

        dush = false; //�_�b�V���ł��Ȃ��悤�ɂ���
    }


    //�_�b�V���I��������̃Q�[�W�񕜂̏���
    private IEnumerator DushStop()
    {
        audioSource.Stop();
        int gauge = 3-(int)dushGaugeSlider.value;�@//���������̃Q�[�W���v�Z

        //���������������񕜂�����
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
        if (collision.gameObject.CompareTag("floor"))
        {
            //�W�����v���ł���悤�ɂ���
            jump = true;

            animator.Play("Idle");//���n�����瑖�郂�[�V�����ɖ߂�

            //Player���n�ʂɂ���Ԃ�Enemy�͕ǂɂ��鎞�̏��������Ȃ��悤�ɂ���
            enemyController.GetSetwallKick = false;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        //���ꂽ��W�����v���ł��Ȃ��悤�ɂ���
        if (collision.gameObject.CompareTag("floor"))
        {
            jump = false;
        }
    }

  
    private void OnTriggerEnter(Collider other)
    {
        //�|�[���ɓ����������̏���
        if (other.gameObject.CompareTag("Pole"))
        {
            poleTarget =other.transform;
           
            poleTouch = true ;
            run = false;
        }

        //�W�����v��ɓ����������̏���
        if (other.gameObject.CompareTag("JumpStand"))
        {
            jumpStand = 2.5f;
        }

        //�ǂɓ����������̏���
        if (other.gameObject.CompareTag("Wall"))
        {
            wallTouch = true;
        }
    }


    //���ꂽ��߂�
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pole"))
        {
            poleTouch = false;
            run = true;
        }

        if (other.gameObject.CompareTag("JumpStand"))
        {
            jumpStand = 1;
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            //�Ǔo�肷�鎞�̔����߂�
            aKey = true;
            dKey = true;
            upWall = false;

            wallTouch = false;
        }
    }


    //�A�j���[�V�����C�x���g�p
    private void OnCallChangeFace(){}
}
