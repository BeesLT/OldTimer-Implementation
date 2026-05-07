using UnityEngine;
using System.Collections;

public class RetreatingManeuver : IManeuverBehaviour
{
    private bool _running = true;

    public void Maneuver(Bosses boss) =>
        boss.StartCoroutine(Retreat(boss));

    public void Stop() => _running = false;

    IEnumerator Retreat(Bosses boss)
    {
        while (_running && boss.IsAlive)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) yield break;

            Vector3 awayFromPlayer =
                (boss.transform.position - player.transform.position).normalized;
            Vector3 target =
                boss.transform.position + awayFromPlayer * boss.fallbackDistance;

            float elapsed = 0f;
            float duration = 1.5f;
            Vector3 start = boss.transform.position;

            while (elapsed < duration)
            {
                boss.transform.position =
                    Vector3.Lerp(start, target, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}