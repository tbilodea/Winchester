using UnityEngine;
using System.Collections;

public class OnInteractionDisableEnableObjects : MonoBehaviour {
    public GameObject[] DisableThese;
    public GameObject[] EnableThese;

    private bool doOnce;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (doOnce)
        {
            return;
        }
        if (!GetComponent<Interaction>())
        {
            return;
        }
        if (GetComponent<Interaction>()._interactionFinished)
        {
            enableDisable();
        }
    }

    public void enableDisable()
    {
        foreach (GameObject enable in EnableThese)
        {
            enable.SetActive(true);
        }
        foreach (GameObject disable in DisableThese)
        {
            disable.SetActive(false);
        }
        doOnce = true;
    }

}
