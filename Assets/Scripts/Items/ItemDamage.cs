using UnityEngine;

public class ItemDamage : MonoBehaviour
{
    [SerializeField] private bool dealsDamage = true;
    private Rigidbody2D rb;
    [SerializeField] private float damage = 10;
    [SerializeField] private float damageSpeed = 3;
    public LayerMask damageLayer;
    private GameObject owner;
    [SerializeField] private float maxOwnershipTime;
    private float ownershipTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ownershipTime>0) {
            ownershipTime-=Time.deltaTime;
        } else if (owner!=null) {
            owner = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!dealsDamage || other.gameObject == owner) return;
        if ((damageLayer & (1<<other.gameObject.layer)) != 0) {
            if (rb.linearVelocity.sqrMagnitude >= damageSpeed*damageSpeed) {
                other.GetComponent<IDamageable>()?.takeDamage(damage, new Vector2(rb.linearVelocityX,0).normalized);
            }
        }
    }

    public void setOwner(GameObject other) {
        owner = other;
        ownershipTime = maxOwnershipTime;
    }
}
