using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySpawnData> enemySpawnData = new List<EnemySpawnData>();
    private List<string> enemiesOnMap = new List<string>();
    [SerializeField] private int baseBudget;
    private int budgetOnMap;
    [SerializeField] private int spawnTimer;
    private float timer;
    private float maxProbability;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = spawnTimer;
        for (int i=0; i<enemySpawnData.Count; i++) {
            maxProbability += enemySpawnData[i].weight;
        }
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer<=0) {
            timer = spawnTimer;
            spawnEnemies();
        }
    }

    public void spawnEnemies() {
        float spawnProb = Random.Range(0,maxProbability);
        EnemySpawnData toSpawn = null;
        for (int i=0; i<enemySpawnData.Count; i++) {
            spawnProb -= enemySpawnData[i].weight;
            if (spawnProb<=0) {
                toSpawn = enemySpawnData[i];
                break;
            }
        }
        if (toSpawn==null) {
            Debug.Log("Couldn't find enemy for some reason");
            return;
        }
        if (budgetOnMap + toSpawn.budgetCost > baseBudget) return; //check to see if room in the budget, this function should continue calling itself until there is no room in the budget
        
        int maxToSpawn = toSpawn.maxAllowed; //check to see if there are the max amount of enemy chosen
        for (int i=0; i<enemiesOnMap.Count; i++) {
            if (enemiesOnMap[i] == toSpawn.name) maxToSpawn--;
        }
        if (maxToSpawn<=0) {
            spawnEnemies();
            return;
        }

        budgetOnMap += toSpawn.budgetCost;

        Instantiate(toSpawn.prefab, player.transform.position, Quaternion.identity);
        spawnEnemies();
    }
}
