using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Rush : H_NSkillController
{
    private float m_Time;
    [SerializeField] private int _Player_To_Damage = 10;
    private Vector3 m_PlayerPosition;


    private void OnTriggerEnter(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();

        if (entity != null)
        {
            entity.OnDamage(_Player_To_Damage, transform.parent.position);

        }

        if (other.CompareTag("Player"))
        {
            g_Skill_Hansel.GetComponent<BoxCollider>().isTrigger = true;
            g_Skill_Hansel.GetComponent<Rigidbody>().useGravity = false;
        }

        if (other.CompareTag("Wall"))
        {
            g_Skill_Hansel.GetComponent<Hansel_ControllerVer2>().m_rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }


    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            g_Skill_Hansel.GetComponent<BoxCollider>().isTrigger = false;
            g_Skill_Hansel.GetComponent<Rigidbody>().useGravity = true;
        }      
    }



    private void FixedUpdate()
    {
        loadSkill();
    }
    private void loadSkill()
    {
        m_animator.SetTrigger("H_RushAttack");
        m_Time += Time.deltaTime;
        g_Skill_Hansel.GetComponent<Hansel_ControllerVer2>().m_rb.AddForce(transform.forward * 5f , ForceMode.Acceleration);

        //g_Skill_Hansel.GetComponent<Hansel_ControllerVer2>().m_rb.velocity = Vector3.forward * 5;
        if (m_Time > 6.0)
        {
            g_Skill_Hansel.GetComponent<Hansel_ControllerVer2>().m_rb.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }

    }

    private void OnEnable()
    {
        m_Time = 0;

        m_PlayerPosition = new Vector3(m_Player.transform.position.x, transform.position.y, transform.position.z);
        g_Skill_Hansel.transform.LookAt(m_PlayerPosition);

        //g_Skill_Hansel.GetComponent<BoxCollider>().isTrigger = true;
        //g_Skill_Hansel.GetComponent<Rigidbody>().isKinematic = true;

    }

    private void OnDisable()
    {
        //g_Skill_Hansel.GetComponent<BoxCollider>().isTrigger = false;
        //g_Skill_Hansel.GetComponent<Rigidbody>().isKinematic = false;

    }















    //private int m_CurrentNode = 0;
    //[SerializeField] private List<Transform> m_WayPointer = new List<Transform>();

    //private void OnEnable()
    //{
    //    g_Skill_Hansel.GetComponent<Hansel_Controller>()._isOnSkill = true;
    //    NextPath();
    //    m_nvAgent.autoBraking = false;
    //    m_nvAgent.speed = 10;
    //}

    //private void OnDisable()
    //{
    //    g_Skill_Hansel.GetComponent<Hansel_Controller>()._isOnSkill = false;
    //    m_nvAgent.autoBraking = true;
    //    m_nvAgent.speed = 1;
    //}


    //private void FixedUpdate()
    //{
    //    if (!m_nvAgent.pathPending && m_nvAgent.remainingDistance < 2f)
    //        NextPath();

    //    if (m_CurrentNode == m_WayPointer.Count)
    //    {
    //        m_CurrentNode = 0;

    //        gameObject.SetActive(false);

    //    }
    //}

    //void NextPath()
    //{
    //    Debug.Log(m_CurrentNode);
    //    Debug.Log("±æÃ£±â");
    //    m_nvAgent.destination = m_WayPointer[m_CurrentNode].position;
    //    m_CurrentNode = (m_CurrentNode + 1);
    //}
}
