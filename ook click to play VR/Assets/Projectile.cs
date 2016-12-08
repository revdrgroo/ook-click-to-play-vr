using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
  public float speed;
  public Vector3 direction;
  public float colliderDelay = 0;
  Quaternion angVelocity;
  Rigidbody rigidBody;
  Collider collider;
  float colliderCounter;
	void Start () {
    rigidBody = GetComponent<Rigidbody>();
    rigidBody.AddForce(direction * speed);
    Vector3 torque = Quaternion.Euler(Random.Range(0, 90.0f), Random.Range(0, 360.0f), 0) * Vector3.up;
    rigidBody.AddTorque(torque);
    collider = GetComponent<Collider>();
    collider.enabled = false;
    colliderCounter = colliderDelay;
	}
	
	// Update is called once per frame
	void Update () {
    if (colliderDelay > 0.0f) {
      colliderDelay = Mathf.Max(0, colliderDelay - Time.deltaTime);
      if (colliderDelay == 0) {
        collider.enabled = true;
      }
    }
	}
}
