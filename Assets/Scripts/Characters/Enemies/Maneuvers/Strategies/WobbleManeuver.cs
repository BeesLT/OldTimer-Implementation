using UnityEngine;
using System.Collections;

public class WobbleManeuver : IManeuverBehaviour
{
    private bool _running = true;

    public void Maneuver(Bosses boss) =>
        boss.StartCoroutine(Wobble(boss));

    public void Stop() => _running = false;

    IEnumerator Wobble(Bosses boss)
    {
        while (_running && boss.IsAlive)
        {
            float wobble = Mathf.Sin(Time.time * boss.wobbleAmount);
            boss.transform.Translate(Vector3.up * boss.speed * Time.deltaTime);
            boss.transform.Translate(Vector3.right * wobble * Time.deltaTime);
            yield return null;
        }
    }
}