using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enciclopedia : MonoBehaviour
{
    int[] nivells;

    public List<int> numImgADesblocXNiv;
    public GameObject noFoundText;

    void Awake(){
        nivells = GameData.Instance.gameData.nivells;

        for(int i=0; i<transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(false);
        }

        for(int i=0; i<nivells.Length; i++){
            if(nivells[i] != 0){
                if(numImgADesblocXNiv[i]<= 0) noFoundText.SetActive(true);
                else noFoundText.SetActive(false);
                for(int j=0; j<numImgADesblocXNiv[i]; j++){
                    if(j < transform.childCount) transform.GetChild(j).gameObject.SetActive(true);
                }
            }
        }
    }


}
