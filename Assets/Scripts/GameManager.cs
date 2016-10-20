using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager aGameManager;
    //Level 1 files
    [SerializeField] private GameObject _hannoverCutScene;
    [SerializeField] private GameObject _robinRoomDoor;
    [SerializeField] private GameObject _makeBedSleepable;
    [SerializeField] private GameObject _doorToBedroom;
    [SerializeField] private GameObject _downstairs;
    //Level 2_1 files
    [SerializeField] private GameObject clockTrigger;
    //Scene 7 level 3
    [SerializeField] private GameObject hannover7;
    //afterward scene 8
    [SerializeField] private GameObject afterward;

    private bool doOnce;
    private float originalPlayerY; //used in lookAt
    private int _currentLevel;
	// Use this for initialization
	void Awake() {
        aGameManager = Singleton<GameManager>.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        LevelSave(); //determines if we have changed scenes and saves
        if (doOnce)
        {
            return;
        }
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            //HUD.aHUD.getHint().GetComponent<Text>().text = "";
            return;
        }
        //Intro Scene is build index 1, main menu is build index 0

        //Level 1, mostly controlled by triggers
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (_downstairs.activeInHierarchy && !_hannoverCutScene.activeInHierarchy)
            {
                HUD.aHUD.getHint().GetComponent<Text>().text = "Use a light source to find your way around";
            }else
            {
                HUD.aHUD.getHint().GetComponent<Text>().text = "Explore the level";
            }
            if (_hannoverCutScene.activeInHierarchy)//check for scolding and make doors lock and unlock appropriately
            {
                _makeBedSleepable.SetActive(true);
                HUD.aHUD.getHint().GetComponent<Text>().text = "Get some sleep";
                //check if door to Robin's room is locked. if it is, they've gone through this level and they're allowed to only goto the bed
                if (!_robinRoomDoor.GetComponent<Door>().doorLockedForever)
                {
                    if (_doorToBedroom.GetComponent<Door>().doorState)
                    {
                        _doorToBedroom.GetComponent<Door>().RunDoor();
                    }
                    _doorToBedroom.GetComponent<Door>().doorLockedForever = true; //force character through rest of level
                }
            }
            return;
        }

        if(SceneManager.GetActiveScene().buildIndex == 3)//lvlv 2_0
        {
            HUD.aHUD.getHint().GetComponent<Text>().text = "??????";
            doOnce = true;
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex == 4)//scene 2_1
        {
            if (clockTrigger.GetComponent<Interaction>()._interactionFinished)
            {
                HUD.aHUD.getHint().GetComponent<Text>().text = "Repair the clock.";
                doOnce = true;
            }else
            {
                HUD.aHUD.getHint().GetComponent<Text>().text = "";

            }
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex == 5)//scene 3_0
        {
            HUD.aHUD.getHint().GetComponent<Text>().text = "Figure out how to get out of the basement";
            doOnce = true; //don't come back here!
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex == 6)//scene 3_1
        {
            HUD.aHUD.getHint().GetComponent<Text>().text = "Find a way back into the house";
            doOnce = true;
            return;
        }
        //Level 3_2 Scene 7
        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            if (hannover7)
            {
                if (hannover7.activeInHierarchy)
                {
                    if (hannover7.GetComponent<Interaction>()._interactionFinished)
                    {
                        hannover7.GetComponent<Billboard>().stop = true;
                        HUD.aHUD.getHint().GetComponent<Text>().text = "Get out of the house before it burns down";
                    }else
                    {
                        HUD.aHUD.getHint().GetComponent<Text>().text = "Distract Mrs. Hannover";
                    }
                }
                else
                {
                    HUD.aHUD.GetComponent<Text>().text = "Search the room for info on a way out";
                }
                return;
            }
        }
        if(SceneManager.GetActiveScene().buildIndex == 8)
        {
            if (afterward.GetComponent<Interaction>()._interactionFinished)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
	}

    public float getOriginalPlayerY() { return originalPlayerY; }
    //starts save script if scenes have changed
    private void LevelSave()
    {
        //checks if we aren't at main menu and are in a new scene
        if (_currentLevel != SceneManager.GetActiveScene().buildIndex && SceneManager.GetActiveScene().buildIndex != 0)
        {
            Save();
            StartCoroutine(SaveIcon());//show saved icon for ~2 seconds
            _currentLevel = SceneManager.GetActiveScene().buildIndex;
            //also stores FirstPersonController original transform to the level
            originalPlayerY = FirstPersonController.aFirstPersonController.gameObject.transform.eulerAngles.y;
            Debug.Log("Gamemanager changes original y rotation to: " + originalPlayerY);
            doOnce = false;
        }
        

        
    }

    private IEnumerator SaveIcon()
    {
        HUD.aHUD.getSaveIcon().SetActive(true);
        yield return new WaitForSeconds(5);
        HUD.aHUD.getSaveIcon().SetActive(false);
    }
    //saves player data in binary file
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/Save.dat");
        PlayerData playerSave = new PlayerData();
        playerSave._currentLevel = SceneManager.GetActiveScene().buildIndex;

        if(playerSave._currentLevel == 3)
        {
            playerSave._currentLevel = 2;
        }
        //make one exception for the forked first level! don't save til they finish the first "level"

        bf.Serialize(fileStream, playerSave);
        fileStream.Close();
    }

    //Loads the game up using the file presented
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/Save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/Save.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(fileStream);
            fileStream.Close();

            SceneManager.LoadScene(data._currentLevel);
        }
    }

    //destroys previous save
    public void DestroyPreviousSaveFile()
    {
        if(File.Exists(Application.persistentDataPath + "/Save.dat"))
        {
            File.Delete(Application.persistentDataPath + "/Save.dat");
        }
    }
}
//class stores the data
[System.Serializable]
class PlayerData
{
    public int _currentLevel;
}