using UnityEngine;
using System.Collections;

public class UnicornHealth : MonoBehaviour {
  public float timerMax = 2.0f;
  public float rainbowTimerMax = 0.5f;
  public GameObject hpText;
  public GameObject statusCircle;
  public GameObject rainbow;
  public GameObject jewel;
  public float statusSpeed = 60.0f;
  public int hitPoints = 3;
  public GameObject explosion;
  public PunataSpawner spawner;
  public ScoreBoard scoreBoard;
  public float ragdollForce = 4000.0f;
  public string ragdollPath = "ragdoll";
  public AudioClip meepNoise;
  public float meepVolume = 1.0f;
  float timer = 0;
  float rainbowTimer = 0;
  int hp;
  SpeechBubble speechBubble;
  string currentPun;
  Jewel jewelInstance;
  bool jewelSet;

  string[] puns = {
    "Stop monkeying about!",
    "An unxepected ape-pearance!",
    "Well Ook At You!",
    "Are you Ooking at me!",
    "Get stuffed. Oh you are!",
    "A gift from my butt!",
    "I fly apart under pressure!",
  };

  private bool inRange = false;

  // Use this for initialization
  void Start () {
    speechBubble = GetComponentInChildren<SpeechBubble>();
    hp = hitPoints;
	}
	
	// Update is called once per frame
	void Update () {
    if (jewelInstance != null && !jewelSet) {
      jewelInstance.SetPun(currentPun);
      jewelSet = true;
    }

    timer = Mathf.Max(0, timer - Time.deltaTime);
    rainbowTimer = Mathf.Max(0, rainbowTimer - Time.deltaTime);
    if (inRange) {
      statusCircle.transform.Rotate(0, 0, statusSpeed * Time.deltaTime);
    }
    if (timer == 0 && speechBubble.enabled) {
      speechBubble.enabled = false;
    }
    if (rainbowTimer == 0 && rainbow != null && rainbow.activeInHierarchy) {
      rainbow.SetActive(false);
      GameObject obj = (GameObject)Instantiate(jewel, transform.position - 2.0f * transform.forward, Quaternion.identity);
      jewelInstance =  obj.GetComponent<Jewel>();
      jewelSet = false;
      obj.SetActive(true);
    }
	}

  public void TakeHit() {
    scoreBoard.addScore(10);
    if (timer != 0) { return; }
//    Debug.Log("Puns from my bum");
//   speechBubble.enabled = true;
    SpeechBubble bubble = speechBubble.GetComponent<SpeechBubble>();
    currentPun = NextPun();
    bubble.SetText(currentPun);
    timer = timerMax;
    rainbowTimer = rainbowTimerMax;
    if (rainbow != null) {
      rainbow.SetActive(true);
    }
    hp--;
    if (hp == 0) {
      scoreBoard.addScore(50);
      Die();
    } else {
      GameObject obj = (GameObject)Instantiate(hpText, transform.position, transform.rotation);
      obj.SetActive(true);
      TextMesh text = obj.GetComponent<TextMesh>();
      text.text = string.Format("{0}/{1}", hp, hitPoints);
//      Debug.Log("Created hp text:" + text);
    }
    AudioSource.PlayClipAtPoint(meepNoise, transform.position, meepVolume);
  }

  public void InRange(bool inRange) {
    statusCircle.SetActive(inRange);
    this.inRange = inRange;
  }

  private string NextPun() {
    int index = Random.Range(0, puns.Length);
    return puns[index];
  }

  void Die() {
    spawner.Spawn();
    GameObject obj = (GameObject)Instantiate(explosion, transform.position, transform.rotation);
    obj.SetActive(true);
    ThrowRider();
    Destroy(gameObject);
  }

  private void ThrowRider() {
    Transform rider = transform.Find(ragdollPath);
    if (rider == null) {
      return;
    }
    rider.parent = null;
    Transform pelvis = rider.Find("Pelvis");
    if (pelvis == null) {
      return;
    }
    Rigidbody rb = pelvis.gameObject.GetComponent<Rigidbody>();
    rb.isKinematic = false;
    rb.useGravity = true;
    rb.AddForce(Vector3.up * ragdollForce * rb.mass);
  }
}
