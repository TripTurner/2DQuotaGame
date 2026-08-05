using UnityEngine;

public interface IDamageable
{
    void takeDamage(float dmg);
    void takeDamage(float dmg, Vector2 knockback);

}
