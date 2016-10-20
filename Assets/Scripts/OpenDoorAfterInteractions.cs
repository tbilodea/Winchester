using UnityEngine;
using System.Collections;

public class OpenDoorAfterInteractions : MonoBehaviour {
    public GameObject[] interactables;
    public GameObject DoorObject;
    public bool OpenDoorAfter;

    public bool enableDisableObjects;

    private Door door;
    private bool flagTrue;
    private bool doOnce;
    private bool allInteracted;
	// Use this for initialization
	void Start () {
        door = DoorObject.GetComponent<Door>();
	}
	
	// Update is called once per frame
	void Update () {
        if (doOnce)
        {
            return;
        }

        foreach(GameObject interact in interactables)
        {
            if (!interact.GetComponent<Interaction>()._interactionFinished)
            {
                allInteracted = false;
            }
        }

        if (allInteracted)
        {
            door.doorLocked = false;
            door.doorLockedForever = false;
            if (OpenDoorAfter)
            {
                door.RunDoor();
            }
            if (enableDisableObjects)
            {
                GetComponent<OnInteractionDisableEnableObjects>().enableDisable();
            }
            doOnce = true;
        }

        allInteracted = true;
	}
}
