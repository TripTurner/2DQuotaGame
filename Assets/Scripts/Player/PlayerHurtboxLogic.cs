using UnityEngine;

public class PlayerHurtboxLogic : EnemyData
{
    private PlayerHealth pHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pHealth = GetComponentInParent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void takeDamage(float damage) {
        pHealth.takeDamage(damage);
    }

    public override void takeDamage(float damage, Vector2 knockback) {
        pHealth.takeDamage(damage, knockback);
    }
}
