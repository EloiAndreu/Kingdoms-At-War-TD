using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public List<Sprite> levelsImatges;
    public List<string> levelsDescription;
    public Image levelImg;
    public GameObject levelSelection;
    public GameObject buttonsPanel;
    int levelID;
    public TMP_Text levelNumText, levelNumText1;
    public TMP_Text levelDescriptionText;
    public GameObject enemyEncicl, tropEncicl, levelEncic;

    void Start(){
        //levelSelection.SetActive(false);
    }

    public void EnableLevelSelection(int _levelID){

        if(!enemyEncicl.activeSelf && !tropEncicl.activeSelf && !levelEncic.activeSelf){
            levelID = _levelID;
            levelNumText.text = "LEVEL " + (levelID+1);
            levelNumText1.text = "LEVEL " + (levelID+1);
            levelDescriptionText.text = levelsDescription[levelID];
            levelImg.sprite = levelsImatges[levelID];
            levelSelection.SetActive(true);
            buttonsPanel.SetActive(false);
        }
    }

    public void PlayLevel(){
        GameManager.Instance.currentLevelPlaying = levelID;
        SceneController.Instance.LoadScene((levelID+2));
    }
}
