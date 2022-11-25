using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSceneDirector : MonoBehaviour
{
    [Header("Stage 1")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask turnOnCameraLayer;
    [SerializeField] private LayerMask turnOffCameraLayer;
    [SerializeField] private Hansel hansel;

    [SerializeField] private Transform leftHinge;
    [SerializeField] private Transform rightHinge;
    [SerializeField] private GameObject ground;


    public void LoadScene(int i)
    {
        StartCoroutine(LoadSceneRoutine(i));
    }

    private IEnumerator LoadSceneRoutine(int i)
    {
        yield return YieldInstructionCache.waitForSeconds(2f);
        SceneManager.LoadScene(i);
    }


    public void StartStage(int i)
    {
        if (i == 1) StartCoroutine(Stage1StartRoutine());
    }

    private IEnumerator Stage1StartRoutine()
    {
        yield return YieldInstructionCache.waitForSeconds(0.2f);
        GameManager.SetInGameInput(false);
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.ChangePatrol();
        yield return YieldInstructionCache.waitForSeconds(2f);
        //while (player.transform.position != new Vector3())
        //{

        //    yield return YieldInstructionCache.waitForFixedUpdate;
        //}
        yield return YieldInstructionCache.waitForSeconds(4f);
        var count = 0;
        while (count <= 7)
        {
            mainCamera.cullingMask = turnOnCameraLayer;
            yield return YieldInstructionCache.waitForSeconds(0.06f);
            mainCamera.cullingMask = turnOffCameraLayer;
            count++;
            yield return YieldInstructionCache.waitForFixedUpdate;
        }
        mainCamera.cullingMask = turnOnCameraLayer;
        yield return YieldInstructionCache.waitForSeconds(1.3f);
        GameManager.SetInGameInput(true);        
        hansel.Hansel_Start();
    }

    public void EndStage(int i)
    {
        if (i == 1)
        {
            StartCoroutine(Stage1EndRoutine());
        }
        else if (i == 2)
        {

        }
    }

    private IEnumerator Stage1EndRoutine()
    {        
        GameManager.SetInGameInput(false);
        ground.SetActive(false);
        while (leftHinge.localRotation != Quaternion.Euler(0, 74f, 0))
        {            
            leftHinge.localRotation = Quaternion.Lerp(leftHinge.localRotation, Quaternion.Euler(0, 74f, 0), Time.deltaTime * 4);
            rightHinge.localRotation = Quaternion.Lerp(rightHinge.localRotation, Quaternion.Euler(0, -74f, 0), Time.deltaTime * 4);            
            yield return YieldInstructionCache.waitForFixedUpdate;
        }
        GameManager.FadeEffect(true, 1);
        yield return YieldInstructionCache.waitForSeconds(3f);
        SceneManager.LoadScene(3);
    }

}
