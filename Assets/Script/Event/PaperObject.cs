using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperObject : MonoBehaviour
{
    [SerializeField] private Vector3 startVector;
    [SerializeField] private Vector3 foldVector;

    private Quaternion startRotation;
    private Quaternion foldRotation;

    private GameObject child;


    private void Start()
    {
        if (startVector == Vector3.zero) startRotation = transform.localRotation;
        else startRotation.eulerAngles = startVector;

        foldRotation.eulerAngles = foldVector;

        child = transform.GetChild(0).gameObject;

        child.SetActive(false);
        transform.localRotation = startRotation;
    }

    public void StartEvent(float time)
    {
        StartCoroutine(Routine(time));
    }

    private IEnumerator Routine(float time)
    {
        yield return YieldInstructionCache.waitForSeconds(time);
        child.SetActive(true);
        transform.localRotation = startRotation;
        while (transform.localRotation != foldRotation)
        {            
            transform.localRotation = Quaternion.Lerp(transform.localRotation, foldRotation, 3 * Time.deltaTime);
            yield return YieldInstructionCache.waitForFixedUpdate;
        }        
    }

}
