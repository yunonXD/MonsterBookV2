using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotateObject : MonoBehaviour
{
    [SerializeField] private bool autoAxis;
    private IRotate player;

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<IRotate>();
        if (player != null)
        {
            if (autoAxis) player.Rotate(true);
            else player.CheckRotate(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player != null) player.CheckRotate(false);
    }
}
