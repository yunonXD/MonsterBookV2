using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss_2Stage : MonoBehaviour
{
    //���� ü�� ����
    [SerializeField]
    private UIBossStick stick;

    [SerializeField]
    private UIGaugeWithPointer hpGauge;
    [ReadOnly]
    private float currentHpAmount;
    [ReadOnly]
    private float targetHpAmount;

    public float dampingTime = 2f;

    void Update()
    {
        //���� ü�� �������� ��
        currentHpAmount = Mathf.Lerp(currentHpAmount, targetHpAmount, Time.deltaTime * dampingTime);
        hpGauge.UpdateGauge(currentHpAmount);
    }

    public void OnHit()
    {
        stick.PlayChangeAnimation();
    }

}
