using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anna_WorldEvent : MonoBehaviour
{
    public GameObject FrozenScreen;

    public GameObject Granny;
    public GameObject Granny1_SpawnPoint;
    public GameObject Granny2_SpawnPoint;

    public ParticleSystem Blizzard;
    public GameObject HumanMatch_Blizzard;
    public GameObject HumanMaych_Blizzard_SpawnPoint;
    public GameObject ComfortZone;
    private Renderer ComfortZoneRenderer;

    public GameObject[] HumanMatch_finish_SpawnPoint;
    public GameObject HumanMatch_finish;
    

    void LastMatch()
    {
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[0].transform);
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[1].transform);
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[2].transform);
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[3].transform);
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[4].transform);
    }

    void CreateGranny()
    {
        Instantiate(Granny, Granny1_SpawnPoint.transform);
        Instantiate(Granny, Granny2_SpawnPoint.transform);
    }

    void BlizzardStart()
    {
        FrozenScreen.GetComponent<FlozenScreen>().FlozenStart = true;
        Blizzard.Play();  //传焊扼 角青
        Instantiate(HumanMatch_Blizzard, HumanMaych_Blizzard_SpawnPoint.transform.position, new Quaternion(0, 180, 180, 0)); 
        Instantiate(ComfortZone, HumanMaych_Blizzard_SpawnPoint.transform.position, new Quaternion(0, 180, 180, 0)); 
        ComfortZone.GetComponent<ParticleSystem>().Play();                                
        ComfortZoneRenderer.sharedMaterial.SetFloat("_Dissolve_Value",-1);                 

    }

    void BlizzardEnd()
    {
        Blizzard.Stop();  //传焊扼 角青
    }
    void Start()
    {
        ComfortZoneRenderer = ComfortZone.GetComponent<Renderer>();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            BlizzardStart();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            BlizzardEnd();
            CreateGranny();
            FrozenScreen.GetComponent<FlozenScreen>().Phasetwo = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            LastMatch();
        }


    }


}
