using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeBoi : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    [SerializeField] private int m_Resolution = 2  , m_WaveCount = 10 , m_WobbleCount = 5;
    [SerializeField] private float m_WaveSize = 3 , m_AnimeSpeed = 4;

    private LineRenderer m_Line;

    private  void Awake(){
        m_Line = GetComponent<LineRenderer>();
    }


    void Update(){
        StartCoroutine(AnimationRope(m_Target.position));
    }


    private IEnumerator AnimationRope(Vector3 m_targetPos){
        m_Line.positionCount = m_Resolution;
        float angle = LookAtAngle(m_targetPos - transform.position);

        float m_percent = .0f;
        while(m_percent <= 1f)
        {
            m_percent += Time.deltaTime * m_AnimeSpeed;
            SetPoints(m_targetPos , m_percent , angle);
            yield return null;
        }
        SetPoints(m_targetPos , 1 , angle);
    }


    private void SetPoints(Vector3 targetPos , float percent , float angle){
        Vector3 ropeEnd = Vector3.Lerp(transform.position , targetPos, percent);
        float length = Vector2.Distance(transform.position , ropeEnd);

        for(int i = 0; i < m_Resolution; i++)
        {
            float xPos = (float) i / m_Resolution * length;
            float reversePercent = (1 - percent );

            float amplitude = Mathf.Sin(reversePercent * m_WobbleCount * Mathf.PI) * ((1f - (float)i / m_Resolution) * m_WaveSize);

            float yPos = Mathf.Sin((float)m_WaveCount * i / m_Resolution * 2 * Mathf.PI * reversePercent) * amplitude;

            Vector2 pos = RotatePoints( new Vector2(xPos + transform.position.x , yPos + transform.position.y), transform.position, angle);
            m_Line.SetPosition(i , pos);
        }
    }

    Vector2 RotatePoints(Vector2 point , Vector2 pivot , float angle){
        Vector2 dir = point - pivot;
        dir = Quaternion.Euler(0,0,angle) * dir;
        point = dir + pivot;
        return point;
    }

    private float LookAtAngle(Vector2 target){
        return Mathf.Atan2(target.y , target.x) * Mathf.Rad2Deg;
    }


}
