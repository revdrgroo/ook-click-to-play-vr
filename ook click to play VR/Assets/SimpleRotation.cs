using UnityEngine;
using System.Collections;

public class SimpleRotation : MonoBehaviour {
  public enum Axis { X, Y, Z };

  public Axis axis = Axis.Y;
  public float rotationSpeed = 30;
  
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    switch (axis) {
      case Axis.X:
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
        break;
      case Axis.Y:
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        break;
      case Axis.Z:
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        break;
    }
  }
}
