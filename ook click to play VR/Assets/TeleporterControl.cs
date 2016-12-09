using UnityEngine;
using System.Collections;

public class TeleporterControl : MonoBehaviour {
  public Camera camera;
  public Transform vrCameraRig;
  public float teleportTime = 0.15f;

  private float ellapsed;
  private bool teleporting;
  private Vector3 start;
  private Vector3 destination;

  // Use this for initialization
  void Start() {
    teleporting = false;
  }

  // Update is called once per frame
  void Update() {
    if (teleporting) {
      ellapsed += Time.deltaTime;
      float fraction = ellapsed / teleportTime;
      if (fraction > 1.0f) {
        teleporting = false;
        ellapsed = 0;
      } else {
        Vector3 pos = Vector3.Lerp(start, destination, fraction);
        vrCameraRig.transform.position = pos;
      }
    }
  }

  public void Teleport(Vector3 location) {
    float y = vrCameraRig.transform.position.y;
    start = vrCameraRig.transform.position;
    destination = new Vector3(location.x, y, location.z);
    ellapsed = 0;
    teleporting = true;
  }
}