using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireObject : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] private float speed;

    [SerializeField] private LayerMask targetLayer;
    private GameObject niddleObj;
    private ParticleSystem hitEffect;

    private bool hit;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        niddleObj = transform.GetChild(0).gameObject;
        hitEffect = transform.GetChild(1).GetComponent<ParticleSystem>();

        niddleObj.SetActive(false);
    }

    public void Shot(Vector3 pos, Vector3 target)
    {
        transform.position = pos;
        hit = false;        
        StartCoroutine(Routine(target));
    }

    private IEnumerator Routine(Vector3 target)
    {
        niddleObj.SetActive(true);
        transform.LookAt(target);        
        //rigid.AddForce(transform.forward * speed, ForceMode.Impulse);
        while (!hit)
        {
            var movePosition = Vector3.Lerp(transform.position, target, speed * Time.fixedDeltaTime);
            
            rigid.MovePosition(movePosition);
            if (Vector3.Distance(transform.position, target) <= 0.4f) hit = true;
            yield return YieldInstructionCache.waitForFixedUpdate;
        }
        rigid.velocity = Vector3.zero;
        niddleObj.SetActive(false);
        hitEffect.Play();
    }   
    
    public bool GetHit() { return hit; }

}
