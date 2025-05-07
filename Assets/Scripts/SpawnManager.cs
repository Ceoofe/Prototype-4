using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Variables //
    public GameObject[] enemyPrefab;
    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 1;
    public GameObject[] powerupPrefab;
    int enemyIndex;
    int powerIndex;

    // Start is called before the first frame update
    void Start() // Spawns the powerup and enemy
    {
        powerIndex = Random.Range(0, powerupPrefab.Length);
        SpawnEnemyWave(waveNumber);
        Instantiate(powerupPrefab[powerIndex], GenerateSpawnPosition(), powerupPrefab[powerIndex].transform.rotation);
    }

    private Vector3 GenerateSpawnPosition() // Random positions for the instances
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
    // Update is called once per frame
    void Update() // Add a another wave once the previous one is done
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            powerIndex = Random.Range(0, powerupPrefab.Length);
            Instantiate(powerupPrefab[powerIndex], GenerateSpawnPosition(), powerupPrefab[powerIndex].transform.rotation);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn) // Spawn enemies for each waves
    {
        enemyIndex = Random.Range(0, enemyPrefab.Length);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab[enemyIndex], GenerateSpawnPosition(), enemyPrefab[enemyIndex].transform.rotation);
            enemyIndex = Random.Range(0, enemyPrefab.Length);
        }
    }
}
