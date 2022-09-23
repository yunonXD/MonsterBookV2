using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Knife_L : MonoBehaviour
{
    [SerializeField] private GameObject m_Start;
    Vector3 m_Pos;
    [SerializeField] private GameObject m_Target;
    [SerializeField] private int _Player_To_Damage = 10;


    private void OnTriggerEnter(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();

        if (entity != null) entity.OnDamage(_Player_To_Damage, transform.parent.position);
    }
    private void OnEnable()
    {
        m_Pos = m_Start.transform.position;
        transform.position = m_Pos;
    }

    private void FixedUpdate()
    {
        StartCoroutine(Knife_L());
        //transform.position = Vector3.Lerp(gameObject.transform.position, m_Target.transform.position, 0.05f);
    }


    private void OnDisable()
    {
        return;
    }

    IEnumerator Knife_L()
    {
        transform.position = Vector3.Lerp(gameObject.transform.position, m_Target.transform.position, 0.05f);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
