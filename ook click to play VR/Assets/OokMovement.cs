using UnityEngine;
using System;

public class OokMovement : MonoBehaviour {
  public float velocity = 1.1f;
  public float turnSpeed = 1.0f;
  public float turnTowardsSpeed = 3.0f;
  public float scratchMin = 2.0f;
  public float attackMin = 2.0f;
  public float minCursorDelta = 0.5f;
  public Camera camera;

  public GameObject hpText;
  public AudioClip ookSound;
  public float ookVolume = 1.0f;
  public float ookMaxHealth = 25.0f;
  public float ookHealth;

  public GameObject gameOverSign;

  bool walking;

  bool waving;
  bool turning;
  // targetting true if we have set a target but not reached it yet.
  bool targetting;
  bool dead;

  private const float TARGET_PROXIMITY_SQR = 0.0001f;

  bool tryToHit;
  float scratchDelay = 0;
  float attackDelay = 0;
  float startY = 0.0f;
  public GameObject enemy = null;
  Rigidbody ookRigidbody;
  Vector3 previousPos;
  Vector3 targetPos;
  Animator animator;

  int hitCount = 0;
  GameObject currentText = null;
  float gameOverDelay = 2.0f;
  float gameOverCountdown;

  void Start() {
    UnityEngine.Random.seed = (int)DateTime.Now.Ticks;
    animator = GetComponent<Animator>();
    animator.SetBool("walking", false);
    startY = transform.position.y;
    ookRigidbody = GetComponent<Rigidbody>();
    previousPos = transform.position;
    turning = walking = waving = false;
    ookHealth = ookMaxHealth;
    dead = false;
  }

 
  void Update() {
    if (dead) {
      PostDeathLoop();
      return;
    }
    if (targetting) {
      SeekTarget();
    }

    // Walking overrides waving
    if (walking) {
      waving = false;
      //tryToHit = true;
    }

    // If flagged to hit an enemy, check interval timer has elapsed and we have an enemy
    if (tryToHit) {
      if (attackDelay == 0 && enemy != null) {
        Attack();
        ClearTarget();
      }
    }

    if (IsWalking()) {
      // transform.Translate(Vector3.forward * velocity * Time.deltaTime);
      Vector3 newPos = ookRigidbody.position + transform.forward * velocity * Time.deltaTime;
      float newPosDist = (newPos - transform.position).sqrMagnitude;
      float targetDist = (targetPos - transform.position).sqrMagnitude;
      if (targetDist < newPosDist) {
        newPos = targetPos;
      }
      ookRigidbody.MovePosition(newPos);
    } else if (!turning) {
      ActBored();
    }

    if (waving) {
      Wave();
    }

    animator.SetBool("walking", walking);
    scratchDelay = Mathf.Max(0, scratchDelay - Time.deltaTime);
    attackDelay = Mathf.Max(0, attackDelay - Time.deltaTime);
    ookRigidbody.velocity = Vector3.zero;
  }

  private void SeekTarget() {
    if (!targetting) {
      return;
    }
    walking = TurnTowards(targetPos);
    Vector3 delta = (targetPos - transform.position);
    Debug.DrawLine(transform.position, targetPos, Color.green);
    delta.y = 0;
    if (delta.sqrMagnitude < TARGET_PROXIMITY_SQR) {
      walking = false;
      ClearTarget();
    }
  }

  
  void LateUpdate() {
    if ((transform.position - previousPos).magnitude > 0.5f) {
      transform.position = previousPos;
      Debug.Log("******** Ook position error ******");
    } else {
      previousPos = transform.position;
    }
  }

  bool IsWalking() {
    return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "walk";
  }

  void Wave() {
    if (TurnTowards(camera.transform.position)) {
      animator.SetTrigger("wave");
      waving = false;
    }
  }

  void Attack() {
    animator.SetTrigger("attack");
    attackDelay = attackMin;
    walking = false;
    UnicornHealth unicorn = enemy.GetComponent<UnicornHealth>();
    unicorn.TakeHit();
  }

  void ActBored() {
    if (scratchDelay == 0 && UnityEngine.Random.Range(0, 100) == 0) {
      if (UnityEngine.Random.Range(0, 2) == 0) {
        animator.SetTrigger("scratch");
      } else {
        waving = true;
      }
      scratchDelay = scratchMin;
    }
  }

  public void SetTarget(Vector3 point) {
    Vector3 direction = transform.InverseTransformPoint(point);
    if (direction.magnitude > minCursorDelta) {
      //transform.LookAt(hit.point, Vector3.up);
      SetTargetReally(point);
    }
  }

  public void SetTargetReally(Vector3 target) {
    targetPos = target;
    targetPos.y = transform.position.y;
    targetting = true;
    tryToHit = true;
  }

  public void ClearTarget() {
    if (targetting) {
      scratchDelay = scratchMin;
    }
    //Debug.Log("ook clear target");
    targetPos = Vector3.zero;
    targetting = false;
    tryToHit = false;
    walking = waving = turning = false;
    ActBored();
  }

