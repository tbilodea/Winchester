using UnityEngine;
using System.Collections;

public class triggerCheckObject : MonoBehaviour {
    public GameObject[] Objectstocollect;
    private string[] names;
    public bool changeUponOneCollection; //

    private int amountOfObjectsCollected;
    private int objectCollided;

    public int CollectedAllObjectsType; //what level is doing this and what object

    public bool dontRemoveObjectFromHand;

    public GameObject[] enablingThis; //objects to enable after
    public GameObject[] disablingThis;

    // Use this for initialization
    void Start() {
        names = new string[Objectstocollect.Length];

        for (int i = 0; i < Objectstocollect.Length; i++)
        {
            names[i] = Objectstocollect[0].name;
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider aCollider)
    {
        if (CheckCollider(aCollider))
        {
            GameObject collectedObject = FirstPersonController.aFirstPersonController._holdingThisGameobject;
            FirstPersonController.aFirstPersonController.removeFromHand();
            if (!dontRemoveObjectFromHand)
            {
                aCollider.gameObject.SetActive(false);
            }
            amountOfObjectsCollected++;
        }
        if (amountOfObjectsCollected == Objectstocollect.Length)
        {
            CollectedAllObjects();
        }
    }

    //checks if the collider is one of the gameobjects and marks which one if so using objectCollided int
    bool CheckCollider(Collider coll)
    {
        for (int i = 0; i < Objectstocollect.Length; i++)
        {
            if (coll.name == Objectstocollect[i].name)
            {
                if (changeUponOneCollection)
                {
                    CollectedOneObject(Objectstocollect[i]);
                }
                objectCollided = i;
                return true;
            }
        }
        return false;
    }

    void CollectedOneObject(GameObject theCollected)
    {
        switch (CollectedAllObjectsType)
        {
            case 0: //grandfather clock 2_1
                if(theCollected.name == "longHand")
                {
                    //enable long hand
                    enablingThis[0].SetActive(true);
                }else
                {
                    //enable short hand
                    enablingThis[1].SetActive(true);
                }
                break;
            case 4://coal pile
                transform.position = new Vector3(transform.position.x,transform.position.y-1,transform.position.z);

                break;
        }
    }

    void CollectedAllObjects()
    {
        switch (CollectedAllObjectsType)
        {
            case 0: //grandfather clock 2_1
                GetComponent<Animator>().SetBool("backwards", true);
                GetComponent<AudioSource>().Play();
                //set the back room to daisy's room rather than the study                 //turn on daisy to chase into her room as well
                GetComponent<OnInteractionDisableEnableObjects>().enableDisable();
                break;
            case 1: //plank door
                GetComponent<AudioSource>().Play();
                GetComponent<OnInteractionDisableEnableObjects>().enableDisable();
                break;
            case 2://boiler specific
                GetComponent<AudioSource>().Play();
                GetComponent<OnInteractionDisableEnableObjects>().enableDisable();
                transform.parent.gameObject.GetComponent<Animator>().SetTrigger("boilerOn");
                break;
            case 3://generic
                GetComponent<OnInteractionDisableEnableObjects>().enableDisable();
                break;
            case 4:
                GetComponent<OnInteractionDisableEnableObjects>().enableDisable();
                gameObject.SetActive(false);
                break;
        }
    }
}
