using UnityEngine;
using System.Collections;

public class LockDoorBehind : MonoBehaviour {
    [SerializeField] private GameObject lockThisDoor;
    [SerializeField] private bool lockedForever;

	void OnTriggerEnter()
    {
        Door aDoor = lockThisDoor.GetComponent<Door>();
        Debug.Log("OnTriggerActivated");
        if (lockedForever)
        {
            Debug.Log("Door locked forever");
            
            aDoor.doorLockedForever = true;

            if (aDoor.doorState)
            {
                aDoor.CloseDoor();
            }
        }
        else
        {
            aDoor.doorLocked = true;
            if (aDoor.doorState)
            {
                aDoor.CloseDoor();
            }
        }
    }
}
