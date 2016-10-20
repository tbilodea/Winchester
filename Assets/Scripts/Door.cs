using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//Purpose: Handles the door's key, locked, and timing behaviors
//Called by Interaction class, which determines what type of interaction occurs
public class Door : MonoBehaviour {

    public bool doorLockedForever; //door open-able variables
    public bool doorLocked; 
    public GameObject key;

    [SerializeField] private AudioClip openingDoor; //set sounds
    [SerializeField] private AudioClip closingDoor;

    [SerializeField] private TextAsset doorLockedMessage; //messages for if the door won't open
    [SerializeField] private TextAsset doorLockedForeverMessage;

    public bool doorState;

    public bool doorOnTimer; //door on timer variables
    private bool doorIsMoving; 
    [SerializeField] private float _timerForDoor;

    [SerializeField] private bool doorWarpNextLevel;
    private Transform _doorOpen;
    private Animator _doorAnim;     
    private AudioSource _audioSource; 
    
    //Initialization
	void Start () {
        _doorAnim = gameObject.GetComponent<Animator>(); //sets animator and audio components
        _audioSource = gameObject.GetComponent<AudioSource>();
	}
	
    //manages door
    public void RunDoor()
    {
        

        if (doorOnTimer && !doorIsMoving)
        {
            if (!doorLockedForever)
            {
                if (!doorLocked)
                {
                    StartCoroutine(TimedCloseDoor());
                }else
                {
                    TextboxManager.aTextboxManager.loadScript(doorLockedMessage, null); //static singleton instance manages message
                }
            }else
            {
                TextboxManager.aTextboxManager.loadScript(doorLockedForeverMessage, null);
            }
        }else if(!doorOnTimer)
        {
            if (doorState)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }
    //Purpose: opens the door and activates sound
    public void OpenDoor()
    {
        if (!doorLockedForever)
        {
            if (!doorLocked) 
            {
                if (doorWarpNextLevel)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                }
                //play animation opendoor
                _doorAnim.SetTrigger("openDoor");
                //do creak sound
                _audioSource.clip = openingDoor;
                _audioSource.Play();
                //move actual door
                gameObject.transform.position = new Vector3(0.678f, 1.293f, 0.563f); //predetermined position and rotation that is "open"
                gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90, -90, 0)); //-90,0,-90 for x,y,z but quaternion does z,x,y

                if (!doorOnTimer)
                {
                    doorState = true; //door is now open (flag for Interaction class)
                }
            }
            else //if door is locked, do a message that needs a key
            {
                TextboxManager.aTextboxManager.loadScript(doorLockedMessage, null); //static singleton instance manages message
            }
        }
        else //message door is permanently locked
        {
            TextboxManager.aTextboxManager.loadScript(doorLockedForeverMessage, null);
        }
    }

    //Same function as OpenDoor() except to close the door
    public void CloseDoor()
    {
        /*if (!doorLockedForever) 
        {
            if (!doorLocked)
            {*/
                //play animation closedoor
                _doorAnim.SetTrigger("closeDoor");
                //do other creak sound
                _audioSource.clip = closingDoor;
                _audioSource.Play();
                //move the door back to origin
                gameObject.transform.position = new Vector3(0, 0, 0);
                gameObject.transform.rotation = Quaternion.identity;
                doorState = false; //door is now closed
            /*}
        }*/
    }

    //Purpose: Coroutine for when the door is on a timer
    //Output: a door that opens and closes and isn't interactable
    private IEnumerator TimedCloseDoor()
    {
        if (!doorLockedForever)
        {
            if (!doorLocked)
            {
                doorIsMoving = true;
                OpenDoor();
                yield return new WaitForSeconds(_timerForDoor); //wait timer seconds
                CloseDoor();
                yield return new WaitForSeconds(1f); //waiting for animation to finish (will be 1 second in this case)
                doorIsMoving = false;
            }
        }
    }

    //Purpose: Handles if the door is locked and needs a key
    //Output: Destroys key and unlocks door
    public void OnTriggerEnter(Collider theObject)
    {
                if (theObject.gameObject == key)
                {
                    doorLocked = false;
                    FirstPersonController.aFirstPersonController.removeFromHand(); //removes the key from our hand
                    theObject.gameObject.SetActive(false);
                }
    }
}
