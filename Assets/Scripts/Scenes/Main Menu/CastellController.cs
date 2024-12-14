using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CastellController : MonoBehaviour
{
    public int levelID;
    GameObject levelSelectionGameObject;
    LevelSelection levelSelection;
    public GameObject imgNivEnemic, imgNivAmic;

    void Start(){
        levelSelectionGameObject = GameObject.FindGameObjectWithTag("selectorNivell");
        levelSelection = levelSelectionGameObject.GetComponent<LevelSelection>();
    }

    void OnMouseDown(){
        levelSelection.EnableLevelSelection(levelID);
    }

    public void ActivarImgNivSup(bool superat){
        if(superat){
            imgNivAmic.SetActive(true);
        }
        else{
            imgNivEnemic.SetActive(true);
        }
    }

    
}