  void HandleKeys() {
    Quaternion rot;
    if (Input.GetKey(KeyCode.LeftArrow)) {
      rot = Quaternion.Euler(0, -turnSpeed * 90 * Time.deltaTime, 0);
      ookRigidbody.MoveRotation(ookRigidbody.rotation * rot);
      //      transform.RotateAround(Vector3.down, turnSpeed * Time.deltaTime);
    } else if (Input.GetKey(KeyCode.RightArrow)) {
      rot = Quaternion.Euler(0, turnSpeed * 90 * Time.deltaTime, 0);
      ookRigidbody.MoveRotation(ookRigidbody.rotation * rot);
      //      transform.RotateAround(Vector3.up, turnSpeed * Time.deltaTime);
    }

    if (Input.GetKey(KeyCode.UpArrow)) {
      walking = true;
    }

    if (Input.GetKeyDown(KeyCode.Space)) {
      tryToHit = true;
    }
  }

  void OnTriggerExit(Collider other) {
    if (other.gameObject == enemy) {
      //      Debug.Log("clearing enemy " + enemy);
      SetCurrentEnemy(null);
    }
  }

  void SetCurrentEnemy(GameObject obj) {
    if (obj == enemy) {
      return;
    }
    if (enemy != null) {
      UnicornHealth oldUh = enemy.GetComponent<UnicornHealth>();
      oldUh.InRange(false);
      enemy = null;
    }

    if (obj != null) {
      enemy = obj;
      UnicornHealth newUh = enemy.GetComponent<UnicornHealth>();
      newUh.InRange(true);
    }
  }

  void OnTriggerEnter(Collider other) {
    if (other.gameObject.tag == "Unicorn") {
      SetCurrentEnemy(other.gameObject);
      //Debug.Log("setting enemy: " + enemy);
      //UnicornHealth uh = enemy.GetComponent<UnicornHealth>();
      //uh.InRange(true);
    }
  }

  bool TurnTowards(Vector3 target) {
    Vector3 targetDir = (target - transform.position);
    targetDir.y = 0;
    targetDir.Normalize();
    //    Debug.Log("targetDir = " + targetDir);

    float step = turnTowardsSpeed * Time.deltaTime;
    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
    Debug.DrawRay(transform.position, newDir, Color.red);
    Quaternion rot = Quaternion.LookRotation(newDir);
    //transform.rotation = rot;
    ookRigidbody.MoveRotation(rot);
    //    ookRigidbody.isKinematic = true;
    //    ookRigidbody.rotation = rot;
    //    ookRigidbody.isKinematic = false;

    //    Debug.Log("rot = " + rot.eulerAngles + " transform.rotation = " + transform.rotation.eulerAngles);
    bool finished = (Vector3.Dot(transform.forward, targetDir) > 0.99f);
    turning = !finished;
    return finished;
  }

  internal void FishHit(FishBulletController fishBulletController) {
    //Debug.Log("ook hit by " + fishBulletController);
    Hurt(1.0f);
    if (currentText == null || currentText.activeInHierarchy == false) {
      currentText = (GameObject)Instantiate(hpText, transform.position, transform.rotation);
      currentText.GetComponent<TextMesh>().color = Color.yellow;
      currentText.SetActive(true);
      hitCount = 1;
    } else {
      hitCount = Math.Min(hitCount + 1, 9);
    }
    TextMesh text = currentText.GetComponent<TextMesh>();
    text.text = string.Format("ouch! {0}", hitCount);
    //collisions.Clear();
  }

  internal void WaveHit(float intensity) {
    GameObject obj = (GameObject)Instantiate(hpText, transform.position, transform.rotation);
    obj.SetActive(true);
    TextMesh text = obj.GetComponent<TextMesh>();
    float hp = Mathf.Round(intensity * 10);
    text.color = Color.Lerp(Color.yellow, Color.red, intensity);
    text.text = string.Format("Wet! {0}", hp);
    Hurt(hp);
  }

  public void OokFootFall() {
    AudioSource.PlayClipAtPoint(ookSound, transform.position, ookVolume);
  }

  public void Heal(float percentage) {
    float healAmount = ookMaxHealth * percentage * 0.01f;
    ookHealth = Mathf.Min(ookHealth + healAmount, ookMaxHealth);
    Debug.Log("Oook heal: " + healAmount + "-> " + ookHealth + "/" + ookMaxHealth);
  }

  public void Hurt(float hurtAmount) {
    ookHealth = Mathf.Max(ookHealth - hurtAmount, 0);
    Debug.Log("Oook hurt: " + hurtAmount + "-> " + ookHealth + "/" + ookMaxHealth);
    if (ookHealth <= 0) {
      Die();
    }
  }

  private void Die() {
    ookHealth = 0;
    animator.SetTrigger("die");
    dead = true;
    gameOverCountdown = gameOverDelay;
  }

  private void PostDeathLoop() {
    gameOverCountdown -= Time.deltaTime;
    if (gameOverCountdown <= 0) {
      gameOverSign.SetActive(true);
    }
  }

  public float getHealthFraction() {
    return ookHealth / ookMaxHealth;
  }

  public bool isAlive() {
    return !dead;
  }

	public void ResetIfDead() {
		Debug.Log ("Reset if dead!");
		if (isAlive ()) {
			return;
		}
		dead = false;
		Heal (100);
		animator.SetTrigger ("reborn");
		gameOverSign.SetActive (false);
	}
}
