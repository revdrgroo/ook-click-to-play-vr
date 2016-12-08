using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//[ExecuteInEditMode]
public class FishTriggerScript : MonoBehaviour {
  public GameObject ook;
  public GameObject hpText;

  private ParticleSystem ps;
  private GameObject currentText;
  private int currentCount = 0;
  // these lists are used to contain the particles which match
  // the trigger conditions each frame.
  List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
  List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
  List<ParticleCollisionEvent> collisions = new List<ParticleCollisionEvent>();

  private const float HIT_THRESHOLD = 30.0f;

  void OnEnable() {
    ps = GetComponent<ParticleSystem>();
  }

  private void OnParticleCollision(GameObject other) {
    if (other != ook) {
      return;
    }
    ps.GetCollisionEvents(ook, collisions);
    int count = 0;
    foreach (ParticleCollisionEvent ev in collisions) {
      //Debug.Log("ev" + ev.velocity.sqrMagnitude);
      if (ev.velocity.sqrMagnitude > HIT_THRESHOLD) {
        count++;
      }
    }
    Debug.Log("collisions count = " + count);
    if (count == 0) {
      return;
    }
    if (currentText == null || currentText.activeInHierarchy == false) {
      currentText = (GameObject)Instantiate(hpText, ook.transform.position, ook.transform.rotation);
      currentCount = count;
    } else {
      currentCount = Math.Min(count + currentCount, 9);
    }
    currentText.SetActive(true);
    TextMesh text = currentText.GetComponent<TextMesh>();
    text.text = string.Format("ouch! {0}", currentCount);
    //collisions.Clear();

  }

  void OnParticleTrigger() {
    // get the particles which matched the trigger conditions this frame
    int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);

    Debug.Log("FishTriggerScript: OnParticleEnter: enter = " + enter.Count);
    // iterate through the particles which entered the trigger and make them red
    for (int i = 0; i < numEnter; i++) {
      ParticleSystem.Particle p = enter[i];
      p.startColor = new Color32(255, 0, 0, 255);
      enter[i] = p;
    }

    // iterate through the particles which exited the trigger and make them green
    for (int i = 0; i < numExit; i++) {
      ParticleSystem.Particle p = exit[i];
      p.startColor = new Color32(0, 255, 0, 255);
      exit[i] = p;
    }

    // re-assign the modified particles back into the particle system
    ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
  }
}