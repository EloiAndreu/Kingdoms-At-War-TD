using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Events;

    /*[Serializable]
    public class Onades{
        public List<GameObject> enemies;
        public List<int> camins;

        [HideInInspector]
        public int waveValue;
        [HideInInspector]
        public List<EnemyGroup> enemyGroups;
    }

    public class EnemyGroup{
        public int nEnemies;
        public GameObject enemyPrefab;

        public EnemyGroup(int nEnemies, GameObject enemyPrefab)
        {
            this.nEnemies = nEnemies;
            this.enemyPrefab = enemyPrefab;
        }
    }*/

public class WaveGenerator2 : MonoBehaviour
{
    /*public int minValueDiff, maxValueDiff;
    public AnimationCurve diffCurve;
    public List<Onades> waves;*/
    
    /*int minValueDiff, maxValueDiff;
    AnimationCurve diffCurve;
    List<WaveObject.Onades> waves;
    public WaveObject levelObj;

    public int currentWave = -1;
    Coroutine currentCoroutine;
    List<Coroutine> wavesCoroutines = new List<Coroutine>();
    Coroutine fillImgCoroutine;

    public float TimeBetweenEnemies = 1f;
    public float timeToNextWave = 15f;

    GameObject spawnObject;
    Vector3[] spawnPositions;
    GameObject enemyParent;

    public TMP_Text waveText1, waveText2;
    public Animator waveTextAnim;
    public Button waveButton;
    public Image imgFillTempsEnrere;
    public TMP_Text tempsText;

    public UnityEvent startWaveEvent;

    void Start(){
        minValueDiff = levelObj.minValueDiff;
        maxValueDiff = levelObj.maxValueDiff;
        diffCurve = levelObj.diffCurve;
        waves =  levelObj.waves;     

        spawnObject = GameObject.FindGameObjectWithTag("SpawnPosition");
        spawnPositions = new Vector3[spawnObject.transform.childCount];

        for(int i=0; i<spawnObject.transform.childCount; i++){
            spawnPositions[i] = spawnObject.transform.GetChild(i).transform.position;
        }
        enemyParent = GameObject.FindGameObjectWithTag("EnemyParent");

        waveText1.text = "0/" + waves.Count;
        waveText2.text = "0/" + waves.Count;
        tempsText.text = "START";

        SetupWaves();
    }

    void SetupWaves(){
        int incrementValue = (int)(maxValueDiff-minValueDiff)/waves.Count;
        //Debug.Log("Increment Value: " + incrementValue); //

        for(int i=0; i<waves.Count; i++){

            int value = minValueDiff + incrementValue*(i+1);
            float valueCurve = diffCurve.Evaluate((float)(value-minValueDiff)/(maxValueDiff-minValueDiff));
            float finalValue = valueCurve*(maxValueDiff-minValueDiff)+minValueDiff;
            //Debug.Log("Final Value: " + finalValue);
            waves[i].waveValue = (int)finalValue;
            //Debug.Log("Wave Value: " + value);

            waves[i].enemyGroups = new List<WaveObject.EnemyGroup>();
            
            int diffSpendXEnemy = (int)value/waves[i].enemies.Count;
            for(int j=0; j<waves[i].enemies.Count; j++){
                int enemyValue = waves[i].enemies[j].GetComponent<EnemyMov>().enemyValue;
                int nEnemies = (int) diffSpendXEnemy/enemyValue;
                //Debug.Log("Enemies: " + j + " Quantity: " + nEnemies);
                waves[i].enemyGroups.Add(new WaveObject.EnemyGroup(nEnemies, waves[i].enemies[j]));
            }
        }
    }

    public void StartNewWave(){
        if(currentWave < waves.Count-1){
            startWaveEvent.Invoke();
            tempsText.gameObject.SetActive(false);

            if(fillImgCoroutine != null) StopCoroutine(fillImgCoroutine);
            imgFillTempsEnrere.fillAmount = 0f;
            waveButton.interactable = false;

            currentWave++;
            
            waveTextAnim.SetTrigger("NextWave");
            waveText1.text = (currentWave+1) + "/" + waves.Count;
            waveText2.text = (currentWave+1) + "/" + waves.Count;

            currentCoroutine = StartCoroutine(InstanceWave(waves[currentWave]));
            wavesCoroutines.Add(currentCoroutine);
        }
    }

    IEnumerator InstanceWave(WaveObject.Onades wave){
        while(wave.enemyGroups.Count > 0){
            int randomEnemy = UnityEngine.Random.Range(0, wave.enemyGroups.Count);
            int randomCami = UnityEngine.Random.Range(0, wave.camins.Count);

            int cami = 0;
            if(wave.camins.Count > 0) cami = wave.camins[randomCami];

            GameObject enemyPrefab = wave.enemyGroups[randomEnemy].enemyPrefab;
            int enemiesXGroup = enemyPrefab.GetComponent<EnemyMov>().maxEnemiesXGroup;
            int i=0;
            while(i<enemiesXGroup && wave.enemyGroups[randomEnemy].nEnemies > 0){
                if(enemyPrefab != null) InstanceEnemy(enemyPrefab, cami);
                else Debug.Log("Enemic no generat");
                
                wave.enemyGroups[randomEnemy].nEnemies--;
                i++;
                yield return new WaitForSeconds(0.5f);
            }

            if(wave.enemyGroups[randomEnemy].nEnemies <= 0) wave.enemyGroups.RemoveAt(randomEnemy);
            float timeBEnemies = UnityEngine.Random.Range(TimeBetweenEnemies, TimeBetweenEnemies+0.5f);
            if(enemiesXGroup > 1) yield return new WaitForSeconds(timeBEnemies+1f);
            else yield return new WaitForSeconds(timeBEnemies);
        }

        wavesCoroutines.RemoveAt(0);
        if(currentWave == waves.Count-1 && wavesCoroutines.Count == 0){
            LevelManager.Instance.lastWaveSpawned = true;
        }
        currentCoroutine = null;

        if(currentWave < waves.Count-1){
            waveButton.interactable = true;
            fillImgCoroutine = StartCoroutine(FillImageTempsEnrere());
        }
        
        yield return null;
        //yield return new WaitForSeconds(wave.timeToNextWave);
    }

    void InstanceEnemy(GameObject enemyPrefab, int cami){
        Vector2 randomDir = UnityEngine.Random.insideUnitCircle * 5f;
        Vector3 displPos = new Vector3(randomDir.x, 0, randomDir.y);
        Vector3 newPosition = spawnPositions[cami] + displPos;

        GameObject enemy = Instantiate(enemyPrefab, newPosition, Quaternion.identity, enemyParent.transform);
        enemy.GetComponent<EnemyMov>().displPos = displPos;
        enemy.GetComponent<EnemyMov>().cami = cami;
    }

    IEnumerator FillImageTempsEnrere(){
        tempsText.gameObject.SetActive(true);
        float tempsTranscorregut = 0f;

        float duration =  timeToNextWave;
        while(tempsTranscorregut < duration){
            tempsTranscorregut += Time.deltaTime;
            imgFillTempsEnrere.fillAmount = Mathf.Clamp01(tempsTranscorregut / duration);
            tempsText.text = (int)(duration-tempsTranscorregut) + "s";

            yield return null;
        }

        imgFillTempsEnrere.fillAmount = 1f;
        StartNewWave();
    }*/
}
