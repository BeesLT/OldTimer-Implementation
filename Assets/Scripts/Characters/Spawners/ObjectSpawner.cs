using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance { get; private set; }

    [Header("Laser Pool")]
    public Laser laserPrefab;
    public int laserPoolSize = 10;

    [Header("Sphere Pool")]
    public SphereAttack spherePrefab;
    public int spherePoolSize = 10;

    private Queue<Laser> _laserPool = new Queue<Laser>();
    private Queue<SphereAttack> _spherePool = new Queue<SphereAttack>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitLaserPool();
        InitSpherePool();
    }

    void InitLaserPool()
    {
        for (int i = 0; i < laserPoolSize; i++)
        {
            Laser laser = Instantiate(laserPrefab);
            laser.gameObject.SetActive(false);
            _laserPool.Enqueue(laser);
        }
    }

    public Laser GetLaser()
    {
        Laser laser = _laserPool.Count > 0
            ? _laserPool.Dequeue()
            : Instantiate(laserPrefab);

        laser.gameObject.SetActive(true);
        return laser;
    }

    public void ReleaseLaser(Laser laser)
    {
        laser.OnDespawn();
        _laserPool.Enqueue(laser);
    }

    void InitSpherePool()
    {
        for (int i = 0; i < spherePoolSize; i++)
        {
            SphereAttack sphere = Instantiate(spherePrefab);
            sphere.gameObject.SetActive(false);
            _spherePool.Enqueue(sphere);
        }
    }

    public SphereAttack GetSphere()
    {
        SphereAttack sphere = _spherePool.Count > 0
            ? _spherePool.Dequeue()
            : Instantiate(spherePrefab);

        sphere.gameObject.SetActive(true);
        return sphere;
    }

    public void ReleaseSphere(SphereAttack sphere)
    {
        sphere.OnDespawn();
        _spherePool.Enqueue(sphere);
    }

    public void ReleaseBoss(Bosses boss)
    {
        boss.OnDespawn();
    }
}