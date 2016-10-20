using UnityEngine;
using System.Collections;

public class OnInteractionDelayedWarp : MonoBehaviour {
    public GameObject warpPoint;
    public float rotationX;
    public float rotationY;
    public float timeWait;
    public GameObject interactionObj;

    private Interaction waitforthis;
    private bool doOnce;
	// Use this for initialization
	void Start () {
        waitforthis = interactionObj.GetComponent<Interaction>();
	}
	
	// Update is called once per frame
	void Update () {
        if (doOnce)
        {
            return;
        }
        if (waitforthis._interactionFinished)
        {
            StartCoroutine(delayedWarp());
            doOnce = true;
        }
	}

    public IEnumerator delayedWarp()
    {

        yield return new WaitForSeconds(timeWait);
        yield return StartCoroutine(Fade.aFade.FadetoBlack());
        FirstPersonController.aFirstPersonController.gameObject.transform.position = warpPoint.transform.position;
        FirstPersonController.aFirstPersonController.RotateCharacter(rotationX, rotationY);
        yield return StartCoroutine(Fade.aFade.FadetoClear());
    }
}
