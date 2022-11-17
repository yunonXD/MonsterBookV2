using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSound : MonoBehaviour
{
    public GameObject[] SoundObject;  //0 = ���� 1 = ���� 2 = ��ں� 3 = �丮


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WorldSoundPlay(string soundname,int num)
    {
        SoundManager.PlayVFXLoopSound(soundname, SoundObject[num].transform);
    }

    public void WorldSoundStop(string soundname)
    {
        SoundManager.StopVFXLoopSound(soundname);
    }





}
