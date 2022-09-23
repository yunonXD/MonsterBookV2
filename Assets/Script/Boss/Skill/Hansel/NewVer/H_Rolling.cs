using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class H_Rolling : H_NSkillController
{
    [SerializeField] private int _Player_To_Damage = 10;

    [SerializeField] private Transform[] m_Pointer;

    [Header("ÁÂ¿ì ÀÌµ¿ ¼Óµµ Á¶Á¤")]
    [SerializeField] private float m_MoveSpeed = 2;

    private int m_CountPointer = 0;


    private void OnTriggerEnter(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();

        if (entity != null) entity.OnDamage(_Player_To_Damage, transform.parent.position);
    }

    private void FixedUpdate()
    {
        Debug.Log("m_CountPointer : " + m_CountPointer);
        _MoveNext();
    }


    private void OnEnable()
    {
        g_Skill_Hansel.GetComponent<BoxCollider>().isTrigger = true;
        g_Skill_Hansel.GetComponent<Rigidbody>().isKinematic = true;


    }

    private void OnDisable()
    {
        g_Skill_Hansel.GetComponent<BoxCollider>().isTrigger = false;
        g_Skill_Hansel.GetComponent<Rigidbody>().isKinematic = false;
        m_CountPointer = 0;
    }


    private void _MoveNext()
    {
        g_Skill_Hansel.transform.position = Vector3.MoveTowards(
            g_Skill_Hansel.transform.position, m_Pointer[m_CountPointer].transform.position, m_MoveSpeed * Time.deltaTime);
        m_animator.SetFloat("H_Walk", 1);

        g_Skill_Hansel.transform.LookAt(m_Pointer[m_CountPointer].transform);

        if (g_Skill_Hansel.transform.position == m_Pointer[m_CountPointer].transform.position)
            m_CountPointer++;

        if (m_CountPointer == m_Pointer.Length)
        {
            Debug.Log("µµÂø");
            gameObject.SetActive(false);
            return;
        }

    }




}
