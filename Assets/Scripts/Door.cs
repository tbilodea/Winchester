using UnityEngine;
using System.Collections;

//handles the door's key, locked, and timing behaviors
public class Door : MonoBehaviour {

    public bool doorLockedForever; //stops door from being opened again
    public bool doorLocked; //this door needs a key
    public GameObject key; //the key that opens to this door
    public AudioClip openingDoor; //set sounds
    public AudioClip closingDoor;
    public bool isOpen; //this is what state the door currently in
    public TextAsset doorLockedMessage;
    public TextAsset doorLockedForeverMessage;
    public bool doorState; //tells if the door is opened or closed

    public bool doorOnTimer; //if door on timer
    public bool doorIsMoving; //flags if the door is in the coroutine
    [SerializeField] private float _timerForDoor; //timer

    private Transform _doorOpen; //used to move the door
    private Animator _doorAnim;     //animator
    private AudioSource _audioSource; //audio source for creaking


    //Initialization
	void Start () {
        _doorAnim = gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
	}
	
    //opens the door and activates sound
    public void OpenDoor()
    {
        if (!doorLockedForever) //check if door is locked forever
        {
            if (!doorLocked) //check if the door is locked
            {
                //play animation opendoor
                _doorAnim.SetTrigger("openDoor");
                //do creak sound
                _audioSource.clip = openingDoor;
                _audioSource.Play();
                //move actual door
                gameObject.transform.position = new Vector3(0.678f, 1.293f, 0.563f); //predetermined position and rotation that is "open"
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90, -90, 0)); //-90,0,-90 : x,y,z but quaternion does z,x,y

                if (doorOnTimer) //if door is on a timer
                {//wait and then close the door
                   // StartCoroutine(TimedCloseDoor());
                }else //if not on a timer
                {
                    doorState = true; //door is now open
                }
            }
            else //if door is locked, do a message that needs a key
            {
                TextboxManager.aTextboxManager.loadScript(doorLockedMessage);
            }
        }else
        {
            //message door is permanently locked
            TextboxManager.aTextboxManager.loadScript(doorLockedForeverMessage);
        }
    }

    //closes the door and activates the other sound
    public void CloseDoor()
    {
        if (!doorLockedForever) //check again if door is locked
        {
            if (!doorLocked)
            {
                //play animation closedoor
                _doorAnim.SetTrigger("closeDoor");
                //do other creak sound
                _audioSource.clip = closingDoor;
                _audioSource.Play();
                //move the door back to origin
                gameObject.transform.position = new Vector3(0, 0, 0);
                gameObject.transform.rotation = Quaternion.identity;
                doorState = false; //door is now closed
            }
        }
    }

    //coroutine for when the door is on a timer
    public IEnumerator TimedCloseDoor()
    {
        doorIsMoving = true;
        OpenDoor();
        yield return new WaitForSeconds(_timerForDoor); //wait timer seconds
        CloseDoor(); //now activate close door
        yield return new WaitForSeconds(1f); //waiting for animation to finish
        doorIsMoving = false;

    }



    //handles if the door is locked and needs a key
    public void OnTriggerEnter(Collider theObject)
    {

                Debug.Log("key is destroyed");
                if (theObject.gameObject == key) //if the key is next to our door
                {
                    doorLocked = false; //allow player to open the door
                    FirstPersonController.aFirstPersonController.removeFromHand(); //removes the key from our hand
                    theObject.gameObject.SetActive(false); //destroy the key
                }
        
    }
}
