using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MonsterFSM
{
    public abstract class NonAttackFSMBase : MonsterFSMBase
    {
               

        public override abstract void FixedExecute(Rigidbody rigid);
        public override abstract MonsterFSMBase Transition();
        public override abstract void UpdateExecute();

        // �ش� FSM�� ������ Collider�浹 �Լ��� ���� ���ؼ� Action �Լ��� ���ؼ� ó���� ���ְ� �˴ϴ�.
        // ( new List<Aaction<T>()>() )�ʱ�ȭ�� �����ڿ��� ����� �Ѵ�.
        public List<Action<Collider>> TriggerFunction;
        public List<Action<Collision>> CollisionFunction;




    }
}

