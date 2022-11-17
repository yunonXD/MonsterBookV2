using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Target;
    public int Type;

    void Start()
    {
        gameObject.transform.position = Target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Type == 1)
        {
            gameObject.transform.position = new Vector3(Target.transform.position.x, 10, Target.transform.position.z);  //�׷��� ����ִϸ��̼� Follow
        }
        else if(Type == 2)
        {
            gameObject.transform.position = new Vector3(Target.transform.position.x-6, 0, Target.transform.position.z); //�׷��� Į���� Follow
        }
        else if(Type == 0)
        {
            gameObject.transform.position = new Vector3(Target.transform.position.x, 0, Target.transform.position.z);   //�׷��ڰ� �÷��̾� �Ÿ� ������(������ ���� �Ÿ� ��ȯ ������)
        }
    }
}
