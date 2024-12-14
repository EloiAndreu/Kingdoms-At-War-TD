using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTeleporter : MonoBehaviour
{
    public GameObject nextPosition;

    void OnTriggerEnter(Collider coll){
        if(coll.tag == "Enemy"){
            coll.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            coll.gameObject.transform.localPosition = nextPosition.transform.position;
            coll.gameObject.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
