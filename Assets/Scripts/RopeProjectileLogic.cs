using UnityEngine;

public class RopeProjectileLogic : MonoBehaviour
{
    private float ttl = 1.5f;
    [SerializeField] private float startSpeed = 5;
    private Rigidbody2D rb;
    private float speedMod;
    private float maxHeight;
    private float height;
    public GameObject rope;
    public LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = startSpeed;
        speedMod = startSpeed/ttl;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.up,maxHeight-transform.position.y,groundLayer);
        if (hit.collider!=null) {
            height = hit.point.y - 0.2f;
        } else {
            height = maxHeight - 0.1f;
        }
    }

    public void Initialize(float h) {
        maxHeight = h;
    }

    // Update is called once per frame
    void Update()
    {
        startSpeed-=Time.deltaTime*speedMod;
        if (transform.position.y > height) {
            createRope();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        createRope();
    }

    private void createRope() {
        Instantiate(rope,new Vector3(Mathf.Floor(transform.position.x)+0.5f,Mathf.Ceil(height),0),Quaternion.identity);
        Destroy(gameObject);
    }

    // private void OnDrawGizmos() {
    //     if (height!=0) {
    //         Gizmos.color = Color.green;
    //         Gizmos.DrawSphere(new Vector3(transform.position.x,height,0),.2f);
    //     }
    // }
}
