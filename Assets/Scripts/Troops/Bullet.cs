using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public float speed = 10f;
    public float damage = 0f;
    Vector3 direction;
    public bool lookAtTarget = false;

    void Update(){
        if(target != null){
            Vector3 targetPos = target.transform.position;
            targetPos.y += 5f;
            direction = (targetPos - transform.position).normalized;

            if(lookAtTarget){
                transform.LookAt(target.transform.position);
            }
        }
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision coll){
        if(coll.gameObject.tag == "Enemy"){
            if (target != null && target.GetComponent<Health>() != null){
                target.GetComponent<Health>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else Destroy(gameObject);
    }

}
