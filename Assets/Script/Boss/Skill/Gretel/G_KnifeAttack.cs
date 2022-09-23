using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_KnifeAttack : MonoBehaviour
{

    [SerializeField] private GameObject m_Knife_R;
    [SerializeField] private GameObject m_Knife_L;

    private void OnEnable()
    {
        StartCoroutine(_Knife());
    }

    private void OnDisable()
    {
        return;
    }

    IEnumerator _Knife()
    {
        yield return new WaitForSeconds(1.0f);
        m_Knife_R.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        m_Knife_L.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        gameObject.SetActive(false);
    }
}
