using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDamage : MonoBehaviour
{
    public float damage = 0f;

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == "Enemy"){
            coll.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
