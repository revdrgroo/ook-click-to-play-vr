using UnityEngine;
using System.Collections;
using System;

public class Sinker : MonoBehaviour {
  public float rate = 1.0f;
  public float sinkTime = 1.0f;

  private bool sinking;
  private float sinkCountdown;

  void Start () {
    enabled = false;
    sinking = false;
	}
	
	void Update () {
    if (sinking) {
      transform.position = transform.position + Vector3.down * rate * Time.deltaTime;
      sinkCountdown -= Time.deltaTime;
      if (sinkCountdown <= 0) {
        Destroy(gameObject);
      }
    }
	}

  public void Sink() {
    Rigidbody body = GetComponent<Rigidbody>();
    body.isKinematic = true;
    sinking = true;
    enabled = true;
    sinkCountdown = sinkTime;
  }

  public void Sank() {
    Rigidbody body = GetComponent<Rigidbody>();
    body.isKinematic = true;
    transform.position = transform.position + Vector3.down * rate * sinkTime * 2;
  }
}
