using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MousePlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float inputH;
    public float inputV;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        if (Input.GetMouseButton(1))
        {
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed = 5f;
        }

        //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
        rb.velocity = moveFoward * moveSpeed + new Vector3(0, rb.velocity.y, 0);

        //�L�����N�^�[�̌�����i�s������
        //if(moveFoward != Vector3.zero)
        //{
        transform.rotation = Quaternion.LookRotation(moveFoward);
        //}




    }
}
