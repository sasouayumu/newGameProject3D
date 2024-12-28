using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloorController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidBody = null;

    [SerializeField]
    Vector3 speed = Vector3.zero;

    List<Rigidbody> rigidBodies = new();

    private Vector3 floorPos;
    private bool floorMove;
    void Start()
    {
        floorPos = transform.position;

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        MoveFloor();
        AddVelocity();
    }

    private void OnTriggerEnter(Collider other)
    {
        rigidBodies.Add(other.gameObject.GetComponent<Rigidbody>());
    }

    private void OnTriggerExit(Collider other)
    {
        rigidBodies.Remove(other.gameObject.GetComponent<Rigidbody>());
    }

    void MoveFloor()
    {
        if (transform.position == floorPos)
        {
            floorMove = true;
        }
        else if (transform.position == floorPos +new Vector3(5f, 0, 0))
        {
            floorMove = false;
        }

        if (floorMove)
        {
            rigidBody.MovePosition(transform.position + Time.deltaTime * speed);
        }
        else
        {
            rigidBody.MovePosition(transform.position - Time.deltaTime * speed);
        }
    }

    void AddVelocity()
    {
        if (rigidBody.velocity.sqrMagnitude <= 0.01f)
        {
            return;
        }

        for (int i = 0; i < rigidBodies.Count; i++)
        {
            rigidBodies[i].AddForce(rigidBody.velocity, ForceMode.VelocityChange);
        }
    }
}
