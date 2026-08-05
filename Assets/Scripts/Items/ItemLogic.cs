using UnityEngine;
using UnityEngine.InputSystem;

public class ItemLogic : MonoBehaviour
{
    public int priority = 10;
    protected bool held = false;
    protected bool stored = false;
    protected GameObject holder;
    [SerializeField] protected float dropSpeed = 1f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (held) {
            if (stored) {
                transform.position = new Vector2(0,10);
            } else {
                transform.position = holder.transform.position + new Vector3(0,0,-0.5f);
            }
        }
    }

    public bool getHeld() {
        return held;
    }

    public void interact(GameObject other) {
        transform.GetChild(0).gameObject.SetActive(false);
        held = true;
        stored = false;
        holder = other;
    }

    public void drop() {
        transform.GetChild(0).gameObject.SetActive(true);
        held = false;
        stored = false;
        Vector2 otherVel = holder.GetComponent<Rigidbody2D>().linearVelocity;
        rb.linearVelocity = otherVel + new Vector2(Mathf.Sign(otherVel.x)*dropSpeed,0);
    }

    public void drop(Vector2 addVel) {
        transform.GetChild(0).gameObject.SetActive(true);
        held = false;
        stored = false;
        Vector2 otherVel = holder.GetComponent<Rigidbody2D>().linearVelocity;
        rb.linearVelocity = otherVel + addVel;
    }

    public void store() {
        stored = true;
    }

    public virtual void use() {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.GetComponent<ItemDamage>()?.setOwner(holder);
        held = false;
        if (holder.CompareTag("Player")) {
            holder.GetComponent<PlayerInventory>().removeItem(this);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 dir = mousePos-holder.transform.position;
            // float angle = Mathf.Atan2(dir.x,dir.y);
            rb.linearVelocity = dir.normalized * holder.GetComponent<PlayerMovement>().throwStrength + holder.GetComponent<Rigidbody2D>().linearVelocity;
        }
    } //Right now it only works with the player. 
    // I need to make an interface that the player uses and other enemies can as well
    // to handle removing it from their inventory to stop any quantum holding from happening
}
