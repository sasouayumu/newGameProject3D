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
        //カメラの方向からX-Z平面の単位ベクトルを取得
        Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        //方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveFoward = cameraFoward+ Camera.main.transform.right*inputV;



        //スピードを上げる
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

        //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rbPlayer.velocity = moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);

        //キャラクターの向きを進行方向に
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
