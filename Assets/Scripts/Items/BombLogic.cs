using UnityEngine;
using System.Collections.Generic;

public class BombLogic : ItemLogic
{
    private float explodeTimer;
    [SerializeField] private float maxExplodeTimer;
    [SerializeField] private float explodeForce;
    public LayerMask destructLayer;
    public LayerMask hitLayer;
    public float explodeRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        explodeTimer = maxExplodeTimer;
    }

    protected override void Update() {
        base.Update();
        if (!held) {
            explodeTimer-=Time.deltaTime;
            if (explodeTimer<=0) {
                explode();
            }
        }
    }

    public void explode() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explodeRadius, destructLayer);
        int range = Mathf.CeilToInt(explodeRadius);
        List<Vector3Int> breakPositions = new List<Vector3Int>();
        Vector3Int bombPos = Vector3Int.RoundToInt(transform.position);
        for (int i=-range; i<=range; i++) {
            for (int j=-range; j<=range; j++) {
                Vector3Int testPos = new Vector3Int(bombPos.x + i, bombPos.y + j, bombPos.z);
                if ((testPos-bombPos).sqrMagnitude <= explodeRadius*explodeRadius) {
                    breakPositions.Add(testPos);
                }
            }
        }
        foreach (Collider2D c in hits) {
            foreach (Vector3Int v in breakPositions) {
                c.GetComponent<TileDestroyer>()?.destroyTile(v);
            }
        }
        hits = Physics2D.OverlapCircleAll(transform.position, explodeRadius, hitLayer);
        foreach (Collider2D c in hits) {
            if (c.gameObject.CompareTag("Player")) {
                c.gameObject.GetComponent<PlayerHealth>().takeDamage(20, (c.gameObject.transform.position - transform.position).normalized * explodeForce);
            }/* else {
                Rigidbody2D otherRB = c.gameObject.GetComponent<Rigidbody2D>();
                if (otherRB!=null) otherRB.linearVelocity = (c.gameObject.transform.position - transform.position).normalized * explodeForce;
            }*/
        }
        Destroy(gameObject);
    }

    public void setExplodeTimer(float time) {
        explodeTimer = time;
        maxExplodeTimer = time;
    }
}
