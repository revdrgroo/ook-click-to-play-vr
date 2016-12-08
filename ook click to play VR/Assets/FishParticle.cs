using UnityEngine;
using System.Collections;

public class FishParticle : MonoBehaviour {
  public float spin = 3.0f;
  private Rigidbody rigidbody;
  

	// Use this for initialization
	void Start () {
    rigidbody = GetComponent<Rigidbody>();
    Quaternion q = Random.rotation;
    float angle;
    Vector3 axis;
    q.ToAngleAxis(out angle, out axis);
    rigidbody.AddTorque(axis * Random.Range(spin / 2, spin));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
