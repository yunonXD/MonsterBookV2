using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss_1Stage : MonoBehaviour
{
    public Image hansel_Hp;
    public Image Gretel_Hp;
    public GameObject Gretel_Face;
    public GameObject Gretel_Hit_Face;
    public GameObject Hensel_Face;
    public GameObject Hensel_Hit_Face;
    public GameObject Hansel;
    public GameObject Gretel;
    private float time = 0.0f;
    private float fill_Gretel;
    private float fill_Hansel;
    // Start is called before the first frame update

    void FaceChange()
    {
        Gretel_Hit_Face.SetActive(false);
        Hensel_Hit_Face.SetActive(false);
        Hensel_Face.SetActive(true);
        Gretel_Face.SetActive(true);
    }
    void HitFaceChange()
    {
        Gretel_Hit_Face.SetActive(true);
        Hensel_Hit_Face.SetActive(true);
        Hensel_Face.SetActive(false);
        Gretel_Face.SetActive(false);
    }

    void Start()
    {
        time = 0.0f;
        fill_Gretel = 1;
        fill_Hansel = 1;
}
    

    // Update is called once per frame
    void Update()
    {
        fill_Hansel = Hansel.GetComponent<Hansel>().CurrentHP / Hansel.GetComponent<Hansel>().HanselHP;
        fill_Gretel = Gretel.GetComponent<Gretel>().CurrentHP / Gretel.GetComponent<Gretel>().GretelHP;

        if (hansel_Hp.fillAmount != fill_Hansel || Gretel_Hp.fillAmount != fill_Gretel)
        {
            time = 2.0f * Time.deltaTime;

            hansel_Hp.fillAmount = Mathf.Lerp(hansel_Hp.fillAmount,fill_Hansel,time);
            Gretel_Hp.fillAmount = Mathf.Lerp(Gretel_Hp.fillAmount,fill_Gretel,time);
        }

    }
}
