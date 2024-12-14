using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;
using TMPro; 

public class DragUI : MonoBehaviour, IDragHandler, IEndDragHandler
{
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    //VerticalLayoutGroup verticalLayoutGroup;
    //HorizontalLayoutGroup horizontalLayoutGroup;
    Vector2 originalPosition;
    GameObject previsualizeInstance;
    Vector3 previsualizePos;

    public GameObject objPrevisualize;
    public GameObject objToInstantiate;
    RaycastHit hit;
    
    public UnityEvent endDragEvent;
    public bool potArrossegar = true;

    void Start()
    {           
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        /*verticalLayoutGroup = transform.parent.GetComponent<VerticalLayoutGroup>();
        if(verticalLayoutGroup == null){
            horizontalLayoutGroup = transform.parent.GetComponent<HorizontalLayoutGroup>();
        }*/

        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(potArrossegar){
            // Moure la carta segons el desplaçament del ratolí o el dit
            Camera.main.GetComponent<CameraMov>().potArrossegar = false;
            rectTransform.anchoredPosition += eventData.delta;
            
            Vector3 inputPosition;
            if (Input.touchCount > 0){
                inputPosition = Input.GetTouch(0).position;
            }
            else{
                inputPosition = Input.mousePosition;
            }

            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            if (Physics.Raycast(ray, out hit))
            {
                int selectedFloorLayer = LayerMask.NameToLayer("Walkable");
                //Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.gameObject.layer == selectedFloorLayer){
                    AmagarCarta_MostrarCostruccio();

                    previsualizePos = hit.point;
                    if(previsualizeInstance == null){
                        previsualizeInstance = Instantiate(objPrevisualize, new Vector3(previsualizePos.x, previsualizePos.y, previsualizePos.z), Quaternion.identity);
                    }
                    else previsualizeInstance.transform.position = new Vector3(previsualizePos.x, previsualizePos.y + 3f, previsualizePos.z); 
                }
                else{
                    MostrarCarta_AmagarCostruccio();
                    Destroy(previsualizeInstance);
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(potArrossegar){
            Camera.main.GetComponent<CameraMov>().potArrossegar = true;
            MostrarCarta_AmagarCostruccio();
            canvasGroup.alpha = 1f; // Tornar a la transparència original quan es deixa d'arrossegar
            canvasGroup.blocksRaycasts = true; // Tornar a bloquejar la interacció amb altres objectes

            /*if(verticalLayoutGroup != null){
                verticalLayoutGroup.enabled = false;
                rectTransform.anchoredPosition = originalPosition; // Tornar la carta a la seva posició original si no es solta sobre una àrea d'acceptació
                verticalLayoutGroup.enabled = true;
            }
            else{
                horizontalLayoutGroup.enabled = false;
                rectTransform.anchoredPosition = originalPosition; // Tornar la carta a la seva posició original si no es solta sobre una àrea d'acceptació
                horizontalLayoutGroup.enabled = true;
            }*/

            rectTransform.anchoredPosition = originalPosition;

            Vector3 pos = hit.point;
            Instantiate(objToInstantiate, new Vector3(pos.x, 0f, pos.z), Quaternion.identity);
            endDragEvent.Invoke();
            potArrossegar = false;

            Destroy(previsualizeInstance);
            previsualizeInstance = null;
        }
    }

    void AmagarCarta_MostrarCostruccio(){
        canvasGroup.alpha = 0f;
    }

    void MostrarCarta_AmagarCostruccio(){
        canvasGroup.alpha = 0.6f;
    }
}
