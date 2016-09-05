using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public static GameObject areYouSure; //double check for deleting old save
    public bool play;
    public bool cont;
    public bool options;
    public bool quit;
    public bool yes;
    public bool no;

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
            GameManager.GameManage.Load();
        }
    }

    void OptionsPressed()
    {
        //set GUI options menu active
    }

    // Checks for escape press
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnMouseUpAsButton()
    {
        if (play)
        {
            if (!_areYouSureMenu)
            {
                PlayPressed();
            }
        }
        if (cont)
        {
            if (!_areYouSureMenu)
            {
                ContinuePressed();
            }
        }
        if (options)
        {
            if (!_areYouSureMenu)
            {
                OptionsPressed();
            }
        }
        if (quit)
        {
            Application.Quit();
        }
        if (yes)
        {
            areYouSureYES();
        }
        if (no)
        {
            areYouSureNO();
        }
    }
}