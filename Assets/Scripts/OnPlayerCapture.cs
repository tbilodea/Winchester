using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OnPlayerCapture : MonoBehaviour {
    //capture type 0 = hannover library level 1
    // type 1 = second level, we are warping levels when that happens
    //type 2 is general reset
    [SerializeField] private int captureType;
    [SerializeField] private GameObject warpPoint;
    [SerializeField] private AudioClip _onCaptureClip;
    private GameObject _player;
    [SerializeField] private GameObject _makeThisObjectActive;
    [SerializeField] private bool _runActiveObjectInteraction;

    [SerializeField] private int noCaptureType;
    [SerializeField] private GameObject _unlockDoorOnDestroy;

    [SerializeField] private bool _resetScene;
    [SerializeField] private GameObject _spawnPointPlayer;
    [SerializeField] private GameObject _spawnPointAI;
    [SerializeField] private float _waitBeforeHuntingAgain; //how long before the AI moves from their spawn point
    [SerializeField] private int _nextSceneLoad;

    public bool isWorking = false;
    private Vector3 _AISpawnTransform;
    private Vector3 _PlayerSpawnTransform;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        //sets spawn points on capture
        if (!_spawnPointAI)
        {
            _AISpawnTransform = transform.position;
        }else
        {
            _AISpawnTransform = _spawnPointAI.transform.position;
        }
        if (!_spawnPointPlayer)
        {
            _PlayerSpawnTransform = GameObject.FindGameObjectWithTag("Player").transform.position;
        }else
        {
            _PlayerSpawnTransform = _spawnPointPlayer.transform.position;
        }
    }

    public void PlayerNotCaptured()
    {
        
        switch (noCaptureType)
        {
            case 0: //library hannover
                if (_unlockDoorOnDestroy)
                {
                    _unlockDoorOnDestroy.GetComponent<Door>().doorLockedForever = false; //unlock a door
                }
                if (_makeThisObjectActive)
                {
                    _makeThisObjectActive.SetActive(true);
                }
                break;
        }
    }

    public IEnumerator PlayerCaptured()
    {
        if (FirstPersonController.aFirstPersonController._holdingThisGameobject)
        {
            FirstPersonController.aFirstPersonController.removeFromHand();
        }
        //no matter what, look at the capturing AI
        if (GetComponent<Interaction>() && captureType==2)
        {
            Debug.Log("location  " + gameObject.transform.position);
            GetComponent<OnInteractionLookAt>().lookAtLocation = gameObject;
            GetComponent<Interaction>()._interactionFinished = true;
        }
        switch (captureType)
        {
            case 0: //hannover library
                isWorking = true;
                GetComponent<AudioSource>().clip = _onCaptureClip;
                GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().loop = false;

                yield return StartCoroutine(Fade.aFade.FadetoBlack());

                _player.transform.position = warpPoint.transform.position;
                FirstPersonController.aFirstPersonController.RotateCharacter(-20f, 90f);

                if (_makeThisObjectActive)
                {
                    _makeThisObjectActive.SetActive(true);
                    if (_runActiveObjectInteraction)
                    {
                        _makeThisObjectActive.GetComponent<Interaction>().interactionAction();
                    }
                }

                yield return StartCoroutine(Fade.aFade.FadetoClear());
                isWorking = false;
                break;
            case 1: //dream daisy
                StartCoroutine(LoadScene());

                break;
            case 2: //typical reset
                
                yield return StartCoroutine(Fade.aFade.FadetoBlack());
                FirstPersonController.aFirstPersonController.gameObject.transform.position = _PlayerSpawnTransform;
                gameObject.transform.position = _AISpawnTransform;
                GetComponent<AIController>().setCapturedPlayer();
                yield return StartCoroutine(Fade.aFade.FadetoClear());
                if (GetComponent<Interaction>())
                {
                    GetComponent<Interaction>()._interactionFinished = false;
                    Debug.Log("resetting interaction");
                    GetComponent<OnInteractionLookAt>().doOnce = false;
                }
                yield return new WaitForSeconds(_waitBeforeHuntingAgain);
                break;
            default:
                //other things
                break;
        }
    }


    private IEnumerator LoadScene()
    {
        yield return StartCoroutine(Fade.aFade.FadetoBlack());
        SceneManager.LoadScene(_nextSceneLoad);
    }
}
