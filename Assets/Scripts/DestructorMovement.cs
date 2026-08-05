using UnityEngine;
using UnityEngine.InputSystem;

public class DestructorMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Vector2 boxCastScale = new Vector2(1,1);
    [SerializeField] private float dist;
    private Vector2 velocity = Vector2.zero;
    [SerializeField] private LayerMask destructLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame) {
            float multiplier = velocity.x==0 ? 0 : Mathf.Sign(velocity.x);
            RaycastHit2D hit = Physics2D.BoxCast((Vector2) transform.position + new Vector2(multiplier*dist,0), boxCastScale, 0, Vector2.zero, Mathf.Infinity, destructLayer);
            if (hit.collider!=null) {
                hit.collider.gameObject.GetComponent<TileDestroyer>().destroyTile(transform.position+ new Vector3(velocity.x/10,0,0));
            }
        }
    }

    void FixedUpdate() {
        if (Keyboard.current.upArrowKey.isPressed) {
            velocity.y=10;
        } else if (Keyboard.current.downArrowKey.isPressed) {
            velocity.y=-10;
        } else {
            velocity.y=0;
        }
        if (Keyboard.current.leftArrowKey.isPressed) {
            velocity.x = -10;
        } else if (Keyboard.current.rightArrowKey.isPressed) {
            velocity.x = 10;
        } else {
            velocity.x = 0;
        }

        velocity = velocity.normalized * 10;

        rb.linearVelocity = velocity;
    }

    void OnDrawGizmos() {
        float multiplier = velocity.x==0 ? 0 : Mathf.Sign(velocity.x);
        Gizmos.DrawWireCube(transform.position+ new Vector3(multiplier*dist,0,0), boxCastScale);
    }
}
