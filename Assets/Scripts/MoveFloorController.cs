using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloorController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidBody = null;

    [SerializeField]
    Vector3 speed = new Vector3(1f,1f,1f);

    List<Rigidbody> rigidBodies = new();

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
        rigidBody.MovePosition(transform.position + Time.deltaTime * speed);
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
