using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void SetTimeScale(float timeScale){
        Time.timeScale = timeScale;
    }

    public void Restart(){
        GameManager.Instance.GetComponent<SceneController>().LoadSameScene();
    }

    public void Exit(){
        GameManager.Instance.GetComponent<SceneController>().LoadScene(0);
    }
}
