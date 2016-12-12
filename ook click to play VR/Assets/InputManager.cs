using UnityEngine;
using System.Collections;
using System;

public class InputManager : MonoBehaviour {
  public Camera worldCamera;
  public TeleporterControl teleporter;
  public OokMovement ookMovement;
  public GameObject ookCursor;
  public GameObject teleportCursor;
  public bool useMouse = false;
  public Bounds vrBounds = new Bounds(Vector3.zero, new Vector3(12, 1, 16));
  public Bounds ookBounds = new Bounds(Vector3.zero, new Vector3(20, 1, 21));

  private bool ookWasPressed = false;
  private bool teleportWasPressed = false;
  private bool wasShift = false;
  // Use this for initialization
  void Start() {
    teleporter = GetComponent<TeleporterControl>();
    ookCursor.SetActive(false);
    teleportCursor.SetActive(false);
  }

  // Update is called once per frame
  void Update() {
		if (useMouse) {
			HandleMouse ();
		}
  }

  private Ray GetMouseRay() {
    Vector3 pos = Input.mousePosition;
    pos.x = pos.x / Screen.width;
    pos.y = pos.y / Screen.height;
    //      Debug.Log("mouse position = " + pos);
    //      Ray ray = camera.ViewportPointToRay(Input.mousePosition);
    pos.z = worldCamera.nearClipPlane;
    Vector3 point = worldCamera.ViewportToWorldPoint(pos);
    Vector3 direction = (point - worldCamera.transform.position).normalized;
    return new Ray(point, direction);
  }

  private bool GetMouseIntersect(out Ray ray, out Vector3 intersect) {
    ray = GetMouseRay();
    return GetRayIntersect(ray, out intersect);
 }

  private bool GetRayIntersect(Ray ray, out Vector3 intersect) {
    //Debug.DrawRay(ray.origin, ray.direction, Color.blue);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 10000000.0f, 1 << 4)) {
      intersect = hit.point;
      return true;
    } else {
      intersect = Vector3.zero;
      return false;
    }
  }

  void HandleMouse() {
    Ray ray;
    Vector3 intersect;
    if (!GetMouseIntersect(out ray, out intersect)) {
      return;
    }
    bool shift = Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
    bool pressed = Input.GetMouseButton(0);
    if (shift && !wasShift) {
      HandleOokCursorRay(ray, false);
    } else if (!shift && wasShift) {
      HandleTeleportCursorRay(ray, false);
    }
    if (shift) {
      HandleTeleportCursorRay(ray, pressed);
    } else {
      HandleOokCursorRay(ray, pressed);
    }
    wasShift = shift;
  }

  public void HandleOokCursorRay(Ray ray, bool pressed) {
    Vector3 intersect;
    GetRayIntersect(ray, out intersect);
    HandleOokCursorIntersect(intersect, pressed);
  }

  public void HandleTeleportCursorRay(Ray ray, bool pressed) {
		//Debug.Log ("handle teleport ray " + ray + ", " + pressed);
		 Vector3 intersect;
    GetRayIntersect(ray, out intersect);
    HandleTeleportCursorIntersect(intersect, pressed);
  }

  private void HandleTeleportCursorIntersect(Vector3 intersect, bool pressed) {
    Vector3 fixedIntersect;
    ValidateIntersect3D(intersect, vrBounds, out fixedIntersect);
    bool released = teleportWasPressed && !pressed;
    teleportWasPressed = pressed;

    if (pressed) {
      teleportCursor.transform.position = fixedIntersect;
    }
    teleportCursor.SetActive(pressed);

    if (released) {
      teleporter.Teleport(fixedIntersect);
    } 
  }

  private void HandleOokCursorIntersect(Vector3 intersect, bool pressed) {
    Vector3 fixedIntersect = Vector3.zero;
    ValidateIntersect3D(intersect, ookBounds, out fixedIntersect);
    bool released = ookWasPressed && !pressed;
    ookWasPressed = pressed;
  
    if (pressed) {
      ookCursor.transform.position = fixedIntersect;
    }
    ookCursor.SetActive(pressed);

    if (pressed) {
      ookMovement.SetTarget(fixedIntersect);
    } else {
      ookMovement.ClearTarget();
    }
  }

  private bool ValidateIntersect(Vector3 intersect, Rect bounds) {
    Vector2 int2 = new Vector2(intersect.x, intersect.z);
    //Debug.Log("validateIntersect: " + int2 + "," + bounds);
    return bounds.Contains(int2);
  }

  private bool ValidateIntersect3D(Vector3 intersect, Bounds bounds, out Vector3 repaired) {
    if (bounds.Contains(intersect)) {
      repaired = intersect;
      return true;
    }
    Vector3 int3 = new Vector3(intersect.x, 0, intersect.z);
    Vector3 cam3 = new Vector3(worldCamera.transform.position.x, 0, worldCamera.transform.position.z);
    Ray ray = new Ray(int3, cam3 - int3);
    float distance;
    if (bounds.IntersectRay(ray, out distance)) {
      repaired = ray.GetPoint(distance);
      repaired.y = intersect.y;
      return false;
    }
    repaired = Vector3.zero;
    return false;
  }
		
	public void HandleResetButton() {
		Debug.Log ("Reset button pressed");
		ookMovement.ResetIfDead();
		Debug.Log ("Reset button pressed done");
	}
}

