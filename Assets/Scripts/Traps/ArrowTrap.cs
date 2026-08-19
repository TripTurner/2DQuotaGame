using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public float dir = 1;
    [SerializeField] private float rayDist=8;
    public LayerMask hitLayers;

    public GameObject arrow;
    [SerializeField] private float arrowSpeed;
    private bool armed = true;

    RaycastHit2D hit;

    private TileDestroyer world;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        world = GameObject.FindWithTag("World").GetComponent<TileDestroyer>();
        if (world.cellTypeAt(new Vector2(transform.position.x + 1, transform.position.y),"any")) dir *=-1;
        hit = Physics2D.Raycast(new Vector2(transform.position.x+0.6f*dir,transform.position.y),new Vector2(dir,0),rayDist,hitLayers);
    }

    // Update is called once per frame
    void Update()
    {
        if (armed) {
            RaycastHit2D newHit = Physics2D.Raycast(new Vector2(transform.position.x+0.6f*dir,transform.position.y),new Vector2(dir,0),rayDist,hitLayers);
            if (newHit.collider!=hit.collider || newHit.point != hit.point) {
                GameObject spawnedArrow = Instantiate(arrow, transform.position + new Vector3(0.6f*dir,0,0), Quaternion.identity);
                spawnedArrow.GetComponent<Rigidbody2D>().linearVelocityX = arrowSpeed*dir;
                armed = false;
            }
        }
    }

    void OnDrawGizmos() {
        /*Gizmos.color = Color.green;
        Vector2 start = new Vector2(transform.position.x+0.6f*dir,transform.position.y);
        Gizmos.DrawLine(start, start + new Vector2(dir,0)*rayDist);*/
    }
}
