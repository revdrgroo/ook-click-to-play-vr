using UnityEngine;
using System.Collections;

public class PunataExplosion : MonoBehaviour {
  ParticleSystem particles;
  public GameObject[] debris;
  public int fantaleCount = 5;
  public float explosionHeight;
  public float lifeTimer = 5.0f;
  public float sinkTime = 2.0f;
  public AudioClip sound;
  public float volume;

  GameObject[] projectiles;
  private bool sinkingDebris;

  // Use this for initialization
  void Start () {
    sinkingDebris = false;
    particles = gameObject.GetComponentInChildren<ParticleSystem>();
    projectiles = new GameObject[fantaleCount];
    for (int i = 0; i < fantaleCount; i++) {
      Vector3 direction = Quaternion.Euler(Random.Range(0, 15.0f), Random.Range(0, 360.0f), 0) * Vector3.up;
      Vector3 pos = transform.position + (direction + Vector3.up) * explosionHeight;

      int d = Random.Range(0, debris.Length);
      GameObject obj = (GameObject) Instantiate(debris[d], pos, gameObject.transform.rotation);
      obj.SetActive(true);
      Projectile proj = obj.GetComponent<Projectile>();
      proj.direction = direction;
      projectiles[i] = obj;
    }
    particles.Play();
    AudioSource.PlayClipAtPoint(sound, transform.position, volume);
	}
	
	// Update is called once per frame
	void Update () {
    lifeTimer -= Time.deltaTime;
    if (lifeTimer <= sinkTime && !sinkingDebris) {
      SinkDebris();
    }
    if (lifeTimer <= 0.0f) {
      Destroy(gameObject);
    }
	}

  void SinkDebris() {
    sinkingDebris = true;
    foreach (GameObject obj in projectiles) {
      Sinker sinker = obj.GetComponent<Sinker>();
      sinker.Sink();
    }
  }
}
