﻿using UnityEngine;
using System.Collections;
using System;

public class InputManager : MonoBehaviour {
  public Camera worldCamera;
  public TeleporterControl teleporter;
  public OokMovement ookMovement;
  public GameObject ookCursor;
  public GameObject teleportCursor;
  public bool useMouse = false;

  private bool ookWasPressed = false;
  private bool teleportWasPressed = false;
  private bool wasShift = false;
  private Rect vrBounds = new Rect(-6f, -8f, 12f, 16f);
  private Rect ookBounds = new Rect (-10f, -10f, 20f, 20f);
  private Vector3 lastValidOokIntersect = Vector3.zero;
  private Vector3 lastValidTeleportIntersect = Vector3.zero;
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
    bool shift = Input.GetKey(KeyCode.RightShift);
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
		Debug.Log ("handle teleport ray " + ray + ", " + pressed);
		 Vector3 intersect;
    GetRayIntersect(ray, out intersect);
    HandleTeleportCursorIntersect(intersect, pressed);
  }

  private void HandleTeleportCursorIntersect(Vector3 intersect, bool pressed) {
		Debug.Log ("handle teleport intersect " + intersect + ", " + pressed);
	if (validateIntersect(intersect, vrBounds)) {
      lastValidTeleportIntersect = intersect;
    }
    bool released = teleportWasPressed && !pressed;
    teleportWasPressed = pressed;

    if (pressed) {
      teleportCursor.transform.position = lastValidTeleportIntersect;
    }
    teleportCursor.SetActive(pressed);

    if (released) {
      teleporter.Teleport(lastValidTeleportIntersect);
    } 
  }

  private void HandleOokCursorIntersect(Vector3 intersect, bool pressed) {
    if (validateIntersect(intersect, ookBounds)) {
      lastValidOokIntersect = intersect;
    }
    bool released = ookWasPressed && !pressed;
    ookWasPressed = pressed;
  
    if (pressed) {
      ookCursor.transform.position = lastValidOokIntersect;
    }
    ookCursor.SetActive(pressed);

    if (pressed) {
      ookMovement.SetTarget(lastValidOokIntersect);
    } else {
      ookMovement.ClearTarget();
    }
  }

  private bool validateIntersect(Vector3 intersect, Rect bounds) {
    Vector2 int2 = new Vector2(intersect.x, intersect.z);
    //Debug.Log("validateIntersect: " + int2 + "," + bounds);
    return bounds.Contains(int2);
  }
}

