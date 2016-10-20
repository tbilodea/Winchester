using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager aMainMenuManager;
    public GameObject clickedOn;
    public GameObject areYouSure;
    public GameObject optionsMenu;

    private bool _savedGame;
    private bool _areYouSureMenu; //flags menu up or down

    void Start()
    {
        //grab the options GUI menu from the prefab
        //check for save and set _savedGame
        if (File.Exists(Application.persistentDataPath + "/Save.dat"))
        {
            _savedGame = true;
        }
    }

    //Pushes play
    void PlayPressed()
    {
        //check for save
        //figure out filepath to save under
        Debug.Log("playpressed");
        if (_savedGame)
        {
            areYouSure.SetActive(true);
            _areYouSureMenu = true;
        }else
        {
            SceneManager.LoadScene(1);
        }
    }

    void areYouSureYES()
    {
        //delete old save
        File.Delete(Application.persistentDataPath + "/Save.dat");
        SceneManager.LoadScene(1); //loadGame
    }

    void areYouSureNO()
    {
        areYouSure.SetActive(false);
    }

    void ContinuePressed()
    {
        if (_savedGame)
        {
            GameManager.aGameManager.Load();
        }
    }

    void OptionsPressed()
    {
        //set GUI options menu active
        PauseMenu.aPauseMenu.gameObject.SetActive(true);
    }

    // Checks for escape press
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionsMenu.activeInHierarchy)
        {
            Application.Quit();
        }else
        {
            optionsMenu.SetActive(false);
        }
        Debug.Log(clickedOn);
        if (clickedOn)
        {
            if (areYouSure.activeInHierarchy)
            {
                if (clickedOn.name == "YES")
                {
                    areYouSureYES();
                }
                if (clickedOn.name == "NO")
                {
                    areYouSureNO();
                }
            }
            else
            {
                if (clickedOn.name == "Play")
                {
                    PlayPressed();
                }
                if (clickedOn.name == "Continue")
                {
                    ContinuePressed();
                }
                if (clickedOn.name == "Options")
                {
                    OptionsPressed();
                }
                if (clickedOn.name == "Quit")
                {
                    Application.Quit();
                }
            }
        }
        clickedOn = null;
    }
}