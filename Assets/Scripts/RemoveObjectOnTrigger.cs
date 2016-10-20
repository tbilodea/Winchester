using UnityEngine;
using System.Collections;

//plays audio and/or makes object disappear
public class RemoveObjectOnTrigger : MonoBehaviour {

    [SerializeField]
    private GameObject objectToDisappear;
    [SerializeField]
    private AudioClip playThisOnTrigger;
    [SerializeField]
    private bool doThisOnce;
    private int timeThrough;

    void OnTriggerEnter(Collider coll)
    {
        if (doThisOnce && timeThrough == 0)
        {
            check(coll);
        }else if (!doThisOnce)
        {
            check(coll);
        }
        timeThrough++;
    }

    void check(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (objectToDisappear)
            {
                objectToDisappear.SetActive(false);
            }
            
            if (GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().clip = playThisOnTrigger;
                GetComponent<AudioSource>().Play();
            }
        }
    }
	
}
