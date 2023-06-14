using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    // [SerializeField] UpgradeC upgradeC;

    [System.Serializable]
    public struct EnemyType
    {
        public GameObject prefab;
        public float value; 
    }
    
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    public EnemyType[] enemyTypes; 
    [SerializeField] float waveValue;
    [SerializeField] float valueIncreasePerWave; 
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float spawnSpeedReductionTime;
    [SerializeField] float minSpawnSpeed;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text enemiesLeftText;
    private float currentWaveValue;
    private float spawnTimer;
    public int waveNumber = 1;
    bool isNewWave;
    [SerializeField] float spawnPointRequiredDistance;
    [SerializeField] int wavesToLevelUp;
    int lastLeveledUpWave = 1;
    public int enemiesSpawned;
    public int killedEnemies;
    int enemiesLeft;
    bool spawnNextWave;
    [SerializeField] int bossSpawnWave;

    // Boss
    public bool inBossFight;


    void Start()
    {
        waveText.text = "Wave: " + waveNumber;
        currentWaveValue = waveValue;
    }

    void FixedUpdate()
    {
        if (GameManager.gm.isPlayerDead)
        {
            return;
        }

        if (inBossFight)
        {
            return;
        }

        if (waveNumber - lastLeveledUpWave >= wavesToLevelUp)
        {   
            // if (upgradeC.powerUps.Count > 2)
            // {
            //     StartCoroutine(upgradeC.LevelUp());
            // }

            lastLeveledUpWave = waveNumber;
        }
        
        if (spawnNextWave)
        {
            enemiesLeftText.text = "Spawning...";
        } else
        {
            enemiesLeft = enemiesSpawned - killedEnemies;
            enemiesLeftText.text = "Enemies Left: " + enemiesLeft.ToString();

            if (enemiesLeft == 0)
            {
                spawnNextWave = true;
            } 
        }

        if (spawnTimer <= 0f && spawnNextWave)
        {
            if (isNewWave)
            {
                waveNumber++;
                ChangeWaveNumber(waveNumber.ToString());


                if (waveNumber == bossSpawnWave)
                {
                    inBossFight = true;
                    return;
                }

                isNewWave = false;

                if (timeBetweenSpawns > minSpawnSpeed)
                {
                    timeBetweenSpawns -= spawnSpeedReductionTime;
                }

            }

            int enemyIndex = Random.Range(0, enemyTypes.Length);

            while (enemyTypes[enemyIndex].value > currentWaveValue || !CanSpawnEnemyType(enemyIndex)) 
            {
                enemyIndex = Random.Range(0, enemyTypes.Length);
            }

            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // while (Vector3.Distance(randomSpawnPoint.position, GameManager.gm.player.transform.position) < spawnPointRequiredDistance)
            // {
            //     randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].spawnPoint;
            // }

            float randomPosX = Random.Range(-2f, 2f);
            float randomPosZ = Random.Range(-2f, 2f);

            Vector3 spawnLocation = new Vector3(randomSpawnPoint.position.x + randomPosX, randomSpawnPoint.position.y, randomSpawnPoint.position.z + randomPosZ);

            GameObject newEnemy = Instantiate(enemyTypes[enemyIndex].prefab, spawnLocation, randomSpawnPoint.rotation);

            enemiesSpawned++;

            currentWaveValue -= enemyTypes[enemyIndex].value;

            spawnTimer = timeBetweenSpawns;

            if (currentWaveValue <= 0f)
            {
                waveValue += valueIncreasePerWave;

                currentWaveValue = waveValue;
                spawnTimer = 0f;
                spawnNextWave = false;
                isNewWave = true;
            }

        } else
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
            }
        }
    }

    public void ChangeWaveNumber(string waveNumber)
    {
        waveText.text = "Wave: " + waveNumber;
    }

    private bool CanSpawnEnemyType(int index)
    {
        if (waveNumber < 5 && enemyTypes[index].prefab.name == "Stan")
        {
            return false;
        } else 
        {
            return true;
        }
    }
}

