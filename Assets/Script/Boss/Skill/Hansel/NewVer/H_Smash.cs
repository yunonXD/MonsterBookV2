using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class H_Smash : H_NSkillController
{

    [SerializeField] GameObject m_Skill1;
    [SerializeField] GameObject m_Skill2;
    [SerializeField] GameObject m_Skill3;
    [SerializeField] private int _Player_To_Damage = 10;
    [SerializeField] private GameObject m_BoxCollider;


    private void OnTriggerEnter(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();

        if (entity != null) entity.OnDamage(_Player_To_Damage, transform.parent.position);
    }


    private void OnEnable()
    {       
        if (m_Skill1.activeSelf == true || m_Skill2.activeSelf == true ||
            m_Skill3.activeSelf == true)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
            return;
        }
        else
            StartCoroutine(AttackDelay(1.0f));
    }

    private void OnDisable()
    {
       // m_BoxCollider.SetActive(false);
        return;
    }

    IEnumerator AttackDelay(float count)
    {

        m_animator.SetTrigger("H_SmashAttack");

        yield return new WaitForSeconds(count);

        //m_BoxCollider.SetActive(true);

        gameObject.SetActive(false);

    }


}
