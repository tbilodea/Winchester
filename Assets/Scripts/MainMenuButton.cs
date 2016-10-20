using UnityEngine;
using System.Collections;

public class MainMenuButton : MonoBehaviour {
    public GameObject mainmenumanager;

    void OnMouseUpAsButton()
    {
        Debug.Log("clicked a athing");
        mainmenumanager.GetComponent<MainMenuManager>().clickedOn = gameObject;
    }
}
