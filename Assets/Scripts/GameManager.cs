using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager GameManage;

    private int _currentLevel;
	// Use this for initialization
	void Awake() {
        GameManage = Singleton<GameManager>.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        LevelSave(); //determines if we have changed scenes and saves
	}

    private void LevelSave()
    {
        //checks if we aren't at main menu and are in a new scene
        if (_currentLevel != SceneManager.GetActiveScene().buildIndex && SceneManager.GetActiveScene().buildIndex != 0)
        {
            Save();
            //show saved icon for ~2 seconds
            _currentLevel = SceneManager.GetActiveScene().buildIndex;
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/Save.dat");
        PlayerData playerSave = new PlayerData();
        playerSave._currentLevel = SceneManager.GetActiveScene().buildIndex;

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

    public void DestroyPreviousSaveFile()
    {
        if(File.Exists(Application.persistentDataPath + "/Save.dat"))
        {
            File.Delete(Application.persistentDataPath + "/Save.dat");
        }
    }
}

[System.Serializable]
class PlayerData
{
    public int _currentLevel;
}