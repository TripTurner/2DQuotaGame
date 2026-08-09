using UnityEngine;
using System.Collections.Generic;

public class WormLogic : MonoBehaviour
{
    public GameObject head;
    public GameObject segmentPrefab;
    private GameObject[] segments;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        segments = new GameObject[wormLength];
        for(int i=0; i<wormLength; i++) {
            GameObject localSegment = Instantiate(segmentPrefab, this.transform);
            localSegment.transform.position = new Vector2(transform.position.x-(segmentLength*(i+1)*dir+.25f), transform.position.y);
            segments[i] = localSegment;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x+wormSpeed*Time.deltaTime*dir, transform.position.y);
        head.transform.position = new Vector2(transform.position.x,transform.position.y + sineHeight * Mathf.Sin(transform.position.x/sinePeriod));
        
        for (int i=0; i<wormLength; i++) {
            float localX = transform.position.x -(segmentLength*(i+1)*dir);
            segments[i].transform.position = new Vector2(localX,transform.position.y + sineHeight * Mathf.Sin(localX/sinePeriod));
        }

        Collider2D hit1 = Physics2D.OverlapCircle((Vector2)head.transform.position + pointCastOffset1,.2f, destructLayer);
        if (hit1!=null) {
            // Debug.Log("Hit with hit1");
            hit1.gameObject.GetComponent<TileDestroyer>()?.destroyTile(head.transform.position + (Vector3)pointCastOffset1);
        }

        Collider2D hit2 = Physics2D.OverlapCircle((Vector2)head.transform.position + pointCastOffset2,.2f, destructLayer);
        if (hit2!=null) {
            // Debug.Log("Hit with hit2");
            hit2.gameObject.GetComponent<TileDestroyer>()?.destroyTile(head.transform.position + (Vector3)pointCastOffset2);
        }
        
        Collider2D hit3 = Physics2D.OverlapCircle((Vector2)head.transform.position + pointCastOffset3,.2f, destructLayer);
        if (hit3!=null) {
            // Debug.Log("Hit with hit3");
            hit3.gameObject.GetComponent<TileDestroyer>()?.destroyTile(head.transform.position + (Vector3)pointCastOffset3);
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawSphere(head.transform.position + (Vector3)pointCastOffset1,.2f);
        Gizmos.DrawSphere(head.transform.position + (Vector3)pointCastOffset2,.2f);
    }
}
