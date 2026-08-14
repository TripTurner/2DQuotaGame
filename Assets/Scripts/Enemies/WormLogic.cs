using UnityEngine;
using System.Collections.Generic;

public class WormLogic : EnemyData
{
    public GameObject head;
    public GameObject segmentPrefab;
    private List<GameObject> segments;
    public float wormSpeed;
    public int wormLength;
    public float dir = 1;
    public float segmentLength;
    public float sineHeight;
    public float sinePeriod;

    public float boxCastDist;
    public Vector2 pointCastOffset1 = new Vector2(.75f, .375f);
    public Vector2 pointCastOffset2 = new Vector2(.75f, -.375f);
    public Vector2 pointCastOffset3 = new Vector2(.75f, 0);
    public LayerMask destructLayer;

    public TileDestroyer world;
    private int frames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        segments = new List<GameObject>(new GameObject[wormLength]);
        initialize();
        pointCastOffset1 = new Vector2(pointCastOffset1.x*dir, pointCastOffset1.y);
        pointCastOffset2 = new Vector2(pointCastOffset2.x*dir, pointCastOffset2.y);
        pointCastOffset3 = new Vector2(pointCastOffset3.x*dir, pointCastOffset3.y);

        world = GameObject.FindWithTag("World").GetComponent<TileDestroyer>();
        head.GetComponent<EnemyData>().setDamage(damage);
        head.GetComponent<EnemyData>().setKnockback(knockback);
        head.GetComponent<EnemyData>().setHealth(wormLength*5);
        for(int i=0; i<wormLength; i++) {
            GameObject localSegment = Instantiate(segmentPrefab, this.transform);
            localSegment.transform.position = new Vector2(transform.position.x-(segmentLength*(i+1)*dir+.25f), transform.position.y);
            segments[i] = localSegment;
            localSegment.GetComponent<EnemyData>().setDamage(damage);
            localSegment.GetComponent<EnemyData>().setKnockback(knockback);
        }
    }

    public override void initialize() {
        knockback = new Vector2(dir*knockback.x,knockback.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x+wormSpeed*Time.deltaTime*dir, transform.position.y);
        head.transform.position = new Vector2(transform.position.x,transform.position.y + sineHeight * Mathf.Sin(transform.position.x/sinePeriod));
        
        for (int i=0; i<segments.Count; i++) {
            float localX = transform.position.x -(segmentLength*(i+1)*dir);
            segments[i].transform.position = new Vector2(localX,transform.position.y + sineHeight * Mathf.Sin(localX/sinePeriod));
        }

        //frames++;
        //if (frames==10) {
            //frames = 0;
            // checkHit(pointCastOffset1);
            // checkHit(pointCastOffset2);
            // checkHit(pointCastOffset3);
            removeTile(pointCastOffset1);
            removeTile(pointCastOffset2);
            removeTile(pointCastOffset3);
        //}
    }

    public override void childDead(EnemyData child) {
        if (child.gameObject == head) {
            Destroy(gameObject);
        } else {
            for (int i=segments.Count-1; i>=0; i--) {
                if (segments[i]==child.gameObject) {
                    for (int j=segments.Count-1; j>=i; j--) {
                        Destroy(segments[j]);
                        segments.RemoveAt(j);
                    }
                    break;
                }
            }
            float newHeadHealth = segments.Count * 5;
            if (head.GetComponent<EnemyData>().getHealth() > newHeadHealth) {
                head.GetComponent<EnemyData>().setHealth(newHeadHealth);
            }
        }
    }

    public void checkHit(Vector2 pointCastOffset) {
        Collider2D hit = Physics2D.OverlapCircle((Vector2)head.transform.position + pointCastOffset,.2f, destructLayer);
        if (hit!=null) {
            // Debug.Log("Hit with hit1");
            hit.gameObject.GetComponent<TileDestroyer>()?.destroyTile(head.transform.position + (Vector3)pointCastOffset);
        }
    }

    public void removeTile(Vector2 pointCastOffset) {
        //Debug.Log(knockback);
        world.destroyTile(head.transform.position + (Vector3)pointCastOffset);
    }

    // void OnDrawGizmos() {
    //     Gizmos.DrawSphere(head.transform.position + (Vector3)pointCastOffset1,.2f);
    //     Gizmos.DrawSphere(head.transform.position + (Vector3)pointCastOffset2,.2f);
    // }
}
