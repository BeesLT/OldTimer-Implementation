using UnityEngine;

public class Bosses : MonoBehaviour
{
    [Header("Stats")]
    public float attackDamage = 10f;
    public float attackCooldown = 2f;

    [Header("Movement")]
    public float speed = 1.0f;
    public float maxHeight = 5.0f;
    public float weavingDistance = 1.5f;
    public float fallbackDistance = 20.0f;
    public float wobbleAmount = 2f;

    [Header("Maneuvers")]
    public ManeuverType[] maneuverSequence;
    public bool loopManeuvers = true;

    private Health _health;
    private float _lastAttackTime = -999f;
    private int _currentManeuverIndex = 0;
    private IManeuverBehaviour _activeManeuver;
    private ManeuverFactory _factory;

    public bool IsAlive => _health != null && _health.IsAlive;
    public float HealthPercent => _health != null ? _health.HealthPercent : 0f;

    void Awake()
    {
        _factory = new ManeuverFactory();
        _health = GetComponent<Health>();

        if (_health == null)
        {
            Debug.LogError("[Boss] No Health component found!");
            return;
        }
        _health.hasIFrames = false;

        _health.onDeath.AddListener(OnBossDeath);
        _health.onHealthChanged.AddListener(OnHealthChanged);
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
        _currentManeuverIndex = 0;
        _activeManeuver = null;

        _health.currentHealth = _health.maxHealth;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log($"[Boss] Spawned with {_health.maxHealth} HP!");
        RunNextManeuver();
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
        Debug.Log("[Boss] Despawned!");
    }

    void Update()
    {
        if (!IsAlive) return;

        if (transform.position.y > maxHeight)
            ObjectSpawner.Instance.ReleaseBoss(this);
    }


    void OnHealthChanged(float healthPercent)
    {
        if (healthPercent <= 0.5f && _currentManeuverIndex == 1)
        {
            Debug.Log("[Boss] Phase 2 triggered!");
            RunNextManeuver();
        }
    }

    void OnBossDeath()
    {
        Debug.Log("[Boss] Defeated!");
        ObjectSpawner.Instance.ReleaseBoss(this);
    }

    public bool CanAttack()
    {
        return Time.time - _lastAttackTime >= attackCooldown;
    }

    public void PerformAttack(GameObject target)
    {
        if (!CanAttack()) return;

        _lastAttackTime = Time.time;

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(attackDamage);
            Debug.Log($"[Boss] Attacked {target.name} for {attackDamage} damage!");
        }
    }

    public void RunNextManeuver()
    {
        if (maneuverSequence == null || maneuverSequence.Length == 0) return;

        if (_currentManeuverIndex >= maneuverSequence.Length)
        {
            if (!loopManeuvers) return;
            _currentManeuverIndex = 0;
        }

        ManeuverType next = maneuverSequence[_currentManeuverIndex];
        _activeManeuver = _factory.Create(next);
        _currentManeuverIndex++;

        _activeManeuver?.Maneuver(this);
        Debug.Log($"[Boss] Running maneuver: {next}");
    }

    public void ApplyManeuver(IManeuverBehaviour maneuver)
    {
        _activeManeuver = maneuver;
        _activeManeuver.Maneuver(this);
    }
}