using UnityEngine;
using System.Collections;

public class OrbitingManeuver : IManeuverBehaviour
{
    private bool _running = true;

    public void Maneuver(Bosses boss) =>
        boss.StartCoroutine(Orbit(boss));

    public void Stop() => _running = false;

    IEnumerator Orbit(Bosses boss)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) yield break;

        float angle = 0f;
        float radius = boss.weavingDistance * 2f;
        float orbitSpeed = boss.speed * 50f;

        while (_running && boss.IsAlive)
        {
            angle += orbitSpeed * Time.deltaTime;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(rad) * radius,
                0f,
                Mathf.Sin(rad) * radius
            );

            boss.transform.position = player.transform.position + offset;
            yield return null;
        }
    }
}