using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UiController : MonoBehaviour {
  public string menuLevel = "intro";
  public string firstLevel = "1";
  public float footerTime = 1.5f;

  public Text punataText;
  public Text scoreText;
  public Text footerText;

  int punataCount;
  int score;
  float footerTimer;

	// Use this for initialization
	void Start () {
    punataCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
    if (footerTimer > 0) {
      footerTimer = Mathf.Max(0, footerTimer - Time.deltaTime);
      if (footerTimer == 0) {
        footerText.text = "";
      }
    }
	}

  public void OnClickMainMenu() {
    Application.LoadLevel(menuLevel);
  }

  public void OnClickQuit() {
    Debug.Log("Quitting!");
    Application.Quit();
  }

  public void OnClickStart() {
    Application.LoadLevel(firstLevel);
  }

  public void RecordPunata() {
    punataCount++;
    if (punataText != null) {
      punataText.text = "Puñatas: " + punataCount;
    }
  }

  public void RecordScore(int add) {
    score += add;
    if (scoreText != null) {
      scoreText.text = "Score: " + score;
    }
  }

  public void SetFooter(string footer) {
    footerText.text = footer;
    footerTimer = footerTime;
  }
}

