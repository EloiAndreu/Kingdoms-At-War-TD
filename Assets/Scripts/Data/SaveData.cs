using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int[] nivells = new int[100];

    public SaveData(int[] _nivells){
        nivells = _nivells;
    }
}
