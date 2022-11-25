using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anna_WorldEvent : MonoBehaviour
{
    public GameObject FrozenScreen;
    private float time;
    [HideInInspector] public  bool GrannyAble;
    [HideInInspector] public bool HumanMatchAble;
    public float GrannyCoolTime;
    public float HumanMatchCoolTime;
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

    private GameObject Player;
    

    public void LastMatch()
    {
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[0].transform);
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[1].transform);
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[2].transform);
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[3].transform);
        Instantiate(HumanMatch_finish, HumanMatch_finish_SpawnPoint[4].transform);
    }

    public void CreateGranny()
    {
        Instantiate(Granny, Granny1_SpawnPoint.transform.position, new Quaternion(0, 0, 0, 0));
        Instantiate(Granny, Granny2_SpawnPoint.transform.position,new Quaternion(0,0,0,0));
        FrozenScreen.GetComponent<FlozenScreen>().Phasetwo = true;
    }

    public void CreateProtectedArea()
    {
        Instantiate(HumanMatch_Blizzard, HumanMaych_Blizzard_SpawnPoint.transform.position, new Quaternion(0, 180, 180, 0));
        Instantiate(ComfortZone, HumanMaych_Blizzard_SpawnPoint.transform.position, new Quaternion(0, 180, 180, 0));
        ComfortZone.GetComponent<ParticleSystem>().Play();
        ComfortZoneRenderer.sharedMaterial.SetFloat("_Dissolve_Value", -1);
    }
    public void BlizzardStart()
    {
        Player.GetComponent<PlayerController>().m_OneHandWalkSpeed = 7.6f;
        FrozenScreen.GetComponent<FlozenScreen>().FlozenStart = true;
        Instantiate(HumanMatch_Blizzard, HumanMaych_Blizzard_SpawnPoint.transform.position, new Quaternion(0, 180, 180, 0));
        Instantiate(ComfortZone, HumanMaych_Blizzard_SpawnPoint.transform.position, new Quaternion(0, 180, 180, 0));
        ComfortZone.GetComponent<ParticleSystem>().Play();
        ComfortZoneRenderer.sharedMaterial.SetFloat("_Dissolve_Value", -1);
        Blizzard.Play();  //눈보라 실행          
        HumanMatchAble = true;

    }

    public void BlizzardEnd()
    {
        HumanMatchAble = false;
        Blizzard.Stop();  //눈보라 실행
    }
    void Start()
    {
        ComfortZoneRenderer = ComfortZone.GetComponent<Renderer>();
        FrozenScreen = GameObject.FindWithTag("FrozenScreen");
        Player = GameObject.FindWithTag("Player");
        GrannyAble = false;
        HumanMatchAble = false;
    }


    void Update()
    {
        if (GrannyAble == true)
        {
            time += Time.deltaTime;
            if(time > GrannyCoolTime+10f)   //할머니 지속시간 10초
            {
                BlizzardEnd();
                CreateGranny();
                Debug.Log("할머니소환쿨");
                FrozenScreen.GetComponent<FlozenScreen>().Phasetwo = true;
                time = 0f;
            }
        }

        if (HumanMatchAble == true)
        {
            time += Time.deltaTime;
            if (time > HumanMatchCoolTime + 6f)   //할머니 지속시간 10초
            {
                CreateProtectedArea();
                time = 0f;
            }
        }


        if (Input.GetKeyDown(KeyCode.I))
        {
            LastMatch();
        }


    }


}
