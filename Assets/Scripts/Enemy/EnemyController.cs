using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform playerTr;
    [SerializeField] float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = 
            Vector3.MoveTowards(transform.position,
            new Vector3(playerTr.position.x,playerTr.position.y,playerTr.position.z),
            speed*Time.deltaTime);
    }
}
