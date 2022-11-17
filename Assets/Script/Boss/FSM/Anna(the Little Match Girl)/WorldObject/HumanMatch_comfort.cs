using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMatch_comfort : MonoBehaviour
{
    // Start is called before the first frame update
    private float time;
    
    
    void Start()
    {
        time = -1;
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Dissolve_Value", -1);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time < 1)
        {
            gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Dissolve_Value", time);
        }
        else if(time > 1 && time < 5)
        {

        }
        else if(time > 5)
        {
            gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Dissolve_Value", 5-time);
            if(gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_Dissolve_Value") < -1)
            {
                Destroy(this.gameObject);
            }
        }



    }
}
