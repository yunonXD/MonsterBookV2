using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Protect : MonoBehaviour
{
    [SerializeField] private GameObject Hansel;
    [SerializeField] private int BuffSustain = 3;

    private void OnEnable()
    {
        StartCoroutine(_Protect(BuffSustain));
    }

    private void OnDisable()
    {
        return;
    }

    IEnumerator _Protect(int Count)
    {

        Hansel.GetComponent<Hansel_ControllerVer2>()._isProtect = true;

        yield return new WaitForSeconds(Count);

        Hansel.GetComponent<Hansel_ControllerVer2>()._isProtect = false;
        gameObject.SetActive(false);
    }
}
