using UnityEngine;
using System.Collections;

public class myGUI : MonoBehaviour {
    public static myGUI aGUI;
	// Use this for initialization
	void Start () {
        aGUI = Singleton<myGUI>.Instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
