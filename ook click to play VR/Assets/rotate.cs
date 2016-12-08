using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {
  public float rate = 30.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    transform.RotateAround(transform.position, Vector3.up, rate * Time.deltaTime);
	}
}
