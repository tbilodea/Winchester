using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Interaction : MonoBehaviour {
    //types of interactions: talking - instigates textbox (0)
    //                       movable - attaches to gameobject "hand" (1)
    //                       inventory - puts object in the inventory (2)
    //                      door-interaction
    [SerializeField]private int _interactType;
    [SerializeField]private TextAsset _dialogueToShow; //input textAsset
    [SerializeField]private bool _destroyAfterActivated;

    private bool _objectInHand;

	// Update is called once per frame
	void Update () {
	
	}

    public void interactionAction()
    {
        switch (_interactType)
        {
            case 0: //talker
                if (_dialogueToShow)
                {//script to textbox
                    TextboxManager.aTextboxManager.loadScript(_dialogueToShow);
                    if (_destroyAfterActivated)
                    {
                        Destroy(gameObject.GetComponent<Interaction>()); //destroys the interaction component of the object
                    }
                }else
                {
                    Debug.LogError("No textasset to load! Attach textasset and retry.");
                }
                break;
            case 1:
                  FirstPersonController.aFirstPersonController.setIntoHand(gameObject); //set object in hand or remove 
                break;
            case 2:
                //put object into inventory
                break;
            case 3: //this is a door
                //animation and grab change as well as sound
                Door thisDoor = gameObject.GetComponent<Door>();
                if (!thisDoor.doorOnTimer)
                {
                    if (thisDoor.doorState) //if the door is open
                    {
                        thisDoor.CloseDoor(); //close
                    }
                    else //if closed
                    {
                        thisDoor.OpenDoor(); //open
                    }
                }else //if on timer
                {
                    if (!thisDoor.doorIsMoving) //and the door isn't moving on it's timer already
                    {
                        StartCoroutine(thisDoor.TimedCloseDoor()); //go through the timer on the door
                    }
                }
                break;
        }
    }
}
