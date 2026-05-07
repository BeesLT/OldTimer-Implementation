using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
public abstract class ProjectileBase : MonoBehaviour, IProjectile
{
    [Header("Projectile Settings")]
    public float damage = 10f;
    public float speed = 20f;
    public float lifetime = 4f;

    [Header("Pulse Settings")]
    public float pulseSpeed = 3f;
    public float pulseMinScale = 0.9f;
    public float pulseMaxScale = 1.1f;

    public float Damage => damage;
    public float Speed => speed;
    public float Lifetime => lifetime;

    protected Rigidbody Rb;
    protected string OwnerTag;
    protected float SpawnTime;

    private TrailRenderer _trail;
    private Renderer _renderer;
    private Vector3 _baseScale;
    private Color _emissionColor;

    protected virtual void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        _trail = GetComponent<TrailRenderer>();
        _renderer = GetComponent<Renderer>();
        _baseScale = transform.localScale;
    }

    public virtual void OnSpawn()
    {
        gameObject.SetActive(true);
        SpawnTime = Time.time;

        if (Rb != null)
        {
            Rb.linearVelocity = Vector3.zero;
            Rb.angularVelocity = Vector3.zero;
        }

        if (_trail != null)
        {
            _trail.Clear();
            _trail.enabled = true;
        }

        transform.localScale = _baseScale;

        if (_renderer != null && _renderer.material.HasProperty("_EmissionColor"))
            _emissionColor = _renderer.material.GetColor("_EmissionColor");
    }

    public virtual void OnDespawn()
    {
        if (_trail != null)
            _trail.enabled = false;

        gameObject.SetActive(false);
    }

    public virtual void Launch(Vector3 direction, string ownerTag)
    {
        OwnerTag = ownerTag;

        if (Rb != null)
            Rb.linearVelocity = direction.normalized * speed;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    protected virtual void Update()
    {
        if (Time.time - SpawnTime >= lifetime)
        {
            ReleaseToPool();
            return;
        }

        ApplyPulse();
    }

    void ApplyPulse()
    {
        float pulse = Mathf.Lerp(
            pulseMinScale,
            pulseMaxScale,
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f
        );

        transform.localScale = _baseScale * pulse;

        if (_renderer != null && _renderer.material.HasProperty("_EmissionColor"))
        {
            float brightness = Mathf.Lerp(0.8f, 1.5f,
                (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            _renderer.material.SetColor("_EmissionColor",
                _emissionColor * brightness);
        }
    }

    protected void SpawnImpactEffect()
    {
        ProjectileEffectsManager.Instance?.PlayImpact(transform.position);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(OwnerTag) && other.CompareTag(OwnerTag)) return;

        Debug.Log($"[{GetType().Name}] Hit: {other.name} for {damage} damage!");

        other.GetComponent<Health>()?.TakeDamage(damage);

        SpawnImpactEffect();
        ReleaseToPool();
    }

    protected abstract void ReleaseToPool();
}