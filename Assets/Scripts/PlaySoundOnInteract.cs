using UnityEngine;
using System.Collections;

public class PlaySoundOnInteract : MonoBehaviour {
    private AudioSource _as;
    public AudioClip doThisClip;
    public bool loop;
    private bool doOnce;

	// Use this for initialization
	void Start () {
        _as = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (doOnce)
        {
            return;
        }

	    if(_as && GetComponent<Interaction>()._interactionFinished)
        {
            if (doThisClip)
            {
                _as.clip = doThisClip;
            }
            if (loop)
            {
                _as.loop = true;
            }
            else
            {
                _as.loop = false;
            }
            _as.Play();
            doOnce = true;
        }
	}
}
