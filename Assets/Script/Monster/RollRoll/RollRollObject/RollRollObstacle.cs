using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RollRollItem
{
    public class RollRollObstacle : MonoBehaviour
    {
        [SerializeField]
        private float Duration;
        [SerializeField]
        private int Damage;


        
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, Duration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var Player = other.gameObject.GetComponent<IEntity>();
                Player.OnDamage(Damage, transform.position);
            }

        }

    }
}

