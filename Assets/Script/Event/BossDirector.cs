using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDirector : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 toWard;


    public void StartEvent(float time)
    {
        StartCoroutine(Routine(time));
    }

    private IEnumerator Routine(float time)
    {
        yield return YieldInstructionCache.waitForSeconds(time);
        var pos = transform.position + toWard;
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);

            yield return YieldInstructionCache.waitForFixedUpdate;
        }
    }

}
