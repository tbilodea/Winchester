using UnityEngine;
using System.Collections;

public class TriggerInteraction : MonoBehaviour {
    public float delay;
    private bool doOnce;
    public GameObject interaction; //defaults to gameObject interaction
    
    void OnTriggerEnter(Collider collided)
    {
        if (!doOnce)
        {
            if (collided.tag == "Player")
            {
                StartCoroutine(interact());
            }
        }
    }

    private IEnumerator interact()
    {
        if(delay != 0)
        {
            yield return new WaitForSeconds(delay);
        }
        if (interaction)
        {
            interaction.GetComponent<Interaction>().interactionAction();
        }
        else
        {
            GetComponent<Interaction>().interactionAction();
        }
        doOnce = true;
        yield return null;
    }
}
