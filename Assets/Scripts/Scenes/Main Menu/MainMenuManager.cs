using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    int[] nivells;
    public GameObject[] estandards;
    public GameObject[] camins;

    void Start()
    {
        estandards = GameObject.FindGameObjectsWithTag("Estandard");
        camins = GameObject.FindGameObjectsWithTag("CamiUI");
        OrdenarObjectesPerNom(estandards);
        OrdenarObjectesPerNom(camins);
        GameData.Instance.LoadGameData();
        nivells = GameData.Instance.gameData.nivells;

        ActivarNivellsSuperats();
        ActivarSeguentNivell();
    }

    void ActivarNivellsSuperats(){
        for(int i=0; i<estandards.Length; i++){
            estandards[i].SetActive(false);
        }

        for(int i=0; i<camins.Length; i++){
            camins[i].SetActive(false);
        }

        for(int i=0; i<estandards.Length; i++){
            if(nivells[i] != 0){
                //GameObject castell = nivellsParent.GetChild(i).gameObject;
                GameObject castell = estandards[i];
                castell.SetActive(true);
                castell.GetComponent<CastellController>().ActivarImgNivSup(true);

                //if(i>0){
                    //GameObject cami = caminsParent.GetChild(i).gameObject;
                    if(i<camins.Length) camins[i].SetActive(true);
                //}
            }
        }
    }

    void ActivarSeguentNivell(){
        bool trobat = false; int i = 0; 
        while(i<nivells.Length && !trobat){
            if(nivells[i] == 0) trobat = true;
            else i++;
        }

        if(i<estandards.Length){
            GameObject castell = estandards[i];
            castell.SetActive(true);
            castell.GetComponent<CastellController>().ActivarImgNivSup(false);
        }
    }

    void OrdenarObjectesPerNom(GameObject[] objectes){
        System.Array.Sort(objectes, (x, y) => string.Compare(x.name, y.name));
    }

    public void ReturnHome(){
        SceneManager.LoadScene(0);
    }
}
