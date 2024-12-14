using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Events;

    [Serializable]
    public class Wave{
        public List<Group> groups;
        public float TimeBetweenNextWave;
    }

    [Serializable]
    public class Group{
        public GameObject prefabEnemy;
        public int quantity;
        public int cami = 0;
    }

public class EnemySpawnner : MonoBehaviour
{
    public List<Wave> waves;
    public int currentWave = -1;
    Coroutine currentCoroutine;
    List<Coroutine> wavesCoroutines = new List<Coroutine>();
    Coroutine fillImgCoroutine;

    public float TimeBetweenEnemies = 0.5f;
    public float TimeBetweenGroups = 5f;

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
        spawnObject = GameObject.FindGameObjectWithTag("SpawnPosition");
        spawnPositions = new Vector3[spawnObject.transform.childCount];

        for(int i=0; i<spawnObject.transform.childCount; i++){
            spawnPositions[i] = spawnObject.transform.GetChild(i).transform.position;
        }
        enemyParent = GameObject.FindGameObjectWithTag("EnemyParent");

        waveText1.text = "0/" + waves.Count;
        waveText2.text = "0/" + waves.Count;
        tempsText.text = "START";
    }

    public void StartNewWave(){
        if(currentWave < waves.Count-1){
            startWaveEvent.Invoke();
            tempsText.gameObject.SetActive(false);
            /*if(currentWave == waves.Count-1){
                LevelManager.Instance.lastWaveSpawned = true;
            }*/
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

    IEnumerator InstanceWave(Wave wave){
        int currentGroup = -1;
        while(currentGroup < wave.groups.Count-1){
            currentGroup++;
            yield return StartCoroutine(InstanceGroup(wave.groups[currentGroup]));
            yield return new WaitForSeconds(TimeBetweenGroups);
        }

        wavesCoroutines.RemoveAt(0);
        if(currentWave == waves.Count-1 && wavesCoroutines.Count == 0){
            LevelManager.Instance.lastWaveSpawned = true;
        }
        currentCoroutine = null;
        //if(currentWave < waves.Count-1 && currentGroup == wave.groups.Count-2){
        if(currentWave < waves.Count-1){
            waveButton.interactable = true;
            fillImgCoroutine = StartCoroutine(FillImageTempsEnrere());
        }
        
        yield return null;
    }

    IEnumerator InstanceGroup(Group group){
        int currentEnemy = -1;
        while(currentEnemy < group.quantity-1){
            currentEnemy++;
            GenerateEnemy(group.prefabEnemy, group.cami);
            yield return new WaitForSeconds(TimeBetweenEnemies);
        }
    }

    void GenerateEnemy(GameObject enemyPrefab, int cami){
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

        float duration =  waves[currentWave].TimeBetweenNextWave;
        while(tempsTranscorregut < duration){
            tempsTranscorregut += Time.deltaTime;
            imgFillTempsEnrere.fillAmount = Mathf.Clamp01(tempsTranscorregut / duration);
            tempsText.text = (int)(duration-tempsTranscorregut) + "s";

            yield return null;
        }

        imgFillTempsEnrere.fillAmount = 1f;
        StartNewWave();
    }
}
