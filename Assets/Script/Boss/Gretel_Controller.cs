using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gretel_Controller : MonoBehaviour, IEntity
{

    public enum g_CurrentState
    {
        idle,
        KnifeAttack,
        FoodAttack,
        HanselGuard,
        Stuned,
        win
    }

    public g_CurrentState curState = g_CurrentState.idle;
    private Animator m_animator;
    private int m_CountKnife = 0;

    private bool _isDead = false;
    private bool _isAttackable = false;
    private bool _isDamaged = false;
    private bool _isFoodable = false;

    [SerializeField] private float m_GretelHP = 100;
    [SerializeField] private TextMeshProUGUI m_Text;
    [SerializeField] private GameObject m_Player;
    [SerializeField] private GameObject m_Hansel;

    [Header("공격 -스킬-")]
    [SerializeField] private GameObject m_KnifeAttack;
    [SerializeField] private GameObject m_FoodAttack;
    [SerializeField] private GameObject m_Protect;
    [Header("----------")]


    [SerializeField] private Slider Gretel_HPBar;


    private void Start()
    {
        m_animator = GetComponent<Animator>();

        StartCoroutine(CheckState());
        StartCoroutine(CheckStateAction());
     
        Gretel_HPBar.value = m_GretelHP;
    }


    IEnumerator CheckState()
    {
        while (!_isDead)
        {
            yield return new WaitForSeconds(5.0f);

            if(_isFoodable == true)
            {              
                curState = g_CurrentState.FoodAttack;
            }
            else if (m_Player.activeSelf == true)
            {
                curState = g_CurrentState.KnifeAttack;
            }

        }
    }


    IEnumerator CheckStateAction()
    {
        while (!_isDead)
        {
            switch (curState)
            {
                case g_CurrentState.idle:
                    m_Text.text = "idle";

                    if (!_isAttackable &&
                        m_Hansel.GetComponent<Hansel_ControllerVer2>().g_HanselHP <= 30)
                        //스턴상태가 아니며 + Hansel HP 가 30 아래면
                    {
                        curState = g_CurrentState.HanselGuard;
                    }
                    break;

                case g_CurrentState.KnifeAttack:
                    m_Text.text = "KnifeAttack";
                    StartCoroutine(_KnifeAttack());

                    break;

                case g_CurrentState.FoodAttack:
                    m_Text.text = "FoodAttack";
                    StopCoroutine(_KnifeAttack());

                    StartCoroutine(_FoodAttack());

                    break;

                case g_CurrentState.Stuned:
                    m_Text.text = "Stuned";
                    Debug.Log("_StunByPlayer");
                    StartCoroutine(_StunByPlayer());

                    break;

                case g_CurrentState.HanselGuard:
                    m_Text.text = "HanselGuard";
                    //애니메이션 실행
                    m_Protect.SetActive(true);
                    break;

            }

            yield return null;
        }
    }


    IEnumerator _KnifeAttack()
    {
        //배기
        //Debug.Log("KnifeAttack");
        m_CountKnife = 0;
        m_KnifeAttack.SetActive(true);
        while (m_CountKnife <= 1)
        {
            if(_isDamaged == true)
            {
                curState = g_CurrentState.Stuned;
                StopCoroutine(_KnifeAttack());    
            }

            if(m_CountKnife >= 1)
            {
                yield return new WaitForSeconds(1.0f);
                _isFoodable = true;

            }
            m_CountKnife++;
        }


        _isDamaged = false;
        yield break;
    }

    IEnumerator _FoodAttack()
    {
        //음식 부리기 (Hansel 버프 아이템)
        //Debug.Log("FoodAttack");
        yield return new WaitForSeconds(2.0f);
        m_FoodAttack.SetActive(true);
        _isFoodable = false;

        curState = g_CurrentState.idle;
    }


    IEnumerator _StunByPlayer()
    {
        _isAttackable = true;
        curState = g_CurrentState.idle;

        yield return new WaitForSeconds(5.0f);
        _isAttackable = false;
    }



    private void Gretel_is_Dead()
    {
        //Gretel Dead 상태 체크
        if (m_GretelHP <= 0)
        {
            _isDead = true;
            Debug.Log("Gretel is DEAD");
            //Dead 애니메이션
        }
    }

    public void OnSkillDamage(float SkillDamage)
    {

    }

    public void OnDamage(int Playerdamage, Vector3 pos)
    {
        if (_isAttackable == true)
        {
            m_GretelHP -= Playerdamage;
            Gretel_is_Dead();

        }
        _isDamaged = true;
    }

    public void OnRecovery(int heal)
    {
       
    }
}
