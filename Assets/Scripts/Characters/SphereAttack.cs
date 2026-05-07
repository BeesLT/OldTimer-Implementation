using UnityEngine;

public class SphereAttack : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 15f;
    public float lifetime = 4f;
    public float damage = 25f;

    private Rigidbody _rb;
    private float _spawnTime;
    private string _ownerTag;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
        _spawnTime = Time.time;

        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("[SphereAttack] Spawned!");
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
        Debug.Log("[SphereAttack] Despawned!");
    }

    public void Launch(Vector3 direction, string ownerTag = "Player")
    {
        _ownerTag = ownerTag;

        if (_rb != null)
            _rb.linearVelocity = direction.normalized * speed;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Update()
    {
        if (Time.time - _spawnTime >= lifetime)
            ObjectSpawner.Instance.ReleaseSphere(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(_ownerTag) && other.CompareTag(_ownerTag)) return;

        Debug.Log($"[SphereAttack] Hit: {other.name} for {damage} damage!");
        other.GetComponent<Health>()?.TakeDamage(damage);

        ProjectileEffectsManager.Instance?.PlayImpact(transform.position);
        ObjectSpawner.Instance.ReleaseSphere(this);
    }
}