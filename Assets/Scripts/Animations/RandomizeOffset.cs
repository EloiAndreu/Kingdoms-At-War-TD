using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeOffset : MonoBehaviour
{
    public string nameAnimation;
    public bool randomRotation = false;

    void Start()
    {
        if(nameAnimation != ""){
            Animator anim = GetComponent<Animator>();
            float randomOffSet = Random.Range(0f, 1f);
            anim.Play(nameAnimation, 0, randomOffSet);

            float randomSpeed = Random.Range(0f, 1f);
            anim.speed = randomSpeed;
        }

        float randomScale = Random.Range(0.75f, 1.5f);
        this.gameObject.transform.localScale *= randomScale;

        if(randomRotation){
            float randomYRotation = Random.Range(0f, 360f);
            Quaternion newRotation = Quaternion.Euler(0f, randomYRotation, 0f);
            this.gameObject.transform.rotation = newRotation;
        }
    }
}
