using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectTowerUI : MonoBehaviour
{
    public List<GameObject> towers;
    public List<GameObject> previsualizeTowers;
    public List<GameObject> confirmButtons;
    public List<TMP_Text> preusTextos;
    Color colorTextosInicial;
    public Color colorTextPreuNoDisponible;

    GameObject previsualizeObject; 

    void Start(){
        for(int i=0; i<towers.Count; i++){
            int price = towers[i].GetComponent<Tower>().price;
            preusTextos[i].text = price.ToString();
        }
        colorTextosInicial = preusTextos[0].color;
    }

    public void AmagarConfirmButtons(){
        for(int i=0; i<confirmButtons.Count; i++){
            confirmButtons[i].SetActive(false);
        }
    }

    public void ShowConfirmButton(int buttonID){
        AmagarConfirmButtons();
        confirmButtons[buttonID].SetActive(true);
    }

    public GameObject PrevisualizeTower(int towerID, Transform objReference){
        previsualizeObject = Instantiate(previsualizeTowers[towerID], objReference.position, objReference.rotation, objReference.parent);
        return previsualizeObject;
    }

    public void CreateTower(int towerID, Transform objReference){
        int price = towers[towerID].GetComponent<Tower>().price;
        if(LevelManager.Instance.BuySomething(price)){
            Instantiate(towers[towerID], objReference.position, objReference.rotation, objReference.parent);
            Destroy(objReference.gameObject);
        }
        Destroy(previsualizeObject);
    }

    void Update(){
        ModificarColorTextosPreus();
    }

    void ModificarColorTextosPreus(){
        for(int i=0; i<preusTextos.Count; i++){
            
            int price = towers[i].GetComponent<Tower>().price;
            if(LevelManager.Instance.currentCoins < price){
                preusTextos[i].color = colorTextPreuNoDisponible;
            }
            else preusTextos[i].color = colorTextosInicial;
        }
    }
}
