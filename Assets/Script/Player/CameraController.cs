using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private Transform target;    
    [SerializeField] private float cameraSpeed;

    [SerializeField] private LayerMask targetLayer;
    private Vector3 startPos;
    
    private float shakeTime;
    private float shakePower;

    private Vector2 limitPos;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetChild(0).transform;

        startPos = transform.position;
    }

    private void Update()
    {
        if (shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakePower + transform.position; /*new Vector3(target.position.x, target.position.y + cameraPos.y, -10);*/
            shakeTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (limitPos == Vector2.zero) limitPos = startPos + new Vector3(-1000, 1000);        

        RaycastHit hit;
        var yPos = 0f;
        if (Physics.Raycast(target.position, Vector3.down, out hit, Mathf.Infinity, targetLayer))
        {
            yPos = hit.point.y;
        }

        var cameraPos = new Vector3(target.position.x, yPos + startPos.y, target.position.z) ;

        transform.position = Vector3.Lerp(transform.position, cameraPos, Time.deltaTime * cameraSpeed);

        if (transform.position.x <= limitPos.x + 14)
        {
            transform.position = new Vector3(limitPos.x + 14, cameraPos.y, cameraPos.z);
        }
        else if (transform.position.x >= limitPos.y - 14)
        {
            transform.position = new Vector3(limitPos.y - 14, cameraPos.y, cameraPos.z);
        }
    }

    public static void CameraShaking(float power = 1, float time = 0.3f)
    {
        Instance.shakePower = power;
        Instance.shakeTime = time;
    }

    public static void SetCameraLimit(float value, bool left = true)
    {
        if (left) Instance.limitPos.x = value;
        else Instance.limitPos.y = value;
    }

    public static void RotateCameraView(Vector3 euler = default(Vector3))
    {
        Instance.transform.rotation = Quaternion.Euler(euler);
    }

}
