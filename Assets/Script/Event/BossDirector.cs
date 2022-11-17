using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossDirector : MonoBehaviour
{
    private bool startEvent;

    [SerializeField] private float speed;
    [SerializeField] private Vector3 toWard;

    [Header("")]
    [SerializeField] private Vector3[] cameraPos;
    [SerializeField] private float[] cameraSpeed;

    [SerializeField] Transform crackHouse;

    [Header("[Move Effector]")]
    [SerializeField] private MoveObject[] moveEffect;
    [SerializeField] private float[] moveTime;

    [Header("[Paper Effector]")]
    [SerializeField] private PaperObject[] paperEffect;
    [SerializeField] private float[] paperTime;


    private void Start()
    {
        for (int i = 0; i < cameraPos.Length; i++)
        {
            cameraPos[i] += transform.position;
        }
    }

    public void StartEvent()
    {
        startEvent = true;        
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {        
        StartMoveEffect();
        CameraController.StartDirectCamera(cameraPos[1]);
        yield return YieldInstructionCache.waitForSeconds(1.4f);

        CameraController.StartDirectCamera(cameraPos[0], 1);

        yield return YieldInstructionCache.waitForSeconds(2f);
        StartPapaerEffect();
        yield return YieldInstructionCache.waitForSeconds(5.2f);

        CameraController.StartDirectCamera(cameraPos[1]);
    }

    private void StartPapaerEffect()
    {
        for (int i = 0; i < paperEffect.Length; i++)
        {
            paperEffect[i].StartEvent(paperTime[i]);
        }
    }

    private void StartMoveEffect()
    {
        for (int i = 0; i < moveEffect.Length; i++)
        {
            moveEffect[i].StartMove(moveTime[i]);
        }
    }

    public void TeleportBossRoom(float time)
    {
        StartCoroutine(TPBossRoutine(time));
    }

    private IEnumerator TPBossRoutine(float time)
    {
        yield return YieldInstructionCache.waitForSeconds(time);

        GameObject.FindGameObjectWithTag("Player").transform.position = crackHouse.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!startEvent) StartEvent();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }

}
