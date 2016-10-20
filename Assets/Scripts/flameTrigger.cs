using UnityEngine;
using System.Collections;

public class flameTrigger : MonoBehaviour {
    public GameObject flameManager;
    private FireCapture _FC;

	// Use this for initialization
	void Start () {
        _FC = flameManager.GetComponent<FireCapture>();
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider coll) {
	    if(coll.tag == "Player")
        {
            StartCoroutine(_FC.resetTimer());
        }
	}
}
