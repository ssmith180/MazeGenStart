using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorWhenHitByPlayer : MonoBehaviour {

    public Material materialToUse;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        print("Collision started");

        if (collision.transform.tag == "Player")
        {
            GetComponent<MeshRenderer>().material = materialToUse;
        }
    }
    
}
