using UnityEngine;

public class EnemyData : MonoBehaviour, IDamageable
{
    [SerializeField] protected float damage;
    [SerializeField] protected bool dealingDamage = true;
    [SerializeField] protected Vector2 knockback = new Vector2(0,1);
    [SerializeField] protected float knockbackFloat = 10;
    [SerializeField] protected float health;
    protected IDamageable parent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void initialize() {
        parent = transform.parent.gameObject.GetComponent<IDamageable>();
        Debug.Log(parent);
        //knockback = new Vector2(0,1);
    }

    public virtual bool isDealingDamage() {
        return dealingDamage;
    }

    public virtual float getDamage() {
        return damage;
    }

    public virtual Vector2 getKnockback() {
        return knockback;
    }

    public virtual void setDealingDamage(bool isIt) {
        dealingDamage = isIt;
    }

    public virtual void setDamage(float newDamage) {
        damage = newDamage;
    }

    public virtual void setKnockback(Vector2 newKnockback) {
        knockback = newKnockback;
    }

    public virtual void setHealth(float hp) {
        health = hp;
    }

    public virtual float getHealth() {
        return health;
    }

    public virtual void takeDamage(float damage) {
        takeDamage(damage, new Vector2(0,0));
    }

    public virtual void childDead(EnemyData child) {
        Destroy(gameObject);
    }

    public virtual void takeDamage(float damage, Vector2 knockback) {
        health -= damage;
        if (health<=0) {
            parent.childDead(this);
        }
    }
}
