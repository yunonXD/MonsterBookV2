using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedObject : MonoBehaviour
{
    [SerializeField] private int damage;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IEntity entity = other.GetComponent<IEntity>();
            if (entity != null) entity.OnDamage(damage, transform.position);
        }
        
    }

}
