using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour {
  public Camera camera;
  public float speed;
  public float lifeTime;

  float timer;
	// Use this for initialization
	void Start () {
    timer = lifeTime;
	}
	
	// Update is called once per frame
	void Update () {
    transform.Translate(Vector3.up * speed * Time.deltaTime);
    transform.forward = camera.transform.forward;
    timer -= Time.deltaTime;
    if (timer <= 0) {
      Destroy(gameObject);
    }
	}
}
