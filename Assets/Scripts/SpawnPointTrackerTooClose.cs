using UnityEngine;
using System.Collections.Generic;

public class SpawnPointTrackerTooClose : MonoBehaviour
{
    public LayerMask spawnPointLayer;
    private List<GameObject> spawnPoints = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((spawnPointLayer & (1<<other.gameObject.layer)) != 0) {
            if (other.gameObject.CompareTag("SpawnDataHolder")) spawnPoints.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (spawnPoints.Contains(other.gameObject)) {
            spawnPoints.Remove(other.gameObject);
        }
    }

    public List<GameObject> getSpawnPoints() {
        return spawnPoints;
    }
 }
