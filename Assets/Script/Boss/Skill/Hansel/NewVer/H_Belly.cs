using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Belly : H_NSkillController
{
    private float m_Time;


    [SerializeField] private int _Player_To_Damage = 10;


    private void OnTriggerEnter(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();

        if (entity != null) entity.OnDamage(_Player_To_Damage, transform.parent.position);
    }

    private void FixedUpdate()
    {
        loadSkill();
    }


    private void loadSkill()
    {
        m_Time += Time.deltaTime;
        g_Skill_Hansel.GetComponent<Hansel_ControllerVer2>().m_rb.AddForce(transform.forward * 1f, ForceMode.VelocityChange);
        transform.LookAt(m_Player.transform);
        if (m_Time > 0.5)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnEnable()
    {
        g_Skill_Hansel.GetComponent<Hansel_ControllerVer2>().m_rb.velocity = Vector3.zero;
        m_Time = 0;
    }

    private void OnDisable()
    {

    }
}
