using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int levelID = -1;

    int initialsCoins = 1000;
    public int currentCoins;
    int maxCoins = 99999;
    public TMP_Text coinsText1;

    public int initialVides = 20;
    int currentVides;
    public TMP_Text videsText1, videsText2;

    public bool lastWaveSpawned = false;
    public GameObject perdreImg, guanyarImg;

    public Color colorTreureVidaVignette;
    Color vignetteInicialColor;
    public Volume globalVolume;
    Vignette vignette;
    Coroutine vignetteCanviarColor;
    
    void Awake(){
        Instance = this;
    }

    void Start(){
        if(GameManager.Instance != null) levelID = GameManager.Instance.currentLevelPlaying;
        initialsCoins = GetComponent<WaveGenerator>().levelObj.initialCoins;
        currentCoins = initialsCoins;
        coinsText1.text = currentCoins.ToString();

        currentVides = initialVides;
        videsText1.text = currentVides.ToString();
        videsText2.text = currentVides.ToString();

        //vignette = globalVolume.profile.GetComponent<Vignette>();
        if (globalVolume.profile.TryGet<Vignette>(out vignette)){
            vignette = vignette;
            vignetteInicialColor = vignette.color.value;
        }
        else{
            Debug.LogError("No s'ha trobat el Vignette al Global Volume.");
            return;
        }

        GameManager.Instance.gameObject.GetComponent<AudioManager>().PlayMusic(0);
    }

    public void CheckEnemicsRestants(){
        StartCoroutine(CheckEnemicsRestantsCoroutine());
    }
    
    IEnumerator CheckEnemicsRestantsCoroutine(){
        if(lastWaveSpawned){
            yield return new WaitForSeconds(3f);
            int enemicsRestants = GameObject.FindGameObjectsWithTag("Enemy").Length;
            Debug.Log(enemicsRestants);
            if(enemicsRestants < 1){
                Guanyar();
            }
        }
    }

    public void AddCoins(int _coins){
        if((currentCoins + _coins) > maxCoins){
            currentCoins = maxCoins;
        }
        else currentCoins += _coins;

        coinsText1.text = currentCoins.ToString();
    }

    public bool BuySomething(int price){
        if(currentCoins < price) return false;
        else{
            currentCoins -= price;

            coinsText1.text = currentCoins.ToString();
            return true;
        }
    }

    public void TreureVides(int _vides){
        if(vignetteCanviarColor != null) StopCoroutine(vignetteCanviarColor);
        vignetteCanviarColor = StartCoroutine(CanviarColorVignette(2f));
        currentVides = currentVides - _vides;
        
        if(currentVides <= 0){
            currentVides = 0;
            //PERDRE NIVELL
            Perdre();
        }

        videsText1.text = currentVides.ToString();
        videsText2.text = currentVides.ToString();
    }

    public void Guanyar(){
        if(GameData.Instance != null) GameData.Instance.AddLevelSuperat(levelID);
        guanyarImg.SetActive(true);
        StartCoroutine(TornarAlMenu());
    }

    public void Perdre(){
        perdreImg.SetActive(true);
        StartCoroutine(TornarAlMenu());
    }

    IEnumerator TornarAlMenu(){
        yield return new WaitForSeconds(5f);
        Debug.Log("Ara");
        SceneController sc = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SceneController>();
        if(sc != null){
            if(SceneManager.GetActiveScene().buildIndex == 10) sc.LoadScene(11);
            else sc.LoadScene(1);
        }
    }

    IEnumerator CanviarColorVignette(float duration){
        float timeElapsed = 0f;
        vignette.color.value = colorTreureVidaVignette;

        while (timeElapsed < duration){

            Color currentColor = Color.Lerp(colorTreureVidaVignette, vignetteInicialColor, timeElapsed/duration);
            vignette.color.value = currentColor;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        vignette.color.value = vignetteInicialColor;
    }
}
