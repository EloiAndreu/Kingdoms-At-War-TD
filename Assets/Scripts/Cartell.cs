using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartell : MonoBehaviour
{
    void Start()
    {
        Vector3 targetPosition = new Vector3(-1f, 0f, 0f);
        Quaternion globalRotation = Quaternion.LookRotation(targetPosition, Vector3.up);
        transform.rotation = globalRotation;
    }
}
