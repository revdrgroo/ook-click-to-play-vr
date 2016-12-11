using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class BarryAnimation: MonoBehaviour {
  public bool moving { get; set; }

  void Start() {
    moving = true;
  }

  void OnAnimatorMove() {
    Animator animator = GetComponent<Animator>();

    if (animator && moving) {
      //Debug.Log("Root motion = " + animator.GetFloat("RootMotion"));
      Vector3 delta = transform.right * animator.GetFloat("RootMotion") * Time.deltaTime;
      Vector3 newPosition = transform.position - delta;
      transform.position = newPosition;
    }
  }
  
}