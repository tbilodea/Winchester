using UnityEngine;
using System.Collections;

public class SwitchHandObject : MonoBehaviour {

    [SerializeField]
    private GameObject invisibleObject;
    private bool firstTime = true;
    [SerializeField]
    private string idName;

    void Start()
    {

    }
    //removes and makes invisible the previous object while making the invisible object the new held object
    void OnTriggerEnter()
    {
        if (firstTime)
        {
            if (FirstPersonController.aFirstPersonController._holdingThisGameobject)
            {

                if (FirstPersonController.aFirstPersonController._holdingThisGameobject.name == idName)
                {
                    GameObject _oldHolding = FirstPersonController.aFirstPersonController._holdingThisGameobject;
                    FirstPersonController.aFirstPersonController.removeFromHand();
                    Transform _oldTransform = _oldHolding.transform;
                    _oldHolding.SetActive(false);
                    invisibleObject.SetActive(true);
                    invisibleObject.transform.position = _oldTransform.position;
                    invisibleObject.transform.rotation = _oldTransform.rotation;

                    FirstPersonController.aFirstPersonController.setIntoHand(invisibleObject);
                }
                firstTime = false;
            }
        }
    }


}
