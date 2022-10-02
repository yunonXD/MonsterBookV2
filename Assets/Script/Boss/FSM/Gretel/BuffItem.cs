using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    { 
        if(other.collider.CompareTag("Boss"))
        {
            //ÇîÁ© ¹öÇÁ Bool  true
            gameObject.transform.position = new Vector3(500, 500, 500);
        }

        else if (other.collider.CompareTag("Player"))
        {
            gameObject.transform.position = new Vector3(500, 500, 500);
        }
    }


    
}
