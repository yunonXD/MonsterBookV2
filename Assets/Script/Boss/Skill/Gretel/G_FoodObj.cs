using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_FoodObj : MonoBehaviour
{
    [SerializeField] private int _Player_To_Damage = 10;


    private void OnTriggerEnter(Collider other)
    {
        IEntity entity = other.GetComponent<IEntity>();

        if (entity != null) entity.OnDamage(_Player_To_Damage, Vector3.zero);


        if (other.CompareTag("Ground") || other.CompareTag("Boss") || other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }
}