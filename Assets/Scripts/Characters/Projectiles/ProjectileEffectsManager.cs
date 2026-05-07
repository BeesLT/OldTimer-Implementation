using UnityEngine;
using System.Collections.Generic;

public class ProjectileEffectsManager : MonoBehaviour
{
    public static ProjectileEffectsManager Instance { get; private set; }

    [Header("Impact Particles")]
    public ParticleSystem impactPrefab;
    public int poolSize = 10;

    private Queue<ParticleSystem> _pool = new Queue<ParticleSystem>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitPool();
    }

    void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            ParticleSystem ps = Instantiate(impactPrefab);
            ps.gameObject.SetActive(false);
            _pool.Enqueue(ps);
        }
    }

    public void PlayImpact(Vector3 position)
    {
        ParticleSystem ps = _pool.Count > 0
            ? _pool.Dequeue()
            : Instantiate(impactPrefab);

        ps.transform.position = position;
        ps.gameObject.SetActive(true);
        ps.Play();

        StartCoroutine(ReturnAfterPlay(ps));
    }

    System.Collections.IEnumerator ReturnAfterPlay(ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.main.duration + ps.main.startLifetime.constantMax);
        ps.Stop();
        ps.gameObject.SetActive(false);
        _pool.Enqueue(ps);
    }
}