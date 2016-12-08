using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class BarryAnimation: MonoBehaviour {

  void OnAnimatorMove() {
    Animator animator = GetComponent<Animator>();

    if (animator) {
      //Debug.Log("Root motion = " + animator.GetFloat("RootMotion"));
      Vector3 delta = transform.right * animator.GetFloat("RootMotion") * Time.deltaTime;
      Vector3 newPosition = transform.position - delta;
      transform.position = newPosition;
    }
  }
  
}