using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterFSM;

namespace MonsterFSM.RollRollFSM
{
    public class RollRoll_Patrol : NonAttackFSMBase
    {
        private bool isHit = false;
        private Vector3 Jumpforce;
        private bool isJump = false;

        private RollRoll RollRollMon;


        public void init(MonsterBase MonsterController)
        {
            Monster = MonsterController;
            RollRollMon = Monster as RollRoll;
            isJump = false;
            RollRollMon.gIsChase = false;
            RollRollMon.gAnimator.SetTrigger("Walk");

        }



        public override void FixedExecute(Rigidbody rigid)
        {
            AngleTurn();
            Move(rigid , Monster.transform.forward , Monster.gWalkSpeed);
            Jumpforce = JumpForce(Monster.gWalkSpeed);
            if (Jumpforce != Vector3.zero)
                isJump = true;
        }


        public override MonsterFSMBase Transition()
        {
            if (isHit)
            {                
                return RollRollFSMCreator.CreateRunning(Monster);
            }
            else if (isJump)
            {                
                return RollRollFSMCreator.CreateJump(Monster, Jumpforce);
            }               

            return this;
        }

        public override void UpdateExecute()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {                
                isHit = true;
            }
        }
    }

}
