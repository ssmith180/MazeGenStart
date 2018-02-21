using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWASD : MonoBehaviour
{

    public float speed = 2.0f;

    private CharacterController myController;

    // Use this for initialization
    void Start()
    {
        myController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.A)) moveDirection -= Vector3.right;
        if (Input.GetKey(KeyCode.S)) moveDirection -= Vector3.forward;
        if (Input.GetKey(KeyCode.D)) moveDirection += Vector3.right;

        if (moveDirection != Vector3.zero)
        {
            myController.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Wall")
        {
            hit.transform.GetComponent<MeshRenderer>().material = this.GetComponent<MeshRenderer>().material;
        }
    }

}