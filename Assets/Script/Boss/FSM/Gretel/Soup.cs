using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup : MonoBehaviour
{
    public GameObject Gretel;
    public bool Solid = false;
    public GameObject SolidFood;
    public bool down = true;
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
        if (transform.position.y >= 0.5)
        {
           transform.position = new Vector3(transform.position.x, transform.position.y - 0.08f, transform.position.z);
        }
        else
        {

        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Boss")
        {
            if(SolidFood == true)
            {
                gameObject.tag = "BossBuff";
                Soup_Object_Pool.ReturnObject(this);
            }
        }

        else if (other.tag == "Player")
        {
            IEntity entity = other.GetComponent<IEntity>();
            if (entity != null) entity.OnDamage(other.gameObject.GetComponent<PlayerController>().str, transform.position);
        }
    }

}
