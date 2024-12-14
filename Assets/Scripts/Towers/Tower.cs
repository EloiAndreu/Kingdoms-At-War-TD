using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{
    public int selectionID;
    public List<int> IDnivellsTorreNoDisponible = new List<int>();
    public GameObject upgradePrefab;
    GameObject uiSelectionTower;
    UIManager uiManager;
    SelectionUIController selectionUI;

    public int price = 100;
    public bool teTropesMobils = false;
    public GameObject selectPositionArea;

    void Start(){
        if(teTropesMobils) selectPositionArea.SetActive(false);
        uiSelectionTower = GameObject.FindGameObjectWithTag("UISelectTower");
        uiManager = uiSelectionTower.GetComponent<UIManager>();
        selectionUI = uiSelectionTower.GetComponent<SelectionUIController>();
    }

    /*private void OnMouseDown(){
        if(!EventSystem.current.IsPointerOverGameObject() && Input.touchCount == 0){
            ProcessarSeleccio();
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            if(!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
                ProcessarSeleccio();
            }
        }
    }*/

    /*void Update() {
        // Comprova si hi ha tocs en dispositius mòbils
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                // Comprova si el toc està sobre un element UI
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
                    ProcessarSeleccio();
                }
            }
        }

        // Per als dispositius d'escriptori amb ratolí
        if (Input.GetMouseButtonDown(0)) {
            // Comprova si el clic està sobre un element UI
            if (!EventSystem.current.IsPointerOverGameObject()) {
                ProcessarSeleccio();
            }
        }
    }*/

    void Update()
    {
        // Si és un dispositiu tàctil
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == transform)
                        {
                            ProcessarSeleccio();
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) //Si no és un dispositiu tàctil
        { //ELIMINAR ELSE SI LES TORRES ES CLIQUEN PER SOBRE ELS BOTONS
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        ProcessarSeleccio();
                    }
                }
            }
        }
    }

    void ProcessarSeleccio(){
        bool potSelecionar = selectionUI.MoureAObjecteClicat(this.gameObject);
        if(potSelecionar){
            if(selectionID==1){
                if(upgradePrefab != null && ComprovarSiEsPotMillorar()){ 
                    List<int> idObjectesAMostrar = new List<int>(){selectionID, 3};
                    uiManager.MostrarObjectes(idObjectesAMostrar);
                }
                else{
                    List<int> idObjectesAMostrar = new List<int>(){5, 3};
                    uiManager.MostrarObjectes(idObjectesAMostrar);
                }
            }
            else{
                List<int> idObjectesAMostrar = new List<int>(){selectionID, 3};
                uiManager.MostrarObjectes(idObjectesAMostrar);    
            }
        }
    }

    bool ComprovarSiEsPotMillorar(){
        List<int> IDnivellsTorreNoDisponibleMillora = upgradePrefab.GetComponent<Tower>().IDnivellsTorreNoDisponible;
        return (!IDnivellsTorreNoDisponibleMillora.Contains(LevelManager.Instance.levelID));
    }

    public void MoureTropes(){
        selectPositionArea.SetActive(true);
    }

    public void SetInitialToopPosition(Vector3 destination){
        selectPositionArea.GetComponent<SelectPositionArea>().ChangeInitialDestinationPosition(destination);
    }

}
