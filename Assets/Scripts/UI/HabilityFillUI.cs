using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HabilityFillUI : MonoBehaviour
{
    public Image imgFillTempsEnrere;
    public Button habilityButton;
    Coroutine fillImgCoroutine;
    public float duration = 5f;
    public TMP_Text tempsText;

    void Start(){
        tempsText.text = "Ready";
    }

    public void StartCountDown(){
        habilityButton.interactable = false;
        imgFillTempsEnrere.fillAmount = 1f;
        fillImgCoroutine = StartCoroutine(FillImageTempsEnrere());
    }

    IEnumerator FillImageTempsEnrere(){
        float tempsTranscorregut = 0f;

        while(tempsTranscorregut < duration){
            tempsTranscorregut += Time.deltaTime;
            imgFillTempsEnrere.fillAmount = Mathf.Clamp01(1 - (tempsTranscorregut / duration));
            tempsText.text = (int)(duration-tempsTranscorregut) + "s";

            yield return null;
        }

        imgFillTempsEnrere.fillAmount = 0f;
        habilityButton.interactable = true;
        habilityButton.gameObject.GetComponent<DragUI>().potArrossegar = true;
        tempsText.text = "Ready";
    }
}
