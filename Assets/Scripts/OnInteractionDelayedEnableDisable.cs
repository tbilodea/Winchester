using UnityEngine;
using System.Collections;

public class OnInteractionDelayedEnableDisable : MonoBehaviour {
    public GameObject[] DisableThese;
    public GameObject[] EnableThese;

    public float timer;

    private bool doOnce;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (doOnce)
        {
            return;
        }
        if (GetComponent<Interaction>()._interactionFinished)
        {
            StartCoroutine(DelayedEnableDisable());
        }
    }

    private IEnumerator DelayedEnableDisable()
    {
        yield return new WaitForSeconds(timer);
        foreach (GameObject enable in EnableThese)
        {
            enable.SetActive(true);
        }
        foreach (GameObject disable in DisableThese)
        {
            disable.SetActive(false);
        }
        doOnce = true;
        yield return null;
    }
}
