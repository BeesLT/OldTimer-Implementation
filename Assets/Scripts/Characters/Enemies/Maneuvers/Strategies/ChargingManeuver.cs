using UnityEngine;
using System.Collections;

public class ChargingManeuver : IManeuverBehaviour
{
    private bool _running = true;

    public void Maneuver(Bosses boss) =>
        boss.StartCoroutine(Charge(boss));

    public void Stop() => _running = false;

    IEnumerator Charge(Bosses boss)
    {
        while (_running && boss.IsAlive)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) yield break;

            Vector3 target = player.transform.position;
            float elapsed = 0f;
            float duration = 0.8f;
            Vector3 start = boss.transform.position;

            while (elapsed < duration)
            {
                boss.transform.position =
                    Vector3.Lerp(start, target, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            boss.PerformAttack(player);
            yield return new WaitForSeconds(boss.attackCooldown);
        }
    }
}