using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactuar : MonoBehaviour
{
    public UnityEvent[] interactEvents;

    public void Interactua(int eventID){
        if(eventID < interactEvents.Length){
            interactEvents[eventID].Invoke();
        }
    }
}
