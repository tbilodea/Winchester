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
            StartCoroutine(changeLevel());
        }
	}

    private IEnumerator changeLevel()
    {
        yield return StartCoroutine(Fade.aFade.FadetoBlack());

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
