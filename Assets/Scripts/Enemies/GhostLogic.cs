using UnityEngine;
using System.Collections.Generic;

public class GhostLogic : EnemyData
{
    [SerializeField] private float speed;
    [SerializeField] private float terrainDistance;
    [SerializeField] private float destroyFreq;
    [SerializeField] private float destroyTime;
    public LayerMask destructLayer;
    private float timer;
    
    private Rigidbody2D rb;
    private GameObject player;
    public TileDestroyer world;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        world = GameObject.FindWithTag("World").GetComponent<TileDestroyer>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = (Vector2)(player.transform.position - transform.position).normalized * speed;

        timer += Time.deltaTime;
        if (timer>=destroyFreq) {
            timer = 0;
            int range = Mathf.CeilToInt(terrainDistance);
            List<Vector3> breakPositions = new List<Vector3>();
            Vector3 pos = transform.position;
            for (int i=-range; i<=range; i++) {
                for (int j=-range; j<=range; j++) {
                    Vector3 testPos = new Vector3(pos.x + i, pos.y + j, pos.z);
                    // if ((testPos-pos).sqrMagnitude <= terrainDistance*terrainDistance) {
                        breakPositions.Add(testPos);
                    // }
                }
            }
            foreach (Vector3 v in breakPositions) {
                world.switchTile(v,"destroy",true,destroyTime);
            }
        }

    }
}
