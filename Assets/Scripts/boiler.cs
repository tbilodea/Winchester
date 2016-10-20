using UnityEngine;
using System.Collections;

public class boiler : MonoBehaviour {

    public GameObject childAudio;
    public GameObject[] makeAlive;

	// Use this for initialization
	void Start () {
	
	}
	
    void Update()
    {
        if (GetComponent<Interaction>()._interactionFinished)
        {
            GetComponent<AudioSource>().Play();
            childAudio.GetComponent<AudioSource>().Play();
            GetComponent<Animator>().SetTrigger("boilerOn");
            foreach(GameObject flower in makeAlive)
            {
                flower.SetActive(true);
            }
            Destroy(this);
        }
    }
}
