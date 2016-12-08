using UnityEngine;
using System.Collections;

public class FishBulletController : MonoBehaviour {
  public GameObject ook;
  public float sqrCollisionThreshold = 50.0f;
  public AudioSource fishHitAudioSource;

  private OokMovement ookMovement;
  private Rigidbody rigidbody;

  // Use this for initialization
  void Start () {
    ookMovement = ook.GetComponent<OokMovement>();
    rigidbody = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update () {
	
	}

  void OnCollisionEnter(Collision collision) {
    //Debug.Log("Fish hit " + collision.gameObject);
    if (collision.gameObject == ook && collision.relativeVelocity.sqrMagnitude > sqrCollisionThreshold) {
      Debug.Log("Fish hit OOK! " + collision.relativeVelocity.magnitude);
      ookMovement.FishHit(this);
      fishHitAudioSource.Play();
    }
    //if (rigidbody.velocity.magnitude > 5) {
    //  Debug.Log("clamping velocity");
    //  float scale = rigidbody.velocity.magnitude / 5;
    //  rigidbody.velocity /= scale;
    //}
  }
}
