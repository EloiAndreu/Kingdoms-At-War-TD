using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopShoot : MonoBehaviour
{  
    public string tagToAttack;
    public List<GameObject> targets;
    Coroutine atacCoroutine;
    public float timeBetweenAtacs = 5f;
    public int minDamage, maxDamage;
    public float detectDist = 40f;
    public int idEnemicAprop = 0;

    public GameObject balaPrefab;
    public Transform posDispararBala;

    void Start(){
        GetComponent<SphereCollider>().radius = detectDist;

        GetAllNearTargets();
    }

    void GetAllNearTargets(){
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i=0; i<allTargets.Length; i++){
            if(Vector3.Distance(transform.position, allTargets[i].transform.position) < detectDist){
                targets.Add(allTargets[i]);
            }
        }
    }

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == tagToAttack){
            targets.Add(coll.gameObject);
            if(atacCoroutine == null) atacCoroutine = StartCoroutine(Atacar());
        }
    }

    void OnTriggerExit(Collider coll){
        if(targets.Contains(coll.gameObject)){
            targets.Remove(coll.gameObject);
            if(targets.Count == 0) StopCoroutine(atacCoroutine);
        }
    }

    void Update(){
        if(targets.Count > 0) ComprovarPrimerEnemic();
    }

    void ComprovarPrimerEnemic(){
        int currentWayPoint = -10000;
        for(int i=0; i<targets.Count; i++){
            if(targets[i] == null) targets.RemoveAt(i);
            else{
                int newWayPoint = targets[i].GetComponent<EnemyMov>().currentWayPoint;
                if(newWayPoint > currentWayPoint){
                    currentWayPoint = newWayPoint;
                    idEnemicAprop = i;
                }
                else if(newWayPoint == currentWayPoint){
                    float distNewTarget =  targets[i].GetComponent<EnemyMov>().DistToWayPoint();
                    float distcurrentTarget =  targets[idEnemicAprop].GetComponent<EnemyMov>().DistToWayPoint();
                    if(distNewTarget < distcurrentTarget){
                        idEnemicAprop = i;
                    }
                }
            }
        }
    }

    IEnumerator Atacar(){
        float dist = Vector3.Distance(transform.position, targets[idEnemicAprop].transform.position);
        if(idEnemicAprop < targets.Count && targets[idEnemicAprop] != null && targets[idEnemicAprop].GetComponent<Health>() != null && dist < detectDist){
            Shoot();
            yield return new WaitForSeconds(timeBetweenAtacs);
            atacCoroutine = StartCoroutine(Atacar()); 
        }
        else{
            atacCoroutine = null;
        }
    }

    void Shoot(){
        GameObject bala = Instantiate(balaPrefab, posDispararBala.position, posDispararBala.rotation);
        bala.GetComponent<Bullet>().target = targets[idEnemicAprop];
        int damage = Random.Range(minDamage, maxDamage);
        bala.GetComponent<Bullet>().damage = damage;
    }
}
