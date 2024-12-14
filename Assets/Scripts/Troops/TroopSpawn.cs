using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopSpawn : MonoBehaviour
{
    public GameObject troopPrefab;
    bool esperarGenerarInstancia = false;
    public float tempsAReapareixer = 5f;
    GameObject troop;
    public Vector3 initialDestination;

    void Start(){
        //Debug.Log(transform.localPosition);
        //Debug.Log(transform.position);
        initialDestination = transform.position;
        GenerarInstancia();
    }

    void Update(){
        if(!esperarGenerarInstancia && transform.childCount <= 0f){
            esperarGenerarInstancia = true;
            StartCoroutine(EsperarIGenerarInstancia());
        }
        
        if(transform.childCount > 0f) esperarGenerarInstancia = false;
    }

    IEnumerator EsperarIGenerarInstancia(){
        yield return new WaitForSeconds(tempsAReapareixer);
        GenerarInstancia();
    }

    void GenerarInstancia(){
        //Vector3 newPosition = transform.InverseTransformPoint(initialDestination);
        troop = Instantiate(troopPrefab, initialDestination, transform.rotation, this.transform);
        troop.GetComponent<TroopMov>().initialTarget = this.gameObject;
        troop.GetComponent<TroopMov>().displacementTarget = transform.localPosition;
        troop.GetComponent<TroopMov>().ChangeInitialDestination(initialDestination);
    }

    public void ChangeInitialDestination(Vector3 destination){
        initialDestination = destination + transform.localPosition;
        if(troop != null){
            troop.GetComponent<TroopMov>().ChangeInitialDestination(destination);
        }
    }

    
}
