using UnityEngine;
using System.Collections;

public class OokIdle : MonoBehaviour {
  Animator animator;
  float actionWait = 5.0f;

  public float waitMin = 4.0f;
  public float waitMax = 8.0f;
	// Use this for initialization
	void Start () {
    Random.seed = (int)System.DateTime.Now.Ticks;
    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
  void Update ()
  {
    if (actionWait == 0) {
      switch (Random.Range(0, 6)) {
        case 0:
          animator.SetTrigger ("attack");
          break;
        case 1:
        case 2:
          animator.SetTrigger ("wave");
          break;
        default:
          animator.SetTrigger ("scratch");
          break;
      }
      actionWait = Random.Range(waitMin, waitMax);
    }

    animator.SetBool ("walking", false);
    actionWait = Mathf.Max (0, actionWait - Time.deltaTime);
  }
}
