using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New level Object", menuName = "levelObj")]
public class WaveObject : ScriptableObject
{
    [Serializable]
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
    }

    public int initialCoins;
    public int minValueDiff, maxValueDiff;
    public AnimationCurve diffCurve;
    public List<Onades> waves;
}
