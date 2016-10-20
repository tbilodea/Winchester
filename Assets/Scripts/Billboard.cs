using UnityEngine;
using System.Collections;

//Class used on sprites to make them face the camera at all times
public class Billboard : MonoBehaviour {
    public bool stop;

	void Update () {
        if (stop)
        {
            return;
        }
        //new Vector3(Camera.main.transform.position.x, transform.y, Camera.main.transform.position.z)
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z), Vector3.up);
	}
}
