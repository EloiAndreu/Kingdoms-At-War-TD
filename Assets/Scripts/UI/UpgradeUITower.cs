using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeUITower : MonoBehaviour
{
    public SelectionUIController selectionController;
    int priceMillora = 0; 
    int priceActual = 0;

    public TMP_Text preuMillora;
    GameObject upgradePrefab;
    //public GameObject buttoMoureTropes;

    Color colorTextosInicial = Color.white;
    public Color colorTextPreuNoDisponible;
    //public GameObject towerPlacePrefab;

    void OnEnable(){
        //colorTextosInicial = preuMillora.color;

        GameObject currentPrefab = selectionController.GetHitObject();

        Tower currentTower = currentPrefab.GetComponent<Tower>();
        upgradePrefab = currentTower.upgradePrefab;
        priceActual = currentTower.price;
        //if(currentTower.teTropesMobils) buttoMoureTropes.SetActive(true);
        //else buttoMoureTropes.SetActive(false);

        priceMillora = upgradePrefab.GetComponent<Tower>().price;
        preuMillora.text = priceMillora.ToString();
    }

    public void UpgradeTower(Transform objReference){
        //int price = upgradePrefab.GetComponent<Tower>().price;
        if(LevelManager.Instance.BuySomething(priceMillora)){
            GameObject newTower = Instantiate(upgradePrefab, objReference.position, objReference.rotation, objReference.parent);

            /*if(newTower.GetComponent<Tower>().selectPositionArea != null){
                //Debug.Log("Prova");
                Vector3 destination = objReference.GetChild(2).GetChild(0).GetComponent<TroopSpawn>().initialDestination;
                newTower.GetComponent<Tower>().SetInitialToopPosition(destination);
                newTower.GetComponent<Tower>().MoureTropes();
            }*/
            Destroy(objReference.gameObject);
        }
    }

    void Update(){
        ModificarColorTextosPreus();
    }

    void ModificarColorTextosPreus(){
        if(LevelManager.Instance.currentCoins < priceMillora){
            preuMillora.color = colorTextPreuNoDisponible;
        }
        else preuMillora.color = colorTextosInicial;
    }

    /*public void SellTower(){
        GameObject currentPrefab = selectionController.GetHitObject();
        LevelManager.Instance.AddCoins((int)(priceActual*0.75f));
        Instantiate(towerPlacePrefab, currentPrefab.transform.position, currentPrefab.transform.rotation, currentPrefab.transform.parent);
        Destroy(currentPrefab);
    }*/

    public void MoureTropes(){
        GameObject currentPrefab = selectionController.GetHitObject();
        Tower currentTower = currentPrefab.GetComponent<Tower>();
        currentTower.MoureTropes();
    }
}
