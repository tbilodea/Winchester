using UnityEngine;
using System.Collections;

//stops music attached when trigger goes off
public class StopAudioTrigger : MonoBehaviour {
    [SerializeField] private AudioClip stopClip;
    private bool firstTime = true;

    void OnTriggerEnter(Collider thisObj)
    {
        if (firstTime)
        {
            AudioSource _as = gameObject.GetComponent<AudioSource>();
            if (_as)
            {
                _as.Stop();
                if (stopClip)
                {
                    _as.clip = (stopClip);
                    _as.Play();
                    _as.loop = false;
                }
            }
            firstTime = false;
        }
    }
	
}
