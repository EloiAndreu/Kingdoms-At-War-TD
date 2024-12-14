using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalMapManager : MonoBehaviour
{
    public int currentMapID = 0;
    public GameObject globalMap;
    public List<GameObject> maps;
    public GameObject buttonsAmuntAvall;

    public Button musicButton, soundButton;

    void Start(){
        GameManager.Instance.gameObject.GetComponent<AudioManager>().PlayMusic(1);

        if(!GameManager.Instance.SoundActivated){
            soundButton.onClick.Invoke();
        }

        if(!GameManager.Instance.MusicActivated){
            musicButton.onClick.Invoke();
        }
    }

    public void Amunt(){
        if(currentMapID > 0){
            buttonsAmuntAvall.SetActive(false);
            currentMapID--;
            StartCoroutine(Moure(globalMap, true, 1f, 100));
            StartCoroutine(Moure(maps[currentMapID], false, 1f, 400));
        }
    }

    public void Avall(){
        if(currentMapID < 2){
            buttonsAmuntAvall.SetActive(false);
            currentMapID++;
            StartCoroutine(Moure(globalMap, false, 1f, 100));
            StartCoroutine(Moure(maps[currentMapID-1], true, 1f, 400));
        }
    }

    IEnumerator Moure(GameObject obj, bool amunt, float duracio, float displ){
        Vector3 posicioInicial = obj.transform.position;
        Vector3 posicioFinal;

        if(amunt) posicioFinal = posicioInicial + new Vector3(0, displ, 0);
        else posicioFinal = posicioInicial - new Vector3(0, displ, 0);

        float temps = 0f;

        while (temps < duracio)
        {
            temps += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(posicioInicial, posicioFinal, temps / duracio);

            yield return null;
        }

        obj.transform.position = posicioFinal;
        buttonsAmuntAvall.SetActive(true);
    }
}
