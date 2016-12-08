using UnityEngine;
using System.Collections;

public class Jewel : MonoBehaviour {
  public GameObject player;
  public GameObject particles;
  public GameObject jewel;
  public GameObject textBubble;
  public float sparkleTimerMax = 0.7f;
  public ScoreBoard scoreBoard;
  public string pun;

  bool sparkling = false;
  float sparkleTimer = 0.0f;

  bool alive;

	// Use this for initialization
	void Start () {
    alive = true;
    pun = "pun pun pun...";
	}
	
	// Update is called once per frame
	void Update () {
    sparkleTimer = Mathf.Max(0, sparkleTimer - Time.deltaTime);
    if (sparkling && sparkleTimer == 0.0f) {
      Destroy(gameObject);
    }
	}

  void OnTriggerEnter (Collider other)
  {
    if (!alive) { return; }

    if(other.gameObject == player)
    {
      Debug.Log("player collided: pun = " + pun);
      alive = false;
      scoreBoard.addScore(10);
      scoreBoard.addPun(pun);
      Destroy(jewel);
      particles.SetActive(true);
      sparkleTimer = sparkleTimerMax;
      sparkling = true;
      GameObject obj = (GameObject)Instantiate(textBubble, transform.position, transform.rotation);
      obj.SetActive(true);
      TextMesh text = obj.GetComponent<TextMesh>();
      text.text = string.Format(pun);
    }
  }

  public void SetPun(string pun) {
    Debug.Log("Setting pun: " + pun);
    this.pun = pun;
  }
}
