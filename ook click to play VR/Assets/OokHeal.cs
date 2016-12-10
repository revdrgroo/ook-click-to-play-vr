using UnityEngine;
using System.Collections;

public class OokHeal : MonoBehaviour {
  public float healAmount = 10.0f;
  public GameObject ook;
  public AudioClip healSound;
  public float healSoundVolume = 1.0f;

  private OokMovement ookMovement;
  private Sinker sinker;

  // Use this for initialization
  void Start () {
    ookMovement = ook.GetComponent<OokMovement>();
    sinker = GetComponent<Sinker>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnCollisionEnter(Collision collision) {
    if (collision.gameObject == ook) {
      ookMovement.Heal(healAmount);
      AudioSource.PlayClipAtPoint(healSound, transform.position, healSoundVolume);
      if (sinker != null) {
        sinker.Sank();
      }
    }
  }
}
