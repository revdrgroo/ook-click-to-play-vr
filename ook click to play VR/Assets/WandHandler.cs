using UnityEngine;
using System.Collections;

public class WandHandler : MonoBehaviour {
  public enum Action { OOK, TELEPORT };

	public SteamVR_TrackedController trackedController;
	public InputManager inputManager;
  public Action action;

	private bool pressed;

	// Use this for initialization
	void Start () {
//		trackedController = GetComponent<SteamVR_TrackedController> ();
		Debug.Log ("controller = " + trackedController);
		trackedController.TriggerClicked += Device_TriggerClicked;
		trackedController.TriggerUnclicked += Device_TriggerUnclicked;
		trackedController.PadClicked += Device_PadClicked;
		pressed = false;
	}

	void Device_PadClicked(object sender, ClickedEventArgs e) {
		inputManager.HandleResetButton();
	}

	void Device_TriggerClicked (object sender, ClickedEventArgs e)
	{
		Debug.Log ("pressed");
		transform.Rotate (0, 10, 0);
		pressed = true;
	}

	void Device_TriggerUnclicked(object sender, ClickedEventArgs e) {
		Debug.Log ("released");
		pressed = false;
	}

	void Update ()
	{
    Ray ray = new Ray(transform.position, transform.up);
		Debug.DrawRay (transform.position, transform.up, Color.red);
		switch(action) {
		case Action.OOK:
			inputManager.HandleOokCursorRay (ray, pressed);
			break;
		case Action.TELEPORT:
			inputManager.HandleTeleportCursorRay (ray, pressed);
			break;
		}
	}
}
