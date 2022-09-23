using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedObject : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Boss")
        {
            return;
        }
        else
        {
            IEntity entity = other.GetComponent<IEntity>();

            if (entity != null) entity.OnDamage(damage, this.transform.position);
        }
    }
 }
