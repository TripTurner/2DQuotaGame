using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySpawnData> enemySpawnData = new List<EnemySpawnData>();
    private List<string> enemiesOnMap = new List<string>();
    [SerializeField] private int baseBudget;
    // private int budgetOnMap;
    [SerializeField] private int spawnTimer;
    private float timer;
    private float maxProbability;
    private GameObject player;

    public SpawnPointTracker tracker;

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
        if (!tracker.canSpawn()) return;
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

        int budgetOnMap = tracker.getBudget();
        if (budgetOnMap>=baseBudget) return;     
        // if (budgetOnMap + toSpawn.budgetCost > baseBudget) return; //check to see if room in the budget, this function should continue calling itself until there is no room in the budget
        
        if (tracker.amountOfEnemies(toSpawn.name)>=toSpawn.maxAllowed) {
            spawnEnemies();
            return;
        }

        // budgetOnMap += toSpawn.budgetCost;

        // Instantiate(toSpawn.prefab, player.transform.position, Quaternion.identity);
        tracker.spawnEnemy(toSpawn);
        spawnEnemies();
    }
}
