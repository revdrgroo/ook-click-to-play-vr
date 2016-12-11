using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class BarryAnimation: MonoBehaviour {
  public bool moving { get; set; }
  Rigidbody parentRigidbody;

  void Start() {
    moving = true;
    parentRigidbody = GetComponentInParent<Rigidbody>();
  }

  void OnAnimatorMove() {
    Animator animator = GetComponent<Animator>();

    if (animator && moving) {
      //Debug.Log("Root motion = " + animator.GetFloat("RootMotion"));
      Vector3 delta = transform.right * animator.GetFloat("RootMotion") *
        animator.GetFloat("Speed") * Time.deltaTime;
      Vector3 newPosition = parentRigidbody.transform.position - delta;
      parentRigidbody.MovePosition(newPosition);
    }
  }
  
}