using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperObject : MonoBehaviour
{
    [SerializeField] private Vector3 startVector;
    [SerializeField] private Vector3 foldVector;

    [SerializeField] private bool canSee;

    private Quaternion startRotation;
    private Quaternion foldRotation;

    private GameObject[] child;


    private void Start()
    {
        if (startVector == Vector3.zero) startRotation = transform.localRotation;
        else startRotation.eulerAngles = startVector;

        foldRotation.eulerAngles = foldVector;

        child = new GameObject[transform.childCount];

        for (int i = 0; i < child.Length; i++)
        {
            child[i] = transform.GetChild(i).gameObject;
        }

        if (!canSee)
        {
            for (int i = 0; i < child.Length; i++)
            {
                child[i].SetActive(false);
            }
        }
        transform.localRotation = startRotation;
    }

    public void StartEvent(float time)
    {
        StartCoroutine(Routine(time));
    }

    private IEnumerator Routine(float time)
    {
        yield return YieldInstructionCache.waitForSeconds(time);
        if (!canSee)
        {
            for (int i = 0; i < child.Length; i++)
            {
                child[i].SetActive(true);
            }
        }
        transform.localRotation = startRotation;
        while (transform.localRotation != foldRotation)
        {            
            transform.localRotation = Quaternion.Lerp(transform.localRotation, foldRotation, 3 * Time.deltaTime);
            yield return YieldInstructionCache.waitForFixedUpdate;
        }        
    }

}
