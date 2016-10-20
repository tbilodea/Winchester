using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
    public static HUD aHUD;

    private GameObject _interactImage;
    private GameObject _hintText;
    private GameObject _saveIcon;
	// Use this for initialization
	void Start () {
        aHUD = Singleton<HUD>.Instance;
        _hintText = gameObject.transform.GetChild(0).gameObject;
        _interactImage = gameObject.transform.GetChild(1).gameObject;
        _saveIcon = gameObject.transform.GetChild(2).gameObject;
	}
	
    //function shows and hides the grab hand
    public void toggleGrab(bool isActive)
    {
        _interactImage.SetActive(isActive);
    }

    public GameObject getSaveIcon() { return _saveIcon; }
    public GameObject getHint() { return _hintText; }
}
