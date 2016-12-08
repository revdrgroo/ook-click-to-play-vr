using UnityEngine;
using System.Collections;

public class TeleporterControl : MonoBehaviour {
  public Camera camera;
  public Transform vrCameraRig;

  // Use this for initialization
  void Start() {
  }

  // Update is called once per frame
  void Update() {
  }

  public void Teleport(Vector3 location) {
    float y = vrCameraRig.transform.position.y;
    vrCameraRig.transform.position = new Vector3(location.x, y, location.z);
  }
}