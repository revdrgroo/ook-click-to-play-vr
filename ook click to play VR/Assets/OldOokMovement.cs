using UnityEngine;
using System.Collections;

public class OldOokMovement : MonoBehaviour {
  public float velocity = 1.1f;
  public float turnSpeed = 1.0f;
  public float turnTowardsSpeed = 3.0f;
  public float scratchMin = 5.0f;
  public float attackMin = 2.0f;
  public Camera camera;
  public GameObject cursor;
  public float minCursorDelta = 0.5f;

  bool walking;
  bool turning;
  bool waving;

  bool tryToHit;
  float scratchDelay = 0;
  float attackDelay = 0;
  float startY = 0.0f;
  public GameObject enemy = null;
  Rigidbody ookRigidbody;
  Vector3 previousPos;
  Animator animator;
	// Use this for initialization
	void Start () {
    Random.seed = (int)System.DateTime.Now.Ticks;
    animator = GetComponent<Animator>();
    animator.SetBool("walking", false);
    startY = transform.position.y;
    ookRigidbody = GetComponent<Rigidbody>();
    previousPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
 //   walking = false;
 //   tryToHit = false;
//    turning = false;

    HandleKeys();
    HandleMouse();

    if (walking) {
      waving = false;
    }

    if (tryToHit) {
//      Debug.Log("Space: attackDelay = " + attackDelay);
      if (attackDelay == 0 && enemy != null) {
        Attack();
      }
    }

    if (IsWalking()) {
//      transform.Translate(Vector3.forward * velocity * Time.deltaTime);
      Vector3 newPos = ookRigidbody.position + transform.forward * velocity * Time.deltaTime;
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
	if (TurnTowards(camera.transform.position)) {//transform.position + new Vector3(1.8f, 0, -3))) {
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
    if (scratchDelay == 0 && Random.Range(0, 100) == 0) {
      if (Random.Range(0,2) == 0) {
        animator.SetTrigger("scratch");
      } else {
        waving = true;
      }
      scratchDelay = scratchMin;
    }
  }

  void HandleMouse() {
    if (Input.GetMouseButton(0)) {
      Vector3 pos = Input.mousePosition;
      pos.x = pos.x / Screen.width;
      pos.y = pos.y / Screen.height;
//      Debug.Log("mouse position = " + pos);
//      Ray ray = camera.ViewportPointToRay(Input.mousePosition);
      pos.z = 0.1f;
      Vector3 point = camera.ViewportToWorldPoint(pos);
      Ray ray = new Ray(point, camera.transform.forward);
//      Debug.Log("curor ray = " + ray);
      RaycastHit hit;
      Physics.Raycast(ray, out hit, 10000000.0f, 1 << 4);
      cursor.transform.position = hit.point + Vector3.up * 0.02f;
//      Debug.Log("cursor hit " + hit.collider + " at position = " + hit.point);
      Vector3 direction = transform.InverseTransformPoint(hit.point);
      if (direction.magnitude > minCursorDelta) {
			//transform.LookAt(hit.point, Vector3.up);
			SetTarget (hit.point);

      }
    } else {
       cursor.SetActive(false);
    }
  }

	public void SetTarget(Vector3 target) {
		cursor.SetActive(true);
		walking = TurnTowards(target);
		turning = !walking;
		//        walking = true;
		tryToHit = true;
	}

	public void ClearTarget() {
		walking = false;
		turning = false;
		tryToHit = false;
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

  void OnTriggerExit(Collider other)
  {
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

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Unicorn") {
      SetCurrentEnemy(other.gameObject);
      Debug.Log("setting enemy: " + enemy);
      UnicornHealth uh = enemy.GetComponent<UnicornHealth>();
      uh.InRange(true);
    }
  }

  bool TurnTowards(Vector3 target) {
    Vector3 targetDir = (target - transform.position);
    targetDir.y = 0;
	targetDir.Normalize ();
//    Debug.Log("targetDir = " + targetDir);

    float step = turnTowardsSpeed * Time.deltaTime;
    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
    Debug.DrawRay(transform.position, newDir, Color.red);
    Quaternion rot =  Quaternion.LookRotation(newDir);
    transform.rotation = rot;
//    ookRigidbody.MoveRotation(rot);
//    ookRigidbody.isKinematic = true;
//    ookRigidbody.rotation = rot;
//    ookRigidbody.isKinematic = false;

//    Debug.Log("rot = " + rot.eulerAngles + " transform.rotation = " + transform.rotation.eulerAngles);
		return (Vector3.Dot(transform.forward, targetDir) > 0.99f);

  }
}
