using UnityEngine;
using System.Collections;

public class FireCapture : MonoBehaviour {
    [SerializeField]private float timerLength;
    [SerializeField]private GameObject setFire;
    [SerializeField]private GameObject flames;
    [SerializeField]private GameObject resetPlayerPosition;

    private bool diedOnce;
    private float timerStart;
    private bool fireOn;
    private bool timerModified;
    
	// Update is called once per frame
	void Update () {
        if (setFire)
        {
            if (setFire.activeInHierarchy)
            {
                timerStart = setFire.GetComponent<InteractionOnEnable>().timeAlive;
                setFire = null; //so we don't do this again
                fireOn = true;
            }
        }
        //if time ran out
        if(Time.time-timerStart > timerLength && fireOn)
        {
            //make flames active
            flames.SetActive(true);
        }
        if (diedOnce && !timerModified)
        {
            diedOnce = false;
            timerLength = timerLength * 3 / 4;
            timerModified = true;
        }
	}

    public IEnumerator resetTimer()
    {
        yield return StartCoroutine(Fade.aFade.FadetoBlack());
        timerStart = Time.time;
        diedOnce = true;
        FirstPersonController.aFirstPersonController.removeFromHand();
        flames.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").transform.position = resetPlayerPosition.transform.position;
        yield return StartCoroutine(Fade.aFade.FadetoClear());
    }
}
