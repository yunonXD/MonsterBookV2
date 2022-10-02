using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] private Transform hpParent;
    private Image[] hpImage;

    [SerializeField] private Transform wireGaegeParent;
    private Image wireGaegeImage;

    [SerializeField] private RectTransform wireAim;


    private void Awake()
    {
        hpImage = new Image[hpParent.childCount];

        for (int i = 0; i < hpImage.Length; i++)
        {
            hpImage[i] = hpParent.GetChild(i).GetComponent<Image>();
        }

        wireGaegeImage = wireGaegeParent.GetChild(0).GetComponent<Image>();

    }

    public void UpdateHP(int hp)
    {
        for (int i = 0; i < hpImage.Length; i++)
        {
            hpImage[i].enabled = false;
            hpImage[i].fillAmount = 1;
        }

        var count = hp / 20;
        var harf = (hp - count * 20) / 10;

        for (int i = 0; i < count + harf; i++)
        {
            hpImage[i].enabled = true;
        }

        if (harf != 0) hpImage[count].fillAmount = 0.5f;
    }

    public void SetWireAim(Vector2 pos =default(Vector2), bool active = false)
    {
        wireAim.gameObject.SetActive(active);
        wireAim.position = Camera.main.WorldToScreenPoint(pos);
    }   

}
