using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMenuManager : MonoBehaviour
{

    void Start(){
        GameManager.Instance.gameObject.GetComponent<AudioManager>().PlayMusic(1);
    }

    public void StartGame(){
        SceneManager.LoadScene(1);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void DeleteData(){
        GameManager.Instance.gameObject.GetComponent<GameData>().DeleteData();
    }
}
