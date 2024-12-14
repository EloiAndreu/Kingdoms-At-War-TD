using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troop : MonoBehaviour
{
    public Transform target;
    public Transform initialTarget;
    public NavMeshAgent agent;
    public Animator anim;
    public GameObject enemyToAtac;

    public string tagToAttack;
    public float rangDetectEnemic = 10f;
    public float maxDistInitialPos = 50f;

    public float damage = 10f;
    public float timeBetweenAtacs = 5f;
    public float distToStop = 10f;
    public string estat = "voltant";

    Coroutine atacCoroutine = null;

    void Start(){
        target = initialTarget;
    }

    void Update(){
        if(estat == "voltant"){
            if(target != null){ //Si tenim target enemic
                anim.SetFloat("State", 0.5f);
                agent.isStopped = false; 
                agent.SetDestination(target.position);
                if(target.tag == tagToAttack) enemyToAtac = target.gameObject;
                
                float distInitalPos = Vector3.Distance(transform.position, target.position);
                if(distInitalPos > maxDistInitialPos){
                    target = null;
                }

                float dist = Vector3.Distance(transform.position, target.position);
                if(dist < distToStop){
                    if(target.tag == tagToAttack && estat != "combat" && atacCoroutine == null){
                        Combat();
                    }
                    target = null;
                }
            }
            else{ //Si no tenim target
                if(enemyToAtac == null){
                    float dist = Vector3.Distance(transform.position, initialTarget.position);
                    if(dist < distToStop){
                        anim.SetFloat("State", 0);
                        agent.isStopped = true;
                    }
                    else{
                        anim.SetFloat("State", 0.5f);
                        agent.isStopped = false; 
                        agent.SetDestination(initialTarget.position);
                    }
                }
            }
        }
            
        if(estat == "combat"){
            if(enemyToAtac == null) estat = "voltant";
        }
    }

    GameObject ObtenirEnemicsDinsRang(string tagEnemic){
        GameObject tagetResult = null;
        float distMin = 100000f;
        GameObject[] enemics = GameObject.FindGameObjectsWithTag(tagEnemic);
        
        for(int i=0; i<enemics.Length; i++){
            if(enemics[i].GetComponent<Enemy>().target == null){
                float dist = Vector3.Distance(enemics[i].transform.position, transform.position);
                if(dist < rangDetectEnemic && dist < distMin){
                    distMin = dist;
                    tagetResult = enemics[i];
                }
            }
        }

        return tagetResult;
    }

    void Combat(){
        anim.SetFloat("State", 1);
        estat = "combat";
        agent.isStopped = true;
        target = null;

        atacCoroutine = StartCoroutine(Atacar());
    }

    IEnumerator Atacar(){
        //Si existeix l'enemic i té vida... TREURE VIDA
        if(enemyToAtac != null && enemyToAtac.GetComponent<Health>() != null){
            enemyToAtac.GetComponent<Health>().TakeDamage(damage);
        }
        
        //Si no tenim enemic o l'actual no te vida... COMPROVAR ENEMICS APROP I ATURAR
        if(enemyToAtac == null || enemyToAtac.GetComponent<Health>().currentHealth == 0){
            Debug.Log("Ara");
            //yield return new WaitForSeconds(timeBetweenAtacs);
            
            GameObject enemyToFollow = ObtenirEnemicsDinsRang(tagToAttack);
            if(enemyToFollow != null) {
                target = enemyToFollow.transform;
                enemyToAtac = target.gameObject;
                enemyToFollow.GetComponent<Enemy>().SetEnemy(this.gameObject);
                yield return new WaitForSeconds(timeBetweenAtacs);
                atacCoroutine = StartCoroutine(Atacar()); 
            }
            else{
                StopCoroutine(atacCoroutine);
                atacCoroutine = null;
                yield return null;
            }
        }
        else{
            yield return new WaitForSeconds(timeBetweenAtacs);
            atacCoroutine = StartCoroutine(Atacar()); 
        }
        
    }

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == tagToAttack){
            if(enemyToAtac == null || (target != null && target.tag != tagToAttack)){
                if(coll.gameObject.GetComponent<Enemy>().target == null){
                    //Debug.Log("Ara");
                    agent.isStopped = true;

                    target = coll.gameObject.transform;
                    enemyToAtac = target.gameObject;
                    coll.gameObject.GetComponent<Enemy>().SetEnemy(this.gameObject);
                }
            }
        }
    }
}
