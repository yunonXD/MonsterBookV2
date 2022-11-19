using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMatch_comfort : MonoBehaviour
{
    // Start is called before the first frame update
    private float time;
    private bool onetime;
    private GameObject Player;
    private GameObject Anna;
    
    
    void Start()
    {
        onetime = false;
        time = -1;
        gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("_Dissolve_Value", -1);
        Player = GameObject.FindWithTag("Player");
        Anna = GameObject.FindWithTag("Anna");
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
                Player.GetComponent<PlayerController>().m_OneHandWalkSpeed = 7.6f;
                Destroy(this.gameObject);
            }
        }

        if(Anna.GetComponent<Anna>().AnnaPhase == 2 && onetime == false)
        {
            time = 5;
            onetime = true;
        }

    }

    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().m_OneHandWalkSpeed = 9.6f;
        }

    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().m_OneHandWalkSpeed = 7.6f;
        }
    }

    public void deleteMatch()
    {
        time = 5;
    }

}
