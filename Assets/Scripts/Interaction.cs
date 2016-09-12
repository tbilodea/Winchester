using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Interaction : MonoBehaviour {
    //types of interactions: talking - instigates textbox (0)
    //                       movable - attaches to gameobject "hand" (1)
    //                       inventory - puts object in the inventory (2)
    [SerializeField]private int _interactType;
    [SerializeField]private TextAsset _dialogueToShow; //input textAsset
    [SerializeField]private bool _destroyAfterActivated;

    private bool _objectInHand;

	// Use this for initialization
	void Start () {
	    
	}
	
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
            case 3:
                //put object into inventory
                break;
        }
    }
}
