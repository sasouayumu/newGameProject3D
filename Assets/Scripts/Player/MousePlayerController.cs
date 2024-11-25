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
    private int moveInversion = 1;

    private float moveSpeed = 5f;
    //private float inputH;
    private float inputV;
    private float jumpForce = 5f;
    [SerializeField] private float wallKickHS = 3f;
    [SerializeField] private float wallKickVS = 3f;
    [SerializeField] private float maxStickWallKickFS = 1f;
    private bool jump = true;
    private bool dush = true;
    private bool coroutine = true;
    private bool coroutine2 = true;
    private bool move = true;
    private bool key = false;
    bool wallTouch;
    public bool wallTouchgs { get { return wallTouch; } set { wallTouch = false; } }
    private Vector3 velocity;
    Rigidbody rbPlayer;
    public Rigidbody rbWallKick { get { return rbPlayer; } set{ rbPlayer = GetComponent<Rigidbody>(); } }
    CollisionController collisionCon;
    public Vector3 NomalOfStickingWall { get; private set; } = Vector3.zero;
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

        //inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //カメラの方向からX-Z平面の単位ベクトルを取得
        Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        if (Input.GetMouseButton(2) && move)
        {
            move = false;
            moveInversion *= -1;
        }
        else if (!Input.GetMouseButton(2) && !move)
        {
            move = true;

        }

       


        //方向キーの入力値とカメラの向きから、移動方向を決定
        //+Camera.main.transform.right * inputV
        Vector3 moveFoward = cameraFoward;

        //スピードを上げる
        if (Input.GetMouseButton(1) && dush)
        {
            
            moveSpeed = 10f;
            GetComponent<Renderer>().material.color = UnityEngine.Color.blue;
            if (coroutine)
            {
                
                coroutine = false;
                StartCoroutine("DushCotroller");
            }

        }
        else
        {
            GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            if (!coroutine&&coroutine2)
            {
                dush = false;
                coroutine = true;
                coroutine2 = false;
                
                StopCoroutine("DushCotroller");
                StartCoroutine("DushStop");
            }
            
            moveSpeed = 5f;

        }
        //Debug.Log(wallTouch);
        //壁にくっついている間に反対側に飛ぶようにする
        //if (Input.GetKeyDown("w")&&wallTouch)
        //{
        //    Debug.Log("w");
        //    wallTouch = false;
        //    //rbPlayer.velocity = Vector3.forward*10.0f;
        //    //rbPlayer.velocity = Vector3.up * 10.0f;
        //    //rbPlayer.velocity = Vector3.up * jumpForce*5 + -Vector3.right;
        //    //rbPlayer.velocity = -Vector3.right;
        //    Debug.Log(rbPlayer.velocity);
            
        //}
        //else
        //{
            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            rbPlayer.velocity = moveInversion * moveFoward * moveSpeed + new Vector3(0, rbPlayer.velocity.y, 0);

            //キャラクターの向きを進行方向に
            if (moveFoward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveFoward);
            }
        //}

        if (Input.GetKey(KeyCode.Space) && jump)
        {
            //yPos = rbPlayer.velocity.y;
            rbPlayer.velocity = Vector3.up * jumpForce;
            jump = false;
        }

    }

    //public bool EnemyJump()
    //{
    //    return enJump;
    //}

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
        if (collision.gameObject.CompareTag("floor") || collision.gameObject.CompareTag("Stand"))
        {
            jump = true;
        }

        //if (collision.gameObject.CompareTag("Stand"))
        //{
        //    Debug.Log("ジャンプかいし"+yPos);
        //    Debug.Log("着地"+rbPlayer.velocity.y);

        //    if(yPos > rbPlayer.velocity.y&& !jump)
        //    {
        //        enJump = true;
        //        EnemyJump();
        //    }
        //}

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (collision.gameObject.CompareTag("Key"))
        {

            key = true;
        }

        if (collision.gameObject.CompareTag("Goal") && key)
        {

            SceneManager.LoadScene(2);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            UpwardForce();
            //Vector3 normalVector = collision.contacts[0].normal;
            //Debug.Log(normalVector.y);
            //if(normalVector.y < 0.02f)
            //{
            //    this.mousePlayer.StickWall(normalVector);
            //}
            wallTouch = true;
            //colPos = this.transform.position;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallTouch = false;
            DownwardForce();
        }
    }
    void UpwardForce()
    {
        rbPlayer.drag = 10;
    }

    void DownwardForce()
    {
        rbPlayer.drag = 0;
    }
}
