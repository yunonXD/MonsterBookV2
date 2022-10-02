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
        gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    public void Create()
    {
        if(Solid == false)
        {
            StartCoroutine(DestroySoup());

        }
        else
        {
            StartCoroutine(DestroySolidSoup());
        }

    }

    public IEnumerator DestroySoup()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        Soup_Object_Pool.ReturnObject(this);
    }
    private IEnumerator DestroySolidSoup()
    {
        yield return new WaitForSecondsRealtime(2.0f);
        SolidFood.transform.position = this.transform.position;
        SolidFood.SetActive(true);
        Soup_Object_Pool.ReturnObject(this);
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;

        }

        if(other.tag == "Boss")
        {
            if(SolidFood == true)
            {
                gameObject.tag = "BossBuff";
                Soup_Object_Pool.ReturnObject(this);
            }
        }
    }


}
