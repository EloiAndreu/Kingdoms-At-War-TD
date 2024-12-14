using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopShootParticles : MonoBehaviour
{
    public float detectDist = 40f;
    public int minDamage, maxDamage;

    public List<GameObject> targets;
    public string tagToAttack;

    public GameObject balaPrefab;
    public GameObject sphereDamage;
    public Transform posDispararBala;

    public Animator anim;
    int idEnemicAprop = -1;
    public float delayDamage = 0f;

    //public AudioSource atacAudioSource;
    public string atacSoundName;

    //Coroutine canviStateCoroutine = null;

    void Start(){
        GetComponent<SphereCollider>().radius = detectDist;
    }

    void Update(){
        ComprovarPrimerEnemic();
    }

    public void Atacar(){
        if(idEnemicAprop != -1){
            Shoot();
        }
        else{
            //if(canviStateCoroutine != null) StopCoroutine(canviStateCoroutine);
            //canviStateCoroutine = StartCoroutine(CanviStateAnimation(0f, 1f));
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
        else{
            //if(canviStateCoroutine != null) StopCoroutine(canviStateCoroutine);
            //canviStateCoroutine = StartCoroutine(CanviStateAnimation(0f, 1f));
            StartCoroutine(CanviStateAnimation(0f, 1f));
        } 
        //return idEnemicAprop;
    }

    void Shoot(){//int idEnemicAprop){
        //atacAudioSource.Play();
        AudioManager.Instance.Play(atacSoundName);

        GameObject bala = Instantiate(balaPrefab, targets[idEnemicAprop].transform.position, Quaternion.identity);//, targets[idEnemicAprop].transform);
        StartCoroutine(WaitAndTakeDamage(bala));
    }

    IEnumerator WaitAndTakeDamage(GameObject bala){
        yield return new WaitForSeconds(delayDamage);
        int damage = Random.Range(minDamage, maxDamage);
        if(idEnemicAprop != -1){
            //targets[idEnemicAprop].GetComponent<Health>().TakeDamage(damage);
            GameObject sphere = Instantiate(sphereDamage, targets[idEnemicAprop].transform.position, Quaternion.identity);
            sphere.GetComponent<SphereDamage>().damage = damage;
        }
    }

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == tagToAttack){
            targets.Add(coll.gameObject);
            //if(atacCoroutine == null){
                //if(canviStateCoroutine != null) StopCoroutine(canviStateCoroutine);
                //canviStateCoroutine = StartCoroutine(CanviStateAnimation(1f, 1f));
                StartCoroutine(CanviStateAnimation(1f, 1f));
                ComprovarPrimerEnemic();
                //StartCoroutine(CanviStateAnimation(1f, 1f));
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
