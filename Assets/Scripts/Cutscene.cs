using UnityEngine;
using System.Collections;

public class Cutscene : MonoBehaviour {
    public int sceneType;
    public GameObject[] lookLocations;
    public GameObject[] enableObjects;
    public TextAsset[] dialogue;
    
    public void cutscene()
    {
        switch (sceneType)
        {
            case 0://endscene
                StartCoroutine(cutscene1());
                
                break;
            case 1:
                StartCoroutine(cutscene2());
                break;
        }
    }

    private IEnumerator cutscene1()
    {
        Debug.Log("cutscene");
        GetComponent<Interaction>()._interactionFinished = false;
        OnInteractionLookAt lookAt = GetComponent<OnInteractionLookAt>();
        //lookAt.knownFlip = true;
        lookAt.lookAtLocation = lookLocations[0];
        lookAt.LookAt();
        yield return new WaitForSeconds(.1f);

        //lookAt.knownFlip = false;
        TextboxManager.aTextboxManager.loadScript(dialogue[0], GetComponent<Interaction>());//do dialogue

        Interaction interact = GetComponent<Interaction>();
        while (!interact._interactionFinished)
        {
            yield return null;
        }
        enableObjects[0].SetActive(true);
        yield return new WaitForSeconds(.1f);
        lookAt.lookAtLocation = lookLocations[1]; //look at daisy after dialogue
        lookAt.LookAt();
        yield return new WaitForSeconds(.5f);
        enableObjects[0].GetComponent<AIController>().Stay = false;
        yield return null;
    }

    private IEnumerator cutscene2() //tea room level 1
    {
        GetComponent<OnInteractionLookAt>().LookAt();
        yield return new WaitForSeconds(.1f);
        TextboxManager.aTextboxManager.loadScript(dialogue[0], GetComponent<Interaction>());
        while (!GetComponent<Interaction>()._interactionFinished)
        {
            yield return null;
        }
        enableObjects[0].GetComponent<Animator>().SetBool("sitting", false);
        enableObjects[1].GetComponent<Animator>().SetBool("sitting", false);
        yield return new WaitForSeconds(2f);
        enableObjects[0].GetComponent<NavMeshAgent>().destination = enableObjects[2].transform.position;
        enableObjects[1].GetComponent<NavMeshAgent>().destination = enableObjects[2].transform.position;
        enableObjects[0].GetComponent<Animator>().SetTrigger("walk");
        enableObjects[1].GetComponent<Animator>().SetBool("walking", true);
        while ((enableObjects[1].transform.position - enableObjects[2].transform.position).magnitude > 2)
        {
            yield return null;
        }
        enableObjects[1].SetActive(false);

        while ((enableObjects[0].transform.position - enableObjects[2].transform.position).magnitude > 2)
        {
            yield return null;
        }
        enableObjects[0].SetActive(false);
        Interaction thisinteract = enableObjects[2].GetComponent<Interaction>(); //sets door to warp type and
        if (thisinteract)
        {
            thisinteract.setInteractionType(4, 3);
        }
        enableObjects[3].GetComponent<Door>().doorLockedForever = false;
        while (enableObjects[3].GetComponent<Door>().doorLockedForever)
        {
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
