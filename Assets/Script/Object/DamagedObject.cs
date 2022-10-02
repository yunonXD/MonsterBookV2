using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedObject : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerStay(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();
        if (entity != null) entity.OnDamage(damage, transform.position);        
    }

}
