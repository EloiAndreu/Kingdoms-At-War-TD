using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TroopMov : MonoBehaviour
{
    public GameObject initialTarget;
    public Vector3 displacementTarget;
    Vector3 initialDestination;
    public GameObject target;
    public NavMeshAgent agent;
    public Animator anim;

    public string tagToAttack;
    public float rangDetectEnemic = 10f;

    public float minDamage, maxDamage;
    public float distToStop = 10f;
    public string estat = "esperar";
    SphereCollider sphereColl;

    Vector3 desti;

    public string atacSoundName;

    public List<GameObject> enemies;

    void Start(){
        sphereColl = GetComponent<SphereCollider>();
        if(initialTarget != null){ 
            if(initialDestination == null) initialDestination = initialTarget.transform.position + displacementTarget;
            agent.SetDestination(initialDestination);
        }
    }

    public void Esperar(){
        sphereColl.enabled = true;
        estat = "esperar";
        agent.isStopped = true; 
        anim.SetInteger("State", 0);
    }

    public void Caminar(GameObject _target){
        target = _target;
        estat = "caminar";
        agent.isStopped = false;
        anim.SetInteger("State", 1);

        if(target.tag == tagToAttack) target.transform.GetComponent<EnemyMov>().Esperar(this.gameObject);
    }

    void Update(){
        if(estat == "caminar"){
            sphereColl.enabled = false;
            float distAturar = distToStop;
            if(target == null){
                desti = initialDestination;
            }
            else {
                desti = target.transform.position;
                Vector3 dir = (desti - transform.position).normalized;
                desti = desti - dir * 6;
            }
            
            agent.SetDestination(desti);
            float dist = Vector3.Distance(transform.position, desti);

            if(dist <= distAturar){
                Esperar();
            }
        }

        if(estat == "esperar"){
            if(target != null && target.tag == tagToAttack){
                Combatir(target);
                target.transform.GetComponent<EnemyMov>().Combatir(this.gameObject);
            }
            else if(target == null){
                GameObject enemyToFollow = ObtenirEnemicsDinsRang(tagToAttack);
                if(enemyToFollow != null){
                    Caminar(enemyToFollow);
                }
            }
        }
    }

    GameObject ObtenirEnemicsDinsRang(string tagEnemic){
        GameObject tagetResult = null;
        float distMin = 100000f;
        //GameObject[] enemics = GameObject.FindGameObjectsWithTag(tagEnemic);
        
        for(int i=0; i<enemies.Count; i++){
            if(enemies[i] == null) enemies.Remove(enemies[i]);
            else if(enemies[i].GetComponent<EnemyMov>().estat == "caminar"){
                float dist = Vector3.Distance(enemies[i].transform.position, transform.position);
                if(dist < rangDetectEnemic && dist < distMin){
                    distMin = dist;
                    tagetResult = enemies[i];
                }
            }
        }

        return tagetResult;
    }

    void Combatir(GameObject enemyToAtac){
        anim.SetInteger("State", 2);
        estat = "combat";
        agent.isStopped = true;
        StartCoroutine(GirarLootAt(enemyToAtac.transform.position, 0.25f));
    }

    public void Atac(){
        if(estat == "combat"){
            if(target != null && target.GetComponent<Health>() != null){
                float damage = Random.Range(minDamage, maxDamage);
                bool deadEnemy = target.GetComponent<Health>().TakeDamage(damage);
                if(deadEnemy){
                    GameObject enemyToFollow = ObtenirEnemicsDinsRang(tagToAttack);
                    if(enemyToFollow != null){
                        Caminar(enemyToFollow);
                    }
                    else{
                        Caminar(initialTarget);
                        target = null;
                    }
                }
            }
            else{
                Caminar(initialTarget);
                target = null;
            } 
        }
    }

    /*void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == tagToAttack){
            if(estat == "esperar" && coll.gameObject.GetComponent<EnemyMov>().estat == "caminar"){
                Caminar(coll.gameObject);
            }
        }
    }*/

    
    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == tagToAttack){
            enemies.Add(coll.gameObject);
        }
    }

    void OnTriggerExit(Collider coll){
        if(coll.gameObject.tag == tagToAttack){
            enemies.Remove(coll.gameObject);
        }
    }

    public void ChangeInitialDestination(Vector3 newDestination){
        initialDestination = newDestination + displacementTarget;
        if(target != null && target.tag == tagToAttack) target.transform.GetComponent<EnemyMov>().DeixarDeCombatir();
        target = null;
        agent.isStopped = false; 
        estat = "caminar";
        anim.SetInteger("State", 1);
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

    public void PlayAtacSound(){
        if(atacSoundName == "Sword"){
            int swordClipID = Random.Range(1, 3);
            AudioManager.Instance.Play("Sword " + swordClipID);
        }
        else AudioManager.Instance.Play(atacSoundName);
    }
}
