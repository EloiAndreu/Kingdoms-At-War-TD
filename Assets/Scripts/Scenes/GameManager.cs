using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public bool SoundActivated = true;
    public bool MusicActivated = true;

    public static GameManager Instance{
        get{
            if (instance == null)
            {
                GameObject gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
                if(gameManagerObject != null){ 
                    instance = gameManagerObject.AddComponent<GameManager>();
                    DontDestroyOnLoad(gameManagerObject);
                }
            }
            return instance;
        }
    }

    public int currentLevelPlaying = -1;

    void Awake(){
        if(instance != null && instance != this){
            Destroy(gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
