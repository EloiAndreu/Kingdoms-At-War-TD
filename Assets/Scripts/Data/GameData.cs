using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance;

    public static GameData Instance{
        get{
            if (instance == null)
            {
                GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
                if(gameManagerObject != null){ 
                    instance = gameManagerObject.AddComponent<GameData>();
                    DontDestroyOnLoad(gameManagerObject);
                }
            }
            return instance;
        }
    }

    public SaveData gameData;

    void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveInitialData(){
        int[] nivells = new int[9];
        SaveData initialData = new SaveData(nivells);
        gameData = initialData;
        SaveGameData();
    }

    public void SaveGameData(){
        SaveSystem.SaveData(gameData);
    }

    public void LoadGameData(){
        gameData = SaveSystem.LoadData();
        if(gameData == null) SaveInitialData();
    }

    public void AddLevelSuperat(int levelID){
        gameData.nivells[levelID] = 1;
        SaveGameData();
    }

    public void DeleteData(){
        SaveSystem.DeleteData();
    }
}
