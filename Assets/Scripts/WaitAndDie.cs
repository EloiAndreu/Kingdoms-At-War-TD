using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAndDie : MonoBehaviour
{
    public float timeToDie = 1f;

    void Start(){
        StartCoroutine(WaitToDie());
    }

    IEnumerator WaitToDie(){
        yield return new WaitForSeconds(timeToDie);
        Destroy(this.gameObject);
    }
}
