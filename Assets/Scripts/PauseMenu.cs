using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//this class takes care of all pause menu behavior and is the main menu options behavior
public class PauseMenu : MonoBehaviour {
    public static PauseMenu aPauseMenu;
    public float mainVolume;

    [SerializeField]private GameObject _primaryMenu;
    [SerializeField]private GameObject _optionsMenu;
    [SerializeField]private GameObject _quitMenu;
    [SerializeField]private GameObject _volumeSlider;
    [SerializeField]private AudioClip _testAudio;
    [SerializeField]private GameObject _mouseSensitivity;

    // Use this for initialization
    void Awake () {
        aPauseMenu = Singleton<PauseMenu>.Instance;
        gameObject.SetActive(false);
        //set options and quit items
	}

    //set volume to match that of options menu bar
    public void volumeChange()
    {
        AudioListener.volume = _volumeSlider.GetComponent<Slider>().value;
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            FirstPersonController.aFirstPersonController.gameObject.GetComponent<AudioSource>().clip = _testAudio;
            FirstPersonController.aFirstPersonController.gameObject.GetComponent<AudioSource>().Play(); //in game test to change volume
        }else
        {
            //find where to put clip on main menu;
        }
    }

    public void sensitivityChange()
    {
        FirstPersonController.aFirstPersonController.setMouseLookSensitivity(_mouseSensitivity.GetComponent<Slider>().value);
    }

	public void clickResumeButton()
    {
        aPauseMenu.gameObject.SetActive(false);
        Cursor.visible = false;
    }
	
    public void clickOptionButton()
    {
        _optionsMenu.SetActive(true);
        _primaryMenu.SetActive(false);
    }
    public void openOptionsMenu()
    {
        aPauseMenu.gameObject.SetActive(true);
        _optionsMenu.SetActive(true);
        _primaryMenu.SetActive(false);
    }

    public void clickOptionsBackButton()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)//if we are just in the pause menu
        {
            _optionsMenu.SetActive(false);
            _primaryMenu.SetActive(true);
        }else //we are at the main menu, so the UI just goes on and off
        {
            _optionsMenu.SetActive(false);
            _primaryMenu.SetActive(true); //reset primary to come up when in game
            aPauseMenu.gameObject.SetActive(false);
        }
    }

    public void clickQuitButton()
    {
        _quitMenu.SetActive(true);
        _primaryMenu.SetActive(false);
    }

    public void clickNoQuitMenu()
    {
        _quitMenu.SetActive(false);
        _primaryMenu.SetActive(true);
    }

    public void clickYesQuitMenu()
    {
        aPauseMenu.gameObject.SetActive(false);
        Application.Quit();
    }
}
