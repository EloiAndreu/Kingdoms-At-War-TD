using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCami : MonoBehaviour
{
    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == "Enemy"){
            coll.gameObject.GetComponent<EnemyMov>().EnemicFinalCami();
        }
    }
}
