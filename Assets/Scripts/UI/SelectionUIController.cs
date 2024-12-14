using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionUIController : MonoBehaviour
{
    public RectTransform selectionUI;
    public Canvas canvas;
    Camera mainCamera;
    GameObject hitObject;
    UIManager uiManager;

    public SelectTowerUI selectTowerUI;
    public UpgradeUITower upgradeTowerUI;

    public GameObject towerPlacePrefab;
    public bool selecionantPosMoure = false;
    GameObject previsualizeObject;

    void Start(){
        mainCamera = Camera.main;
        uiManager = GetComponent<UIManager>();
    }

    public bool MoureAObjecteClicat(GameObject objecteClicat){
        if(selecionantPosMoure) return false;
        else{
            if(objecteClicat == null && hitObject == null) return false;
            /*Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)){*/
                if(objecteClicat != null) hitObject = objecteClicat;
                //hitObject = hit.transform.gameObject;
                Vector3 worldPosition = hitObject.transform.position;
                //Vector3 worldPosition = hit.transform.position;
                Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
            
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform, 
                    screenPosition, 
                    canvas.worldCamera, 
                    out Vector2 localPoint);

                selectionUI.localPosition = localPoint;

                if(previsualizeObject != null) previsualizeObject.transform.position = worldPosition;
            //}

            return true;
        }
    }

    public void PrevisualizeTower(int towerID){
        DesPrevisualizeTower();
        previsualizeObject = selectTowerUI.PrevisualizeTower(towerID, hitObject.transform);
    }

    public void DesPrevisualizeTower(){
        if(previsualizeObject != null){
            Destroy(previsualizeObject);
        }
    }

    public void CreateTower(int towerID){
        //Debug.Log("Create Tower");
        selectTowerUI.CreateTower(towerID, hitObject.transform);
        uiManager.AmagarObjectes();
    }

    public void UpgradeTower(){
        //GameObject upgradePrefab = hitObject.GetComponent<Tower>().upgradePrefab;
        upgradeTowerUI.UpgradeTower(hitObject.transform);
        uiManager.AmagarObjectes();
    }

    public void SellTower(){
        GameObject currentPrefab = hitObject;
        float priceActual = currentPrefab.GetComponent<Tower>().price;
        LevelManager.Instance.AddCoins((int)(priceActual*0.75f));
        Instantiate(towerPlacePrefab, currentPrefab.transform.position, currentPrefab.transform.rotation, currentPrefab.transform.parent);
        Destroy(currentPrefab);

        uiManager.AmagarObjectes();
    }

    public GameObject GetHitObject(){
        return hitObject;
    }

    public void MoureTropes(){
        selecionantPosMoure = true;
        upgradeTowerUI.MoureTropes();
        uiManager.AmagarObjectes();
    }

    public void TropesMogudes(){
        selecionantPosMoure = false;
    }
}
