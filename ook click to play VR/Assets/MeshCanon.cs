using UnityEngine;
using Rand = UnityEngine.Random;
using System.Collections.Generic;
using System;

public class MeshCanon : MonoBehaviour {
  public float spread = 10.0f;
  public float speed = 10.0f;
  public float spin = 1.5f;
  public float interval = 0.08f;
  public float shootDuration = 3.0f;
  public float hangAround = 5.0f;
  public float sinkTime = 1.5f;
  public GameObject projectile;
  public int count = 10;

  private float bulletCountdown;
  private float shootingCountdown;
  private float cleanupCountdown;
  private List<GameObject> projectiles;
  private bool firing;
  private bool awaitingCleanup;
  private float mass;
  private bool sinking;

  // Use this for initialization
  void Start () {
    projectiles = new List<GameObject>();
    mass = projectile.GetComponent<Rigidbody>().mass;
    //Fire();
	}
	
	// Update is called once per frame
	void Update () {
	  if (firing) {
      if (shootingCountdown <= 0) {
        firing = false;
      } else {
        if (bulletCountdown <= 0) {
          bulletCountdown = interval * Rand.Range(1.0f, 1.2f);
          Quaternion rot = SpreadAngle(transform.forward, spread) * transform.rotation;
          GameObject bullet = (GameObject)Instantiate(projectile, transform.position, rot, null);
          bullet.SetActive(true);
          Rigidbody rb = bullet.GetComponent<Rigidbody>();
          rb.AddForce(bullet.transform.forward * speed * 100 * mass);
          Quaternion q = Rand.rotation;
          float angle;
          Vector3 axis;
          q.ToAngleAxis(out angle, out axis);
          rb.AddTorque(axis * Rand.Range(spin * 1000 / 2, spin * 1000));

          projectiles.Add(bullet);
        }
      }
    }
    if (cleanupCountdown < sinkTime && !sinking) {
      sinkProjectiles();
    }
    if (awaitingCleanup && cleanupCountdown <= 0) {
      Cleanup();
    }
    bulletCountdown -= Time.deltaTime;
    shootingCountdown -= Time.deltaTime;
    cleanupCountdown -= Time.deltaTime;
	}

  private void Cleanup() {
    awaitingCleanup = false;
    //foreach (GameObject proj in projectiles) {
    //  proj.SetActive(false);
    //  Destroy(proj);
    //}
    projectiles.Clear();
  }

  public bool Fire() {
    if (awaitingCleanup) {
      return false;
    }

    sinking = false;
    firing = true;
    awaitingCleanup = true;
    projectiles.Clear();
    bulletCountdown = 0.0f;
    cleanupCountdown = hangAround + shootDuration;
    shootingCountdown = shootDuration;
    return true;
  }

  private void sinkProjectiles() {
    foreach (GameObject obj in projectiles) {
      Sinker sinker = obj.GetComponent<Sinker>();
      sinker.Sink();
    }
  }

  private Quaternion SpreadAngle(Vector3 forward, float angle) {
    float rdot = Math.Abs(Vector3.Dot(Vector3.right, forward));
    Vector3 v = rdot > 0.8f ? Vector3.up : Vector3.right;
    Vector3 cross = Vector3.Cross(forward, v);
    Quaternion rot1 = Quaternion.AngleAxis(Rand.Range(0, 360), forward);
    Vector3 v1 = rot1 * cross;
    return Quaternion.AngleAxis(Rand.Range(-angle, angle), v1);
  }
}
