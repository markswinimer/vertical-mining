using UnityEngine;

public interface IDamageable
{
    public bool Invincible { get; set; }
    public void TakeDamage(int damage);
    public void TakeDamageWithForce(int damage, Vector2 force);
}
