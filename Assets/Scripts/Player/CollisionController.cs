using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionController : MonoBehaviour
{
    private float input;
    bool key = false;
    bool isStop;
    public bool wallTouch { get; private set; }
    [SerializeField] private MousePlayerController mousePlayer;
    Rigidbody rigP;

    private Vector3 colPos;
    // Start is called before the first frame update
    void Start()
    {
        rigP = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
        rigP.velocity = new Vector3(input * 3, rigP.velocity.y, 0);
        if (Input.GetKeyDown("w"))
        {
            isStop = false;
            rigP.velocity = new Vector3(rigP.velocity.x, 7, 0);
        }

        //if (isStop)
        //{
        //    gameObject.transform.position = colPos;
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

        if (collision.gameObject.CompareTag("Goal")&&key)
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
            colPos = this.transform.position;
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
        rigP.drag = 10;
    }

    void DownwardForce()
    {
        rigP.drag = 0;
    }
}
