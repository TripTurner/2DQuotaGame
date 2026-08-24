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
            spawnPoints.Add(other.gameObject);
        }
        if ((enemyLayer & (1<<other.gameObject.layer)) != 0) {
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (spawnPoints.Contains(other.gameObject)) {
            spawnPoints.Remove(other.gameObject);
        }
        if (enemies.Contains(other.gameObject)) {
            enemies.Remove(other.gameObject);
        }
    }
}
