using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;    
    [SerializeField] private float cameraSpeed;

    private Vector3 startPos;


    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(5).GetChild(0).transform;

        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        var cameraPos = new Vector3(target.position.x, startPos.y, startPos.z);

        transform.position = Vector3.Lerp(transform.position, cameraPos, Time.deltaTime * cameraSpeed);
    }

}
