using UnityEngine;
using System.Collections;

public class AfterDaisySheetMusic : MonoBehaviour {
    private AudioSource _as;
    private Animator _anim;
    [SerializeField] private GameObject DaisySheet;
    private Interaction _daisyInteract;
    [SerializeField] private AudioClip _clockChime;
    [SerializeField] private GameObject targetWarp;
    [SerializeField] private TextAsset commentOnWarp; 
    private bool _canWarp;

	// Use this for initialization
	void Start () {
        _as = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _daisyInteract = DaisySheet.GetComponent<Interaction>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_daisyInteract._interactionFinished)
        {
            Debug.Log("daisy sheet read");
            _as.Stop();
            _as.clip = _clockChime;
            
            _as.Play();
            _canWarp = true;
            _daisyInteract._interactionFinished = false;
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if (_canWarp)
        {
            coll.gameObject.transform.position = targetWarp.transform.position;
            FirstPersonController.aFirstPersonController.RotateCharacter(0f, 180f);
            TextboxManager.aTextboxManager.loadScript(commentOnWarp, null);
        }
    }
}
