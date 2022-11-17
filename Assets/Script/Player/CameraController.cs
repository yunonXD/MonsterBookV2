
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private Transform cameraTrans;
    private Transform target;
    private Transform player;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float startYPos;
    [SerializeField] private Vector3 basicRotation;

    [SerializeField] private LayerMask targetLayer;
    private Vector3 startPos;
    private float lastYPos;

    #region Camera Shake
    private float shakeTime;
    private float shakePower;

    #endregion

    private Vector2 limitPos;
    private Vector2 limitYPos;

    private Axis curAxis;
    private Quaternion rotation;
    private bool type;

    #region Direct Vlaue
    private Vector3 directPos;
    private float directSpeed;
    private bool directBool;

    private bool moveCameraBool;
    private Vector3 moveCameraPos;
    private Quaternion moveCameraRotation;

    #endregion

    [SerializeField] private Volume volume;
    private DepthOfField depth;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = player.GetChild(1).GetChild(0).transform;
        cameraTrans = transform.GetChild(0).transform;
        startPos = transform.position;

        if (volume != null) volume.profile.TryGet<DepthOfField>(out depth);

        limitYPos = new Vector2(100, -20);
    }    

    private void Update()
    {
        if (shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakePower + transform.position; /*new Vector3(target.position.x, target.position.y + cameraPos.y, -10);*/
            shakeTime -= Time.deltaTime;
        }
        if (type)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 4);
            if (transform.rotation == rotation) type = false;
        }
        if (moveCameraBool)
        {
            cameraTrans.position = Vector3.Lerp(cameraTrans.position, moveCameraPos, Time.deltaTime * 2);
            cameraTrans.localRotation = Quaternion.Lerp(cameraTrans.localRotation, moveCameraRotation, Time.deltaTime * 2);
        }
    }

    private void FixedUpdate()
    {
        if (limitPos == Vector2.zero) limitPos = startPos + new Vector3(-1000, 1000);        

        if (directBool)
        {
            transform.position = Vector3.Lerp(transform.position, directPos, Time.deltaTime * directSpeed);
        }
        else
        {
            RaycastHit hit;
            var yPos = 0f;
            //Physics.BoxCast(target.position, new Vector3(0.5f, 0.5f, 1f), Vector3.down, out hit, Quaternion.identity, 8, targetLayer)
            if (Physics.Raycast(target.position, Vector3.down, out hit, 10, targetLayer))
            {
                yPos = hit.point.y;
                lastYPos = yPos;                
            }
            else
            {
                yPos = player.position.y;
            }

            var cameraPos = curAxis == Axis.XAxis ? new Vector3(target.position.x, yPos + startYPos, player.position.z) : new Vector3(player.position.x, yPos + startYPos, target.position.z);

            transform.position = Vector3.Lerp(transform.position, cameraPos, Time.deltaTime * cameraSpeed);

            if (transform.position.x <= limitPos.x + 14)
            {
                transform.position = new Vector3(limitPos.x + 14, cameraPos.y, cameraPos.z);
            }
            else if (transform.position.x >= limitPos.y - 14)
            {
                transform.position = new Vector3(limitPos.y - 14, cameraPos.y, cameraPos.z);
            }
            //if (transform.position.y <= limitYPos.y + 2)
            //{
            //    transform.position = new Vector3(cameraPos.x, limitYPos.y + 2, cameraPos.z);
            //}
            //else if (transform.position.y >= limitYPos.x - 2)
            //{
            //    transform.position = new Vector3(cameraPos.x, limitYPos.x - 2, cameraPos.z);
            //}
        }
    }

    public static void SetCameraMove(Vector3 pos, Quaternion rotation)
    {
        
    }

    public static void StartDirectCamera(Vector3 pos, float sp = 4)
    {
        Instance.directBool = true;
        Instance.directPos = pos;
        Instance.directSpeed = sp;
    }

    public static void EndDirecCamera() { Instance.directBool = false; }

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

    public static void SetYCameraLimit(Vector2 value)
    {
        Instance.limitYPos = value;
    }

    public static void RotateCameraView(bool right)
    {
        Instance.type = true;
        //Instance.rotation = axis == Axis.XAxis ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0, -90, 0));
        Instance.rotation = right ? Quaternion.Euler(Instance.rotation.eulerAngles + new Vector3(0, -90, 0)) : Quaternion.Euler(Instance.rotation.eulerAngles + new Vector3(0, 90, 0));
    }

    public static void SetCameraView(Vector3 value = default(Vector3))
    {
        Instance.rotation = Quaternion.Euler(value);
    }

    public static void SetGlobalVolume(float value)
    {
        Instance.StartCoroutine(Instance.GlobalRoutine(value));
    }

    private IEnumerator GlobalRoutine(float value)
    {
        while (depth.focusDistance != value)
        {
            depth.focusDistance.value = Mathf.Lerp(depth.focusDistance.value, value, Time.deltaTime * 3);

            yield return YieldInstructionCache.waitForFixedUpdate;
        }
    }

    public static Vector3 GetCameraPos() { return Instance.transform.GetChild(0).position; }

}
