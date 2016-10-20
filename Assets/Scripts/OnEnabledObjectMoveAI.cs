using UnityEngine;
using System.Collections;

public class OnEnabledObjectMoveAI : MonoBehaviour {
    public GameObject enabledObject;
    public GameObject moveToHere;
    public bool doInteraction;
    private NavMeshAgent _navMesh;

	// Use this for initialization
	void Start () {
        _navMesh = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (enabledObject.activeInHierarchy)
        {
            _navMesh.destination = moveToHere.transform.position;
            if (doInteraction)
            {
                GetComponent<Interaction>().interactionAction();
            }
        }
	}
}
