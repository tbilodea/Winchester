using UnityEngine;
using System.Collections;

public class AIStayOrNotOnInteract : MonoBehaviour {
    public bool stay;
    public GameObject AI;
    private bool doOnce;

	// Update is called once per frame
	void Update () {
        if (doOnce)
        {
            return;
        }

        if (GetComponent<Interaction>()._interactionFinished)
        {
            AI.GetComponent<AIController>().Stay = stay;
        }
	}
}
