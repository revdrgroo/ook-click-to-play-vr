using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {
  public TextMesh scoreText;
  public TextMesh punText;

  private int score = 0;
  void Start () {
    scoreText.text = "0 Poinxxxts";
    punText.text = "pun pun pun...";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void addScore(int points) {
    score += points;
    scoreText.text = string.Format("{0} Points", score);
  }

  public void addPun(string pun) {
    punText.text = pun;
  }
}
