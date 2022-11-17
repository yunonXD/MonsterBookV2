using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectCameraTimmer : Event
{
    [SerializeField] private float holdTime;
    private bool canUse;


    public override void StartEvent()
    {
        StartCoroutine(Routine(0));
    }

    public override void StartEvent(float time)
    {
        StartCoroutine(Routine(time));
    }

    private IEnumerator Routine(float time)
    {
        yield return YieldInstructionCache.waitForSeconds(time);
        CameraController.StartDirectCamera(transform.position);
        yield return YieldInstructionCache.waitForSeconds(holdTime);
        CameraController.EndDirecCamera();
    }


}
