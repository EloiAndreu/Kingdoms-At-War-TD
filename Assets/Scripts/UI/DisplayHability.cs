using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DisplayHability : MonoBehaviour
{
    bool habilitatSelecionada = false;
    
    public GameObject objToInstantiate;
    public LayerMask walkableLayerMask;

    public UnityEvent clicEvent;

    public void SelecionarHabilitat(bool selecionar){
        habilitatSelecionada = selecionar;
    }

    void Update()
    {
        if (habilitatSelecionada && Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, walkableLayerMask))
            {
                habilitatSelecionada = false;
                Vector3 spawnPosition = hit.point;
                Instantiate(objToInstantiate, spawnPosition, Quaternion.identity);
                clicEvent.Invoke();
            }
        }
    }

}
