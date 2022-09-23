using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterFSM.RollRollFSM
{
    public static class RollRollFSMCreator
    {     
        public static MonsterFSMBase CreateRunning(MonsterBase Monster)
        {


            var Component = Monster.gameObject.GetComponent<RollRoll_Running>();
            if (Component == null)
                Component = Monster.gameObject.AddComponent<RollRoll_Running>();
            Component.init(Monster);

            return Component;
        }

        public static MonsterFSMBase CreateJump(MonsterBase Monster, Vector3 Force)
        {

            if (Monster.gStopJump || Monster.CapsuleCastCheck())
                return null;

            var Component = Monster.gameObject.GetComponent<RollRoll_Jump>();
            if (Component == null)
                Component = Monster.gameObject.AddComponent<RollRoll_Jump>();
            Component.init(Monster, Force);
            return Component;
        }

        public static MonsterFSMBase CreatePatrol(MonsterBase Monster)
        {
            var Component = Monster.gameObject.GetComponent<RollRoll_Patrol>();
            if (Component == null)
                Component = Monster.gameObject.AddComponent<RollRoll_Patrol>();
            Component.init(Monster);
            return Component;

        }
    }
}


