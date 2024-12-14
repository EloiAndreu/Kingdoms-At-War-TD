using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopShoot2 : MonoBehaviour
{
    public float detectDist = 40f;
    public int minDamage, maxDamage;

    public List<GameObject> targets;
    public string tagToAttack;
    //Coroutine atacCoroutine;

    public GameObject balaPrefab;
    public Transform posDispararBala;

    public Animator anim;
    int idEnemicAprop = -1;

    public string atacSoundName;

    void Start(){
        GetComponent<SphereCollider>().radius = detectDist;
    }

    void Update(){
        ComprovarPrimerEnemic();
    }

    //IEnumerator Atacar(){
    public void Atacar(){
        //int idEnemicAprop = ComprovarPrimerEnemic();
        //ComprovarPrimerEnemic();
        if(idEnemicAprop != -1){
            //StartCoroutine(GirarLootAt(targets[idEnemicAprop].transform.position, 0.25f));

            Shoot();//idEnemicAprop);
            //yield return new WaitForSeconds(0f);
        }
        else{
            //atacCoroutine = null;

            //StartCoroutine(GirarLootAt(Vector3.forward, 0.25f));
            StartCoroutine(CanviStateAnimation(0f, 1f));
        }
    }

    public void ComprovarPrimerEnemic(){
        int currentWayPoint = -10000;
        int _idEnemicAprop = -1;
        for(int i=0; i<targets.Count; i++){
            if(targets[i] == null || targets[i].GetComponent<Health>().currentHealth <= 0f) {
                targets.RemoveAt(i);
                i--;
            }
            else{
                int newWayPoint = targets[i].GetComponent<EnemyMov>().currentWayPoint;
                if(newWayPoint > currentWayPoint){
                    currentWayPoint = newWayPoint;
                    _idEnemicAprop = i;
                }
                else if(newWayPoint == currentWayPoint){
                    float distNewTarget =  targets[i].GetComponent<EnemyMov>().DistToWayPoint();
                    float distcurrentTarget =  targets[_idEnemicAprop].GetComponent<EnemyMov>().DistToWayPoint();
                    if(distNewTarget < distcurrentTarget){
                        _idEnemicAprop = i;
                    }
                }
            }
        }
        idEnemicAprop = _idEnemicAprop;
        if(idEnemicAprop != -1) StartCoroutine(GirarLootAt(targets[idEnemicAprop].transform.position, 0.25f));
        else StartCoroutine(CanviStateAnimation(0f, 1f));
        //return idEnemicAprop;
    }

    void Shoot(){//int idEnemicAprop){
        //atacAudioSource.Play();
        AudioManager.Instance.Play(atacSoundName);

        GameObject bala = Instantiate(balaPrefab, posDispararBala.position, posDispararBala.rotation);
        bala.GetComponent<Bullet>().target = targets[idEnemicAprop];
        int damage = Random.Range(minDamage, maxDamage);
        bala.GetComponent<Bullet>().damage = damage;
    }

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == tagToAttack){
            targets.Add(coll.gameObject);
            //if(atacCoroutine == null){
                ComprovarPrimerEnemic();
                if(idEnemicAprop != -1) StartCoroutine(CanviStateAnimation(1f, 1f));
            //} 
        }
    }

    void OnTriggerExit(Collider coll){
        if(targets.Contains(coll.gameObject)){
            targets.Remove(coll.gameObject);
            //ComprovarPrimerEnemic();
        }
    }

    IEnumerator CanviStateAnimation(float targetState, float duration){

        float currentState = anim.GetFloat("State");
        if(currentState != targetState){
            float timeElapsed = 0f;

            while (timeElapsed < duration){
                timeElapsed += Time.deltaTime;
                float newState = Mathf.Lerp(currentState, targetState, timeElapsed / duration);
                anim.SetFloat("State", newState);
                yield return null;
            }

            anim.SetFloat("State", targetState);
        }
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
