using UnityEngine;
using System.Collections;

public class DisplayOokHealth : MonoBehaviour {
  public GameObject healthBar;
  public GameObject hurtBar;
  public OokMovement ook;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    float health = ook.getHealthFraction();
    hurtBar.transform.localScale = new Vector3(1 - health, 1.0f, 1.0f);
    healthBar.transform.localScale = new Vector3(health, 1.0f, 1.0f);
  }
}
