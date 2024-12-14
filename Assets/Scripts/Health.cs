using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float currentHealth = 100;
    public float maxHealth = 100;
    public UnityEvent dieEvent, TakeDamageEvent;
    [HideInInspector]
    public int monedesAlMorir = 0;
    public Animator anim;

    NavMeshAgent agent;

    Slider healthSlider;
    Camera mainCamera;
    Transform canvasTransform;
    Quaternion camRotation;
    public GameObject healthSliderPrefab;
    GameObject healthSliderObject;

    void Start(){
        float height = transform.GetComponent<NavMeshAgent>().height;
        healthSliderObject = Instantiate(healthSliderPrefab, transform.position + new Vector3(0f, height+2f, 0f), transform.rotation, this.transform);
        healthSlider = healthSliderObject.transform.GetChild(0).GetComponent<Slider>();

        mainCamera = Camera.main;
        canvasTransform = healthSlider.transform.parent.transform;
        camRotation = mainCamera.transform.rotation;

        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update(){
        if(healthSlider != null){
            /*Vector3 camLocalForward = mainCamera.transform.InverseTransformDirection(Vector3.forward);
            Vector3 camLocalUp = mainCamera.transform.InverseTransformDirection(Vector3.up);
            healthSlider.transform.parent.transform.LookAt(-mainCamera.transform.forward);*/
            canvasTransform.rotation = camRotation;
        } 
    }

    public bool TakeDamage(float damage){
        currentHealth -= (int) damage;
        if(healthSlider != null) healthSlider.value = currentHealth/maxHealth;
        Mathf.Clamp(currentHealth, 0f, maxHealth);
        TakeDamageEvent.Invoke();
        if(currentHealth <= 0f) {
            //Die();
            if(agent != null) agent.isStopped = true;

            if(anim != null){
                healthSliderObject.SetActive(false);
                anim.SetBool("Mort", true);
                LevelManager.Instance.AddCoins(monedesAlMorir);
            }
            else Die();
            return true;
        }
        else return false;
    }

    public void Die(){
        //if(anim != null) anim.SetBool("Mort", true);
        
        dieEvent.Invoke();
        LevelManager.Instance.CheckEnemicsRestants();
        Destroy(this.gameObject);
    }
}
