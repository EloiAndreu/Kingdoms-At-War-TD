using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public List<GameObject> wayPoints;
    public int currentWayPoint = 0;
    public float waypointRadius = 1f;

    public float damage = 10f;
    public float timeBetweenAtacs = 5f;
    public float distToStop = 10f;
    public float rangDetectEnemic = 10f;

    public string tagToAttack;
    public string estat = "voltant";
    public Transform target;
    GameObject enemyToAtac;

    NavMeshAgent agent;
    public Animator anim;

    void Start(){
        GameObject wayPointsParent = GameObject.FindGameObjectWithTag("WayPoints");
        for(int i=0; i<wayPointsParent.transform.childCount; i++){
            wayPoints.Add(wayPointsParent.transform.GetChild(i).gameObject);
        }

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPoints[currentWayPoint].transform.position);
    }

    void Update(){
        if(estat == "voltant"){ 
            if(target == null){
                if(currentWayPoint >= wayPoints.Count){
                    FinalDeTrajecte();
                    return;
                }

                if(!agent.pathPending && agent.remainingDistance < waypointRadius){
                    currentWayPoint++;
                    if(currentWayPoint < wayPoints.Count){
                        agent.SetDestination(wayPoints[currentWayPoint].transform.position);
                    }
                }
            }
            else if(target.tag == tagToAttack){
                agent.isStopped = false; 
                agent.SetDestination(target.position);

                float dist = Vector3.Distance(transform.position, target.position);
                if(target.tag == tagToAttack && dist < distToStop){
                    if(estat != "combat") Combat();
                }
            }
        }

        if(estat == "combat"){
            if(enemyToAtac == null) estat = "voltant";
        }
    }

    void FinalDeTrajecte(){
        //Restar Vides
        Destroy(this.gameObject);
    }

    //void OnTriggerEnter(Collider coll){
    public void SetEnemy(GameObject obj){
        if(obj.tag == tagToAttack){
            //Debug.Log("Ara?");

            //GameObject enemyToFollow = ObtenirEnemicsDinsRang(tagToAttack);
            GameObject enemyToFollow = obj;
            if(enemyToFollow != null){
                target = enemyToFollow.transform;
                if(target != null) agent.SetDestination(target.position);
            }
            
        }
    }

    GameObject ObtenirEnemicsDinsRang(string tagEnemic){
        GameObject tagetResult = null;
        float distMin = 100000f;
        GameObject[] enemics = GameObject.FindGameObjectsWithTag(tagEnemic);
        
        for(int i=0; i<enemics.Length; i++){
            float dist = Vector3.Distance(enemics[i].transform.position, transform.position);
            if(dist < rangDetectEnemic && dist < distMin){
                distMin = dist;
                tagetResult = enemics[i];
            }
        }

        return tagetResult;
    }

    void Combat(){
        anim.SetFloat("State", 1);
        estat = "combat";
        agent.isStopped = true;
        enemyToAtac = target.gameObject;

        StartCoroutine(Atacar());
    }

    IEnumerator Atacar(){
        if(enemyToAtac != null && enemyToAtac.GetComponent<Health>() != null){
            enemyToAtac.GetComponent<Health>().TakeDamage(damage);
        }
        else{
            agent.isStopped = false;
            enemyToAtac = null;
            target = null;
            agent.SetDestination(wayPoints[currentWayPoint].transform.position);
            anim.SetFloat("State", 0);
            yield return null;
        }
        yield return new WaitForSeconds(timeBetweenAtacs);
        StartCoroutine(Atacar()); 
    }
}
