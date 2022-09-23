using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_FoodGround : MonoBehaviour
{
    [SerializeField] private GameObject m_RandomObj;
    [SerializeField] private GameObject m_FoodBoi;
    private BoxCollider m_Randomcol;
    private bool isFood = true;
    private int isCheck = 0;

    private void Awake()
    {
        m_Randomcol = m_RandomObj.GetComponent<BoxCollider>();

    }


    private void OnEnable()
    {
        isFood = true;
        StartCoroutine(Spawn_Food());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator Spawn_Food()
    {
        while (isFood)
        {
            yield return new WaitForSeconds(0.5f);

            GameObject fFood = Instantiate(m_FoodBoi, RandomPosition(),
                Quaternion.identity);
            isCheck++;

            if (isCheck == 3)
            {
                isFood = false;
                isCheck = 0;
                
                gameObject.SetActive(false);
            }
        }


    }


    Vector3 RandomPosition()
    {
        Vector3 originPosition = m_RandomObj.transform.position;

        float range_X = m_Randomcol.bounds.size.x;
        float range_Z = m_Randomcol.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, 0);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }

}
