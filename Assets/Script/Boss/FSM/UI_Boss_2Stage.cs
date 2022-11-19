using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Boss_2Stage : MonoBehaviour
{
    //보스 체력 정보
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
        //몬스터 체력 가져오는 법
        currentHpAmount = Mathf.Lerp(currentHpAmount, targetHpAmount, Time.deltaTime * dampingTime);
        hpGauge.UpdateGauge(currentHpAmount);
    }

    public void OnHit()
    {
        stick.PlayChangeAnimation();
    }

}
