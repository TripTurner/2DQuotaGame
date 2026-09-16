using UnityEngine;
using System.Collections.Generic;

public class SpawnPointTracker : MonoBehaviour
{
    private SpawnPointTrackerTooClose tooClose;
    private GameObject player;
    [SerializeField] private float radius;
    [SerializeField] private float tooCloseRadius;
    public LayerMask spawnPointLayer;
    public LayerMask enemyLayer;

    private List<GameObject> spawnPoints = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tooClose = GetComponentInChildren<SpawnPointTrackerTooClose>();
        player = GameObject.FindWithTag("Player");
        GetComponent<CircleCollider2D>().radius = radius;
        tooClose.GetComponent<CircleCollider2D>().radius = tooCloseRadius;
    } 

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((spawnPointLayer & (1<<other.gameObject.layer)) != 0) {
            if (other.gameObject.CompareTag("SpawnDataHolder")) {
                enemies.Add(other.gameObject);
            } else {
                spawnPoints.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (spawnPoints.Contains(other.gameObject)) {
            spawnPoints.Remove(other.gameObject);
        }
        if (enemies.Contains(other.gameObject)) {
            enemies.Remove(other.gameObject);
            Debug.Log("Removed enemy");
        }
    }

    public int getBudget() {
        int total = 0;
        foreach (GameObject GO in enemies) {
            total+=GO.GetComponentInChildren<SpawnDataHolder>().spawnData.budgetCost;
        }
        return total;
    }

    public int amountOfEnemies(string enemy) {
        int amount = 0;
        foreach (GameObject GO in enemies) {
            if (enemy == GO.GetComponentInChildren<SpawnDataHolder>().spawnData.name) amount++;
        }
        return amount;
    }

    public bool canSpawn() {
        Debug.Log($"Amount of spawnPoints in range: {spawnPoints.Count}");
        // Debug.Log($"Enemy list: {enemies}");
        if (spawnPoints.Count==0) {
            return false;
        } else if (spawnPoints.Count == tooClose.getSpawnPoints().Count) {
            return false;
        }
        return true;
    }

    public void spawnEnemy(EnemySpawnData enemy) {
        if (!canSpawn()) return;
        GameObject spawnPoint = getFarSpawn();
        // spawnPoints.RemoveAt(index);

        GameObject toInstantiate = Instantiate(enemy.prefab, spawnPoint.transform.position, Quaternion.identity);
        enemies.Add(toInstantiate);
    }

    public GameObject getFarSpawn() {
        int index = Random.Range(0,spawnPoints.Count);
        GameObject spawnPoint = spawnPoints[index];
        if (tooClose.getSpawnPoints().Contains(spawnPoint)) {
            return getFarSpawn();
        } else {
            return spawnPoint;
        }
    }
}
