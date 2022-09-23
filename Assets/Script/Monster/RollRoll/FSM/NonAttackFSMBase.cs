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

        // 해당 FSM을 소유작 Collider충돌 함수를 쓰기 위해서 Action 함수를 통해서 처리를 해주게 됩니다.
        // ( new List<Aaction<T>()>() )초기화는 생성자에서 해줘야 한다.
        public List<Action<Collider>> TriggerFunction;
        public List<Action<Collision>> CollisionFunction;




    }
}

