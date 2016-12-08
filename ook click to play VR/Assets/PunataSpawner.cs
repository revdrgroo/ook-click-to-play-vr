using UnityEngine;
using System.Collections;

public class PunataSpawner : MonoBehaviour {
  public GameObject spawnSource;
  public GameObject[] spawnPoints;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


  public void Spawn() {
    int sp = Random.Range(0, spawnPoints.Length);
    Vector3 location = spawnPoints[sp].transform.position;
    Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360.0f), 0);
    GameObject obj = (GameObject) Instantiate(spawnSource, location, rot);
    obj.SetActive(true);
    unicornMovement mvt = obj.GetComponent<unicornMovement>();
    float sign = ((Random.Range(0, 2) == 0) ? -1 : 1);
    mvt.angularVelocity = Random.Range(20.0f, 40.0f) * sign;
  }
}
