using UnityEngine;

public class LaserGuyLogic : EnemyData
{
    [SerializeField] private float laserTime = 3;
    [SerializeField] private float flashingTime = 1f;
    [SerializeField] private float reloadTime = 4;
    
    [SerializeField] private float flashFrequency = 0.5f;
    private float timer = 0;
    private string state = "idle";
    private bool seesPlayer = false;
    [SerializeField] private float laserDistance = 7;
    [SerializeField] private Vector2 laserOriginOffset;
    private Vector2 laserOrigin;
    private Vector2 aimDir;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    private GameObject player;
    [SerializeField] private LineRenderer laserLine;
    public TileDestroyer world;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        laserLine.positionCount = 2;
        world = GameObject.FindWithTag("World").GetComponent<TileDestroyer>();
    }

    // Update is called once per frame
    void Update()
    {
        laserOrigin = (Vector2)transform.position + laserOriginOffset;
        if (timer>=0) timer-=Time.deltaTime;

        if (!seesPlayer && ((Vector2)player.transform.position-laserOrigin).sqrMagnitude<=Mathf.Pow(laserDistance,2)) {
            seesPlayer = true;
        }

        if (seesPlayer && ((Vector2)player.transform.position-laserOrigin).sqrMagnitude>Mathf.Pow(laserDistance,2)) {
            seesPlayer = false;
        }

        if (state=="idle") {
            if (seesPlayer) {
                state = "aim";
                laserLine.enabled = true;
                timer = laserTime;
            }
        } else if (state=="aim") {
            if (seesPlayer) {
                aimDir = ((Vector2)player.transform.position - laserOrigin).normalized;
            }
            RaycastHit2D wallCast = Physics2D.Raycast(laserOrigin,aimDir,laserDistance,groundLayer);
            Vector2 lineEnd = laserOrigin + aimDir*laserDistance;
            if (wallCast.collider!=null) {
                lineEnd = wallCast.point;
            }
            laserLine.SetPosition(0, laserOrigin);
            laserLine.SetPosition(1, lineEnd);
            if (timer<=0) {
                state = "flash";
                timer = flashingTime;
            }
        } else if (state=="flash") {
            if (timer%flashFrequency>=(flashFrequency/2)) {
                laserLine.enabled = false;
            } else {
                laserLine.enabled = true;
            }
            if (timer<=0) {
                fireLaser();
                state = "reload";
                timer = reloadTime;
                laserLine.enabled = false;
            }
        } else if (state=="reload") {
            if (timer<=0) {
                state = "idle";
            }
        }
    }

    public void fireLaser() {
        RaycastHit2D playerCast = Physics2D.Raycast(laserOrigin,aimDir,laserDistance,playerLayer);
        if (playerCast.collider != null) {
            player.GetComponent<PlayerHealth>().takeDamage(damage, aimDir.normalized * knockbackFloat);
        }
        world.destroyTilesInLine(laserOrigin,laserOrigin+aimDir*laserDistance);
        Debug.Log("LASER FIRED");
    }
}
