using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Fire Points")]
    public Transform firePoint;

    [Header("Laser")]
    public KeyCode laserKey = KeyCode.K;
    public float laserCooldown = 0.3f;

    [Header("Sphere")]
    public KeyCode sphereKey = KeyCode.L;
    public float sphereCooldown = 0.8f;

    private float _lastLaserTime = -999f;
    private float _lastSphereTime = -999f;

    void Update()
    {
        if (Input.GetKeyDown(laserKey) && CanFire(ref _lastLaserTime, laserCooldown))
            ShootLaser();

        if (Input.GetKeyDown(sphereKey) && CanFire(ref _lastSphereTime, sphereCooldown))
            ShootSphere();
    }

    bool CanFire(ref float lastFireTime, float cooldown)
    {
        if (Time.time - lastFireTime >= cooldown)
        {
            lastFireTime = Time.time;
            return true;
        }
        return false;
    }

    void ShootLaser()
    {
        Vector3 origin = GetOrigin();
        Vector3 direction = GetDirection();

        Laser laser = ObjectSpawner.Instance.GetLaser();
        laser.transform.position = origin;
        laser.transform.rotation = Quaternion.LookRotation(direction);
        laser.OnSpawn();
        laser.Launch(direction);

        Debug.Log("[Player] Fired laser!");
    }

    void ShootSphere()
    {
        Vector3 origin = GetOrigin();
        Vector3 direction = GetDirection();

        SphereAttack sphere = ObjectSpawner.Instance.GetSphere();
        sphere.transform.position = origin;
        sphere.OnSpawn();
        sphere.Launch(direction, "Player");

        Debug.Log("[Player] Fired sphere!");
    }

    Vector3 GetOrigin() =>
        firePoint != null ? firePoint.position : transform.position + Vector3.up;

    Vector3 GetDirection() =>
        firePoint != null ? firePoint.forward : transform.forward;
}