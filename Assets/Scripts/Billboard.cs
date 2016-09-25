using UnityEngine;
using System.Collections;

//Class used on sprites to make them face the camera at all times
public class Billboard : MonoBehaviour {

	void Update () {
        //new Vector3(Camera.main.transform.position.x, transform.y, Camera.main.transform.position.z)
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z), Vector3.up);
	}
}
