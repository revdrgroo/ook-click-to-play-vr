﻿using UnityEngine;
using System.Collections;

public class BarryMotion : MonoBehaviour {
  public float velocity = 1.1f;
  public float turnSpeed = 1.0f;
  public float turnTowardsSpeed = 3.0f;
  public GameObject ook;

  OokMovement ookMovment;
  Animator animator;
  BarryAnimation barryAnimation;

  bool seeking;

  private void Start() {
    animator = GetComponent<Animator>();
    barryAnimation = GetComponent<BarryAnimation>();
    ookMovment = ook.GetComponent<OokMovement>();
    seeking = true;
  }
 
  void Update() {
    if (seeking && !ookMovment.isAlive()) {
      barryAnimation.moving = false;
      seeking = false;
      animator.SetTrigger("Stop");
      animator.SetFloat("RootMotion", 0);
    }
    TurnTowards(ook.transform.position);
  }

  bool TurnTowards(Vector3 target) {
    Vector3 targetDir = (target - transform.position);
    targetDir.y = 0;
    targetDir.Normalize();
    //Debug.Log("barry targetDir = " + targetDir);

    float step = turnTowardsSpeed * Time.deltaTime;
    Vector3 newDir = Vector3.RotateTowards(-transform.right, targetDir, step, 0.0F);
    newDir = Quaternion.AngleAxis(90f, Vector3.up) * newDir;
    Debug.DrawRay(transform.position, newDir, Color.red);
    Quaternion rot = Quaternion.LookRotation(newDir);
    transform.rotation = rot;

    //Debug.Log("rot = " + rot.eulerAngles + " transform.rotation = " + transform.rotation.eulerAngles);
    bool finished = (Vector3.Dot(transform.forward, targetDir) > 0.99f);
    //turning = !finished;
    return finished;
  }
}