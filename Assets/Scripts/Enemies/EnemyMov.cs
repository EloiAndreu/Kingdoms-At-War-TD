using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMov : MonoBehaviour
{
    public List<GameObject> wayPoints;
    public int currentWayPoint = 0;
    public float waypointRadius = 1f;

    public float minDamage, maxDamage;
    public float timeBetweenAtacs = 5f;
    public int videsARestar = 1;
    //public int minMonedesAlMorir, maxMonedesAlMorir;
    public int monedesAlMorir;

    public string estat = "caminar";
    public Vector3 displPos;

    NavMeshAgent agent;
    public Animator anim;
    GameObject target;
    public int cami = 0;

    public string atacSoundName;

    public int enemyValue;
    public int maxEnemiesXGroup = 1;

    void Start(){
        if(GetComponent<Health>() != null){
            int monedesARetornar = Random.Range(monedesAlMorir-1, monedesAlMorir+1);
            GetComponent<Health>().monedesAlMorir = monedesARetornar;
        }

        anim.SetInteger("State", 1);
        AfegirWayPoints(); 
        agent.SetDestination((wayPoints[currentWayPoint].transform.position + displPos));
    }

    void AfegirWayPoints(){
        GameObject caminsGameObject = GameObject.FindGameObjectWithTag("WayPoints");
        GameObject wayPointsParent = caminsGameObject.transform.GetChild(cami).gameObject;
        for(int i=0; i<wayPointsParent.transform.childCount; i++){
            wayPoints.Add(wayPointsParent.transform.GetChild(i).gameObject);
        }

        agent = GetComponent<NavMeshAgent>();
    }

    public void Caminar(){
        anim.gameObject.transform.localRotation = Quaternion.identity;
        anim.SetInteger("State", 1);
        //Debug.Log(anim.gameObject);
        //transform.LookAt((wayPoints[currentWayPoint].transform.position + displPos));
        //.LookAt((wayPoints[currentWayPoint].transform.position + displPos));
        estat = "caminar";
        agent.isStopped = false;
    }

    void Update(){
        if(estat == "caminar"){ 
            if(!agent.pathPending && agent.remainingDistance < waypointRadius){
                currentWayPoint++;
                if(currentWayPoint < wayPoints.Count){
                    agent.SetDestination((wayPoints[currentWayPoint].transform.position + displPos));
                }
            }
        }

        if(estat == "esperar"){
            if(target == null){
                Caminar();
            }
        }
    }

    public void EnemicFinalCami(){
        LevelManager.Instance.TreureVides(videsARestar);
        Destroy(this.gameObject);
    }

    public void Esperar(GameObject enemyToAtac){
        StartCoroutine(GirarLootAt(enemyToAtac.transform.position, 0.25f));
        target = enemyToAtac;
        anim.SetInteger("State", 0);
        estat = "esperar";
        agent.isStopped = true; 
    }

    public void Combatir(GameObject enemyToAtac){
        target = enemyToAtac;
        anim.SetInteger("State", 2);
        estat = "combat";
        transform.LookAt(enemyToAtac.transform);
        //StartCoroutine(Atacar(enemyToAtac));
    }

    public void Atac(){
        if(target != null && target.GetComponent<Health>() != null){
            float damage = Random.Range(minDamage, maxDamage);
            bool deadEnemy = target.GetComponent<Health>().TakeDamage(damage);
            if(deadEnemy){
                Caminar();
            }
            /*else{
                yield return new WaitForSeconds(timeBetweenAtacs);
                StartCoroutine(Atacar(enemyToAtac)); 
            }*/
        }

        if(target == null){
            Caminar();
        }
    }

    IEnumerator Atacar(GameObject enemyToAtac){
        if(enemyToAtac != null && enemyToAtac.GetComponent<Health>() != null){
            float damage = Random.Range(minDamage, maxDamage);
            bool deadEnemy = enemyToAtac.GetComponent<Health>().TakeDamage(damage);
            if(deadEnemy){
                Caminar();
            }
            else{
                yield return new WaitForSeconds(timeBetweenAtacs);
                StartCoroutine(Atacar(enemyToAtac)); 
            }
        }
    }

    public float DistToWayPoint(){
        return Vector3.Distance(this.transform.position, wayPoints[currentWayPoint].transform.position);
    }

    public void DeixarDeCombatir(){
        target = null;
        Caminar();
    }

    public void PlayAtacSound(){
        if(atacSoundName == "Sword"){
            int swordClipID = Random.Range(1, 3);
            AudioManager.Instance.Play("Sword " + swordClipID);
        }
        else AudioManager.Instance.Play(atacSoundName);
    }

    IEnumerator GirarLootAt(Vector3 posTarget, float duration){
        Vector3 direction = posTarget - transform.position;
        direction.y = 0f;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / duration);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
