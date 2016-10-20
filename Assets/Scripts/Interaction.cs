using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour {
    //types of interactions: talking - instigates textbox (0)
    //                       movable - attaches to gameobject "hand" (1)
    //                       inventory - puts object in the inventory (2)
    //                      door-interaction (3)
    //                      warp gate to another area (4)
    //                      warp gate to another level (5)
    //                      does nothing so the flag is just the interactionFinished (6)
    [SerializeField]private int _interactType;
    [SerializeField]private TextAsset _dialogueToShow; //input textAsset
    [SerializeField]private bool _destroyAfterActivated;
    [SerializeField]private GameObject _warpToPoint;
    [SerializeField]private int levelLoad;
    [SerializeField]private bool _doOnce;

    private bool _onceCompleted;
    private bool _objectInHand;
    private AudioSource _audioSource;
    public bool _interactionFinished;
    private bool warping;
    
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void interactionAction()
    {
        if (_onceCompleted)
        {
            return;
        }
        if (_doOnce)
        {
            _onceCompleted = true;
        }
        switch (_interactType)
        {
            case 0: //talker
                if (_dialogueToShow)
                {//script to textbox
                    TextboxManager.aTextboxManager.loadScript(_dialogueToShow, this);
                    if (_destroyAfterActivated)
                    {
                        Destroy(gameObject.GetComponent<Interaction>()); //destroys the interaction component of the object
                    }
                }
                else
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
                thisDoor.RunDoor();
                _interactionFinished = true;
                break;
            case 4:
                if (!warping)
                {
                    StartCoroutine(WarpStart());
                }
                break;
            case 5: //warp gate
                Debug.Log("Loading new scene");
                if (!warping)
                {
                    StartCoroutine(LevelWarpStart());
                }

                break;
            case 6:
                _interactionFinished = true;
                break;
            case 7://cutscene
                GetComponent<Cutscene>().cutscene();
                break;
        }
        
    }
    
    //Makes a sound on a collision
    void OnCollisionEnter(Collision thisCollded)
    {
        if(_interactType == 1)
        {
            if (_audioSource)
            {
                _audioSource.Play();
            }
        }
    }

    //sets interaction type to something new
    public void setInteractionType(params System.Object[] values)
    {
        int newType = (int)values[0];
        _interactType = newType;
        Debug.Log(newType);
        switch (newType)
        {
            case 5:
                levelLoad = (int)values[1];
                break;
        }
    }

    private IEnumerator WarpStart()
    {
        warping = true;
        yield return StartCoroutine(Fade.aFade.FadetoBlack());
        FirstPersonController.aFirstPersonController.gameObject.transform.position = _warpToPoint.transform.position;
        yield return StartCoroutine(Fade.aFade.FadetoClear());
        warping = false;
    }

    private IEnumerator LevelWarpStart()
    {
        warping = true;
        yield return StartCoroutine(Fade.aFade.FadetoBlack());
        SceneManager.LoadScene(levelLoad);
        warping = false;
    }
}
