using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anna_StandArea : MonoBehaviour
{

    private GameObject Anna;
    
    // Start is called before the first frame update
    void Start()
    {
        Anna = GameObject.FindWithTag("Anna");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Anna.GetComponent<Anna>().AnnaStand();
            Destroy(this);
        }
    }

}
