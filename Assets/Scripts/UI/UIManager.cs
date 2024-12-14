using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> uiObjects;
    public SelectionUIController selectionController;
    public GameObject buttoMoureTropes;

    void Start(){
        AmagarObjectes();
    }

    void Update(){
        bool trobat = false;
        int i=0;
        while(i<uiObjects.Count && !trobat){
            if(uiObjects[i].activeSelf) trobat = true;
            else i++;
        }

        if(trobat){
            selectionController.MoureAObjecteClicat(null);
        }
    }

    public void AmagarObjectes(){
        for(int i=0; i<uiObjects.Count; i++){
            uiObjects[i].SetActive(false);
            buttoMoureTropes.SetActive(false);
        }
    }

    public void MostrarObjecte(int selectionID){
        AmagarObjectes();
        uiObjects[selectionID].SetActive(true);
        MostrarButoMoureTropes();
    }

    public void MostrarObjectes(List<int> objectes){
        AmagarObjectes();
        for(int i=0; i<objectes.Count; i++){
            uiObjects[objectes[i]].SetActive(true);
            MostrarButoMoureTropes();
        }
    }

    void MostrarButoMoureTropes(){
        GameObject currentPrefab = selectionController.GetHitObject();
        Tower currentTower = currentPrefab.GetComponent<Tower>();

        if(currentTower.teTropesMobils) buttoMoureTropes.SetActive(true);
        else buttoMoureTropes.SetActive(false);
    }
}
