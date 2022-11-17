using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperObject : Event
{
    [SerializeField] private Vector3 startVector;
    [SerializeField] private Vector3 foldVector;

    [SerializeField] private bool canSee;    
    [SerializeField] private float speed = 3f;
    [SerializeField] private bool awakeStart;
    [SerializeField] private float awakeDelay;

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
                PaperObject paper = child[i].GetComponent<PaperObject>();
                if (paper == null)
                {
                    child[i].SetActive(false);
                }
            }
        }
        transform.localRotation = startRotation;        
    }

    private void OnEnable()
    {
        if (awakeStart) StartCoroutine(Routine(awakeDelay));
    }

    public override void StartEvent()
    {
        StartCoroutine(Routine(0));
    }

    public override void StartEvent(float time)
    {
        StartCoroutine(Routine(time));        
    }

    public void SetRotate(float value)
    {
        var v = value / 180f;
        Debug.Log(v);
        transform.localRotation = Quaternion.Lerp(Quaternion.Euler(startVector), Quaternion.Euler(foldVector), v);
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
        while (Quaternion.Angle(transform.localRotation, foldRotation) > 0.1f)
        {            
            transform.localRotation = Quaternion.Lerp(transform.localRotation, foldRotation, speed * Time.deltaTime);
            yield return YieldInstructionCache.waitForFixedUpdate;
        }        
    }

}
