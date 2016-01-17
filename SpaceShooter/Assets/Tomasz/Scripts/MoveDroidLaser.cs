using UnityEngine;
using System.Collections;

public class MoveDroidLaser : MonoBehaviour {

    public float moveSpeed = 500;
    [HideInInspector]
    public Transform target;


    private Rigidbody rigidbody;
   

    // Use this for initialization
    void Start () {
        
        rigidbody = GetComponent<Rigidbody>();
       
    }
	

	// Update is called once per frame
	void Update () {
          
		 rigidbody.velocity = transform.forward * moveSpeed * Time.deltaTime;
    }
}
