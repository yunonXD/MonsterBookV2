using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup : MonoBehaviour
{
    public GameObject Gretel;
    public bool Solid = false;
    public GameObject SolidFood;
    // Start is called before the first frame update
    void Start()
    {
        Gretel = GameObject.FindWithTag("Gretel");
        SolidFood = GameObject.FindWithTag("BossBuff");

    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    public void Create()
    {
        if(Solid == false)
        {
            Invoke("DestroySoup", 2f);
        }
        else
        {
            Invoke("DestroySolidSoup", 2f);
        }

    }

    public void DestroySoup()
    {
        Soup_Object_Pool.ReturnObject(this);
    }
    public void DestroySolidSoup()
    {
        SolidFood.transform.position = this.transform.position;
        SolidFood.SetActive(true);
        Soup_Object_Pool.ReturnObject(this);
    }

    void Update()
    {

    }
}
