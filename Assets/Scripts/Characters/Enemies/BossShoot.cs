using UnityEngine;

public class BossShoot : MonoBehaviour
{
    [Header("Sphere Attack")]
    public float shootCooldown = 3f;
    public Transform firePoint;
    public float detectionRange = 15f;

    private float _lastShootTime = -999f;
    private Bosses _boss;

    void Awake()
    {
        _boss = GetComponent<Bosses>();
    }

    void Update()
    {
        if (_boss == null || !_boss.IsAlive) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distToPlayer <= detectionRange && CanShoot())
            ShootSphere(player);
    }

    bool CanShoot()
    {
        if (Time.time - _lastShootTime >= shootCooldown)
        {
            _lastShootTime = Time.time;
            return true;
        }
        return false;
    }

    void ShootSphere(GameObject player)
    {
        Vector3 origin = firePoint != null
            ? firePoint.position
            : transform.position;

        Vector3 direction = (player.transform.position - origin).normalized;

        SphereAttack sphere = ObjectSpawner.Instance.GetSphere();
        sphere.transform.position = origin;
        sphere.OnSpawn();
        sphere.Launch(direction, "Enemy");

        Debug.Log($"[Boss] Fired sphere at {player.name}!");
    }
}