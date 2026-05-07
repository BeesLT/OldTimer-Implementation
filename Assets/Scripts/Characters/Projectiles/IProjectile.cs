using UnityEngine;

public interface IProjectile
{
    float Damage { get; }
    float Speed { get; }
    float Lifetime { get; }

    void OnSpawn();
    void OnDespawn();
    void Launch(Vector3 direction, string ownerTag);
}