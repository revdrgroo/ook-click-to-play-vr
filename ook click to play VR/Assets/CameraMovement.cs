using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
  private Quaternion myRotation;

  // Use this for initialization
  void Start () {
    myRotation = transform.rotation;

  }

  void LateUpdate() {
    transform.rotation = myRotation;
  }
}
