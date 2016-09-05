using UnityEngine;
using System.Collections;

//Class used on sprites to make them face the camera at all times
public class Billboard : MonoBehaviour {

	void Update () {
        transform.LookAt(Camera.main.transform.position, -Vector3.up);
	}
}
