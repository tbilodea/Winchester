using UnityEngine;
using System.Collections;

public class InteractionOnEnable : MonoBehaviour {
    public float timeAlive;
	// Update is called once per frame
	void OnEnable () {
        GetComponent<Interaction>().interactionAction();
        timeAlive = Time.time;
	}
}
