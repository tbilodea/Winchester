using UnityEngine;
using System.Collections;

public class DoorChangeOnInteract : MonoBehaviour {
    public GameObject DoorObject;
    public bool CloseAndLock;
    public bool OpenAndUnlock;
    public bool Unlock;

    public bool SetOnTimer;

    private Door door;
    private bool doOnce;
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
        if (GetComponent<Interaction>()._interactionFinished)
        {
            if (door)
            {
                if (CloseAndLock)
                {
                    if (door.doorState)
                    {
                        door.RunDoor();
                    }
                    door.doorLockedForever = true;
                }
                if (OpenAndUnlock)
                {

                    door.doorLockedForever = false;
                    if (!door.doorState)
                    {
                        door.RunDoor();
                    }
                }
                if (Unlock)
                {
                    door.doorLockedForever = false;
                    door.doorLocked = false;
                }
                if (SetOnTimer)
                {
                    door.doorOnTimer = true;
                }
                
            }
            doOnce = true;
        }
	}
}
