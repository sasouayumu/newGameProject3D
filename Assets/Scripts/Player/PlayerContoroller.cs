using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour
{
    public int hokou = 1;//方向 北１　東２　南３　西４
    public float rtateSpeed = 3.0f;//回転速度
    public float jumpForce = 4f;
    public float speed;
    Rigidbody rb;
    public GameObject cam; //カメラ
    public GameObject player;//プレイヤー
    private bool key = true;
    private bool jump = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera");
        speed = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 pos =this.transform.position;
        Vector3 eul = this.transform.eulerAngles;

        //pos.z += 0.02f;
        //if (Input.GetKey(KeyCode.W) || hokou == 1)
        //{
        //    cam.transform.position = new Vector3(pos.x,pos.y+1,pos.z-2);
        //    cam.transform.eulerAngles = new Vector3(15, 0, 0);
        //    hokou = 1;
        //    pos.z += 0.02f;
        //}
        //if (Input.GetKey(KeyCode.S) || hokou == 3)
        //{
        //    cam.transform.position = new Vector3(pos.x , pos.y + 1, pos.z+2);
        //    cam.transform.eulerAngles = new Vector3(15, 180, 0);
        //    hokou = 3;
        //    pos.x += 0.02f;
        //}
        
        //連続でボタンを押せないようにする
        if (key)
        {
            //左
            if (Input.GetKey(KeyCode.A))
            {
                
                //cam.transform.position = new Vector3(pos.x+2, pos.y + 1, pos.z);
                player.transform.eulerAngles = new Vector3(0, eul.y-90, 0);
                key = false;
                //hokou = 2;
                //pos.x -= 0.02f;
                hokou -= 1;

            }
            //右
            if (Input.GetKey(KeyCode.D))
            {

                
                //cam.transform.position = new Vector3(pos.x, pos.y + 1, pos.z + 2);
                player.transform.eulerAngles = new Vector3(0, eul.y+90, 0);
                key = false;
                hokou += 1;
                //hokou = 3;
                //pos.z -= 0.02f;
               
            }
            if(hokou == 5 ) 
            {
                hokou = 1;
            }else if (hokou == 0)
            {
                hokou = 4;
            }

            if (hokou == 1)
            {
                //pos.z += speed;
                this.transform.position += new Vector3(0, 0, speed);

            }
            else if (hokou == 2)
            {
                //pos.x += speed;
                this.transform.position += new Vector3(speed, 0, 0);

            }
            else if (hokou == 3)
            {
                //pos.z -= speed;]
                this.transform.position -= new Vector3(0, 0, speed);

            }
            else if (hokou == 4)
            {
                //pos.x -= speed;
                this.transform.position -= new Vector3(speed, 0, 0);
            }

            if (Input.GetKey(KeyCode.Space)&&jump)
            {
                rb.velocity = Vector3.up * jumpForce;
                jump = false;
            }

            if (!key)
            {
                StartCoroutine("KeyStop");
               
            }
           
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            jump = true;
        }
    }

    IEnumerator KeyStop()
    {
        yield return new WaitForSeconds(0.3f);
        key = true;
        
    }
}


