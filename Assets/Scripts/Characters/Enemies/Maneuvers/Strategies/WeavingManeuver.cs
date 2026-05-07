using UnityEngine;
using System.Collections;

public class WeavingManeuver : IManeuverBehaviour
{
    private bool _running = true;

    public void Maneuver(Bosses boss)
    {
        boss.StartCoroutine(Weave(boss));
    }

    public void Stop() => _running = false;

    IEnumerator Weave(Bosses boss)
    {
        bool isReverse = false;
        Vector3 startPosition = boss.transform.position;
        Vector3 endPosition = startPosition + Vector3.right * boss.weavingDistance;

        while (_running && boss.IsAlive)
        {
            float time = 0f;
            Vector3 start = boss.transform.position;
            Vector3 end = isReverse ? startPosition : endPosition;

            while (time < boss.speed)
            {
                boss.transform.position = Vector3.Lerp(start, end, time / boss.speed);
                time += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            isReverse = !isReverse;
        }
    }
}