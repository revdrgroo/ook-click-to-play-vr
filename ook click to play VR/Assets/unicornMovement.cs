using UnityEngine;
using System.Collections;

public class unicornMovement : MonoBehaviour {

  public float velocity = 1.0f;
  public float angularVelocity = 30.0f;

  Rigidbody body;
	// Use this for initialization
	void Start () {
    body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
//    transform.RotateAround(transform.position, Vector3.up, angularVelocity * Time.deltaTime);
//    transform.Translate(Vector3.forward * velocity * Time.deltaTime);

    Quaternion rot = Quaternion.Euler(0, angularVelocity * Time.deltaTime, 0);
    body.MoveRotation(body.rotation * rot);
    body.MovePosition(body.position + transform.forward * velocity * Time.deltaTime);
	}
}
