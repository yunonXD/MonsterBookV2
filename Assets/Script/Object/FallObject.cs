using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour
{
    private Rigidbody rigid;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        rigid.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") || collision.gameObject.layer == LayerMask.NameToLayer("PlayerDash"))
        {
            rigid.isKinematic = false;
            StartCoroutine(Routine());
        }
    }

    private IEnumerator Routine()
    {
        yield return YieldInstructionCache.waitForSeconds(2f);
        Destroy(gameObject);
    }


}
