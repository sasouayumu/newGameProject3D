using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MousePlayerController : MoveController
{
    private float moveSpeed = 5f;
    private float inputH;
    private float inputV;
    private float jumpForce = 5f;
    private bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool coroutine2 = true;
    Rigidbody rbPlayer;
    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //float playermovement = input.getaxis("horizontal") * speed * time.deltatime;
        //transform.translate(playermovement, 0, 0);

        //float m = input.getaxis("mouse x");
        //if (mathf.abs(m) > 0.001f)
        //{
        //    transform.rotatearound(transform.position, vector3.up, m);
        //}

        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //�J�����̕�������X-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //�����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveFoward = cameraFoward+ Camera.main.transform.right*inputV;



        //�X�s�[�h���グ��
        if (Input.GetMouseButton(1) && dush)
        {
            
            moveSpeed = 10f;
            GetComponent<Renderer>().material.color = Color.blue;
            if (coroutine)
            {
                Debug.Log("hasiru"+dush);
                coroutine = false;
                StartCoroutine("DushCotroller");
            }
            
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
            if (!coroutine&&coroutine2)
            {
                coroutine = true;
                coroutine2 = false;
                Debug.Log("yame" + dush);
                StopCoroutine("DushController");
                StartCoroutine("DushStop");
            }
            
            moveSpeed = 5f;

        }



        if (Input.GetKey(KeyCode.Space) && jump)
        {
            rbPlayer.velocity = Vector3.up * jumpForce;
            jump = false;
        }

        //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
        rbPlayer.velocity = moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);

        //�L�����N�^�[�̌�����i�s������
        //if(moveFoward != Vector3.zero)
        //{
        transform.rotation = Quaternion.LookRotation(moveFoward);
        //}

    }

   

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("floor")|| collision.gameObject.CompareTag("Stand"))
        {
            jump = true;
        }
    }

    private IEnumerator DushCotroller()
    {
        
        yield return new WaitForSeconds(3.0f);
        dush = false;
        Debug.Log("tukareta"+dush);
        StartCoroutine("DushStop");
    }

    private IEnumerator DushStop()
    {
        Debug.Log("kyukei" + dush);
        yield return new WaitForSeconds(3.0f);
        Debug.Log("hasireruyo" + dush);
        coroutine2 = true;
        dush = true;
    }
    
}
