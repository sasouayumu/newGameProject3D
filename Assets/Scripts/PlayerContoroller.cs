using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour
{
    public int hokou = 1;//���� �k�P�@���Q�@��R�@���S
    public float rtateSpeed = 3.0f;//��]���x
    public float jumpForce = 4f;
    Rigidbody rb;
    public GameObject cam; //�J����
    public GameObject player;//�v���C���[
    bool key = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        Transform myTransform = this.transform;
        
        Vector3 pos = myTransform.position;
        Vector3 eul = myTransform.eulerAngles;

        pos.z += 0.02f;
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
        
        //�A���Ń{�^���������Ȃ��悤�ɂ���
        if (key)
        {
            //��
            if (Input.GetKey(KeyCode.A))
            {
                
                //cam.transform.position = new Vector3(pos.x+2, pos.y + 1, pos.z);
                player.transform.eulerAngles = new Vector3(0, eul.y-90, 0);
                key = false;
                //hokou = 2;
                //pos.x -= 0.02f;
                hokou -= 1;

            }
            //�E
            if (Input.GetKey(KeyCode.D))
            {

                
                //cam.transform.position = new Vector3(pos.x, pos.y + 1, pos.z + 2);
                player.transform.eulerAngles = new Vector3(0, eul.y+90, 0);
                key = false;
                hokou += 1;
                //hokou = 3;
                //pos.z -= 0.02f;
               
            }

            if (!key)
            {
                StartCoroutine("KeyStop");
               
            }
           
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = Vector3.up * jumpForce;
        }

        myTransform.position = pos;
    }

   
    IEnumerator KeyStop()
    {
        yield return new WaitForSeconds(0.3f);
        key = true;
        
    }
}


