using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private float health;
    [SerializeField] private float maxHealth = 100;
    private PlayerMovement pMovement;
    public float stunPercent;
    private float IFrames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        pMovement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IFrames>0) {
            IFrames-=Time.deltaTime;
        }
    }

    public void takeDamage(float dmg) {
        if (IFrames>0) return;
        health-=dmg;
        IFrames = 0.5f;
        float stun = dmg*stunPercent;
        if (stun>1) {
            pMovement.damagePlayer(stun);
        } else {
            pMovement.damagePlayer();
        }
    }

    public void takeDamage(float dmg, Vector2 knockback) {
        if (IFrames>0) return;
        //Debug.Log("Damaged");
        health-=dmg;
        IFrames = 0.5f;
        float stun = dmg*stunPercent;
        if (stun>1) {
            pMovement.damagePlayer(knockback, stun);
        } else {
            pMovement.damagePlayer(knockback);
        }
    }

    public void childDead(EnemyData child) {
        
    }
}
