using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected bool dealingDamage = true;
    [SerializeField] protected Vector2 knockback = new Vector2(0,1);
    [SerializeField] protected float health;
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
}
