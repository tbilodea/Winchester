using UnityEngine;
using System.Collections;

public class EnableLibraryKey : MonoBehaviour {
    //makes key enabled
    [SerializeField]
    private GameObject enableThis;

	void OnTriggerEnter()
    {
        if (FirstPersonController.aFirstPersonController._holdingThisGameobject)
        {
            if(FirstPersonController.aFirstPersonController._holdingThisGameobject.name == "candleOn")
            {
                enableThis.SetActive(true);
            }
        }
        
    }
}
