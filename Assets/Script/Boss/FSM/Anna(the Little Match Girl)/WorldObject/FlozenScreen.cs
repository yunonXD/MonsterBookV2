using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlozenScreen : MonoBehaviour
{
    private float time;
    public bool Phasetwo;
    public bool FlozenStart;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        Phasetwo = false;
        FlozenStart = false;
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Frozen_Boundary", 1);
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Frozen_Control", -1);
    }

    // Update is called once per frame
    void Update()
    {
        if(FlozenStart == true)
        {
            time += Time.deltaTime;
            var con = gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_Frozen_Control");
            if(con < 1)
            {
                gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Frozen_Control", con + Time.deltaTime);
            }
            else
            {
                time = 0;
                FlozenStart = false;
            }

        }


        if (Phasetwo == true)
        {
            time += Time.deltaTime;

            var mat = gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_Frozen_Boundary");
            var con = gameObject.GetComponent<Renderer>().sharedMaterial.GetFloat("_Frozen_Control");


            if (mat <= 1)
            {
                gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Frozen_Control", con-Time.deltaTime);
            }
            else
            {
                gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Frozen_Control", 1);
                gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Frozen_Boundary", mat - Time.deltaTime);
            }
        }



    }
}
