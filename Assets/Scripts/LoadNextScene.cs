using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider collider) {
	    if(collider.tag == "Player")
        {
            try { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
            catch{
                Scene thisScene = SceneManager.GetActiveScene();
                Debug.LogError("Failed to load scene "+ thisScene.buildIndex);
            }
        }
	}
}
