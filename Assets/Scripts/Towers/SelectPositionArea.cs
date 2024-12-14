using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPositionArea : MonoBehaviour
{
    GameObject selectionObject;
    //public GameObject troopPositions; 
    public List<TroopSpawn> tropes; 

    void Start(){
        selectionObject = GameObject.FindGameObjectWithTag("UISelectTower");
    }
    
    private void OnMouseDown(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)){
            if (hit.collider == GetComponent<Collider>()){
                Vector3 hitPosition = hit.point;

                for(int i=0; i<tropes.Count; i++){
                    tropes[i].ChangeInitialDestination(new Vector3(hitPosition.x, 0f, hitPosition.z));
                }

                //troopPositions.transform.position = new Vector3(hitPosition.x, 0f, hitPosition.z);
                selectionObject.GetComponent<SelectionUIController>().TropesMogudes();
                this.gameObject.SetActive(false);
                //Debug.Log("Tropes Mogudes");
            }
            else Debug.Log("No ha xocat amb collider");
        }
    }

    public void ChangeInitialDestinationPosition(Vector3 destination){
        for(int i=0; i<tropes.Count; i++){
            tropes[i].ChangeInitialDestination(destination);
        }
    }
}
