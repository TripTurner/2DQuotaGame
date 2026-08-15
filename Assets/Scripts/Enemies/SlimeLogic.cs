using UnityEngine;

public class SlimeLogic : EnemyData
{
    [SerializeField] private float speed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float climbSpeed;
    private float dir = 1;
    [SerializeField] Vector2 boxScale;
    [SerializeField] Vector2 boxOffset;
    [SerializeField] Vector2 wallBoxScale;
    [SerializeField] Vector2 wallBoxOffset;
    [SerializeField] private float dangerTileTime = 5;
    public LayerMask groundLayer;
    [SerializeField] private float floorCheckFreq;
    private float timer;

    private string state = "idle";
    [SerializeField] private float sightDist;
    [SerializeField] private float stunTime = 3;
    private float stateTimer;
    private bool seesPlayer;

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

        timer += Time.deltaTime;
        stateTimer += Time.deltaTime;
        if (timer>=floorCheckFreq) {
            timer = 0;
            RaycastHit2D hit = Physics2D.BoxCast((Vector2) transform.position + boxOffset, boxScale, 0, Vector2.zero, Mathf.Infinity, groundLayer);
            if (hit.collider != null) {
                // Debug.Log("hit floor lol");
                world.switchTile(new Vector2(transform.position.x,transform.position.y+boxOffset.y),"danger",true,dangerTileTime);
            }
        }

        if (!seesPlayer && (player.transform.position-transform.position).sqrMagnitude<=Mathf.Pow(sightDist,2)) {
            seesPlayer = true;
            state = "chase";
        } else if (seesPlayer && (player.transform.position-transform.position).sqrMagnitude>Mathf.Pow(sightDist,2)) {
            seesPlayer = false;
            state = "idleStun";
            stateTimer = 0;
        }

        if (state=="idle") {
            rb.linearVelocityX = speed * dir;
            RaycastHit2D hitWall = Physics2D.BoxCast(new Vector2(transform.position.x + wallBoxOffset.x*dir, transform.position.y), wallBoxScale, 0, Vector2.zero, Mathf.Infinity, groundLayer);
            if (hitWall.collider != null) {
                dir *= -1;
            }
        } else if (state=="idleStun") {
            rb.linearVelocityX = 0;
            if (stateTimer>=stunTime) state="idle";
        } else if (state=="chase") {
            if (player.transform.position.x < transform.position.x) {
                dir = -1;
            } else {
                dir = 1;
            }
            rb.linearVelocityX = chaseSpeed * dir;
            RaycastHit2D hitWall = Physics2D.BoxCast(new Vector2(transform.position.x + wallBoxOffset.x*dir, transform.position.y), wallBoxScale, 0, Vector2.zero, Mathf.Infinity, groundLayer);
            if (hitWall.collider != null) {
                rb.linearVelocityY = climbSpeed;
                world.switchTile(new Vector2(transform.position.x + wallBoxOffset.x*dir,transform.position.y),"danger",true,dangerTileTime);
            }
        }
    }

    void FixedUpdate() {

    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube((Vector2)transform.position+boxOffset, boxScale);
        Gizmos.DrawWireCube(new Vector2(transform.position.x + wallBoxOffset.x*dir, transform.position.y), wallBoxScale);
    }
}
