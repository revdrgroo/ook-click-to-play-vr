using UnityEngine;
using System.Collections;

public class BarryAttack : MonoBehaviour {
  public ParticleSystem wave;
  public GameObject ook;
  //public ParticleSystem fishSpray;
  public MeshCanon fishCanon;
  public float waveCooldown = 5.0f;
  public float fishCooldown = 10.0f;
  public float waveVariance = 0.5f;
  public float fishVariance = 0.5f;
  public float sqrWaveRange = 8.0f;
  public float sqrMinFishRange = 1.0f;
  public GameObject sinewave;
  public AudioSource sinewaveSound;
  

  private float waveCountdown;
  private float fishCountdown;
  private OokMovement ookMovement;
  private Animator sinewaveAnimator;


  // Use this for initialization
  void Start () {
    waveCountdown = waveCooldown;
    fishCountdown = fishCooldown;
    ookMovement = ook.GetComponent<OokMovement>();
    sinewaveAnimator = sinewave.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
	  if (waveCountdown <= 0.0f && Random.Range(0, 100) < 2) {
      waveCountdown = waveCooldown * Random.Range(1.0f, 1.0f + waveVariance);
      DoWave();
    }
    if (fishCountdown <= 0.0f && Random.Range(0, 100) < 2) {
      fishCountdown = fishCooldown * Random.Range(1.0f, 1.0f + waveVariance);
      DoFish();
    }
    waveCountdown -= Time.deltaTime;
    fishCountdown -= Time.deltaTime;
	}

  private void DoFish() {
    //fishSpray.Play(true);
    float dist = GetRange();
    if (dist > sqrMinFishRange) {
      fishCanon.Fire();
    }
  }

  private void DoWave() {
    wave.Play(true);
    sinewaveAnimator.SetTrigger("PlayWave");
    sinewaveSound.Play();
    float dist = GetRange();
    //Debug.Log("wave distance = " + dist);
    if (dist <= sqrWaveRange) {
      float intensity = (sqrWaveRange - dist + 1) / (sqrWaveRange + 1);
      ookMovement.WaveHit(intensity);
    }
  }

  private float GetRange() {
    return (ook.transform.position - transform.position).sqrMagnitude;
  }
}
