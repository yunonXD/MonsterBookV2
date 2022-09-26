using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterFSM.RollRollFSM
{
    public class RollRoll_Running : NonAttackFSMBase
    {
        private Vector3 StartTurn;
        private Vector3 Jumpforce;

        private Vector3 ArrivalPos;
        private float RandomDistance;

        private bool isJump;

        private RollRoll RollRollMon;

        private float CurrentObstacle;

        
        public void init(MonsterBase MonsterController)
        {
            Monster = MonsterController;
            RollRollMon = Monster as RollRoll;
            ArrivalPos = Vector3.zero;
            StartTurn = MonsterController.transform.rotation.eulerAngles;
            isJump = false;
            RollRollMon.gIsChase = true;
            RollRollMon.gAnimator.SetTrigger("Run");
            CurrentObstacle = 0.0f;
        }




        public override void FixedExecute(Rigidbody rigid)
        {
            Move(rigid, Monster.transform.forward, Monster.gRunSpeed);
            Jumpforce = JumpForce(Monster.gWalkSpeed);
            if (Jumpforce != Vector3.zero)
            {                
                isJump = true;
            }
            else
            {
                AngleTurn();
            }
        }


        public override MonsterFSMBase Transition()
        {

            if (isJump)
            {   
                return RollRollFSMCreator.CreateJump(Monster, Jumpforce);
            }

            return this;
        }

        public override void UpdateExecute()
        {
            CreateObstacle();
        }

        protected override void AngleTurn()
        {
            if (RollRollMon.gChaseType == RollRoll.EChaseType.RandomChase)
            {
                if (ArrivalPos == Vector3.zero)
                {

                    var v = Monster.gPatrolPoint;
                    Monster.CurrentPatrol = Random.Range(0, v.Length - 1);

                    int idx = Monster.CurrentPatrol;
                    var D1 = Vector2.Distance(new Vector2 (v[idx].position.x, v[idx].position.z ), new Vector2(Monster.transform.position.x, Monster.transform.position.z));
                    var D2 = Vector2.Distance(new Vector2(v[idx + 1].position.x, v[idx + 1].position.z), new Vector2(Monster.transform.position.x, Monster.transform.position.z));

                    if (D1 >= D2)
                    {
                        RandomDistance = D1;
                        ArrivalPos = v[idx].position;
                    }

                    else
                    {
                        RandomDistance = D2;
                        ArrivalPos = v[idx + 1].position;
                    }

                    var rand = Random.Range(1, RandomDistance);
                    var Dir = (Monster.transform.position - ArrivalPos).normalized;

                    ArrivalPos = ArrivalPos + Dir * rand;


                }

                Monster.transform.LookAt(ArrivalPos);
                var rot = Monster.transform.rotation;
                Monster.transform.rotation = Quaternion.Euler(0.0f, rot.eulerAngles.y, 0.0f);





                if (XZDistanceCheck(Monster.transform.position, ArrivalPos))
                {
                    Debug.Log("ÀÐÈû");
                    ArrivalPos = Vector3.zero;
                }

            }
            else if (RollRollMon.gChaseType == RollRoll.EChaseType.OppersiteDirection)
            {
                if (Monster.CapsuleCastCheck())
                    StartTurn.y += 180.0f;
                Monster.transform.rotation = Quaternion.Euler(0.0f, StartTurn.y + 180.0f, 0.0f);
            }
            else if (RollRollMon.gChaseType == RollRoll.EChaseType.NormalChase)
            {
                base.AngleTurn();
            }
        }

        private void CreateObstacle()
        {
            CurrentObstacle += Time.deltaTime;
            if (CurrentObstacle >= RollRollMon.gCreateObstacleTime)
            {
                var rigidbody = Instantiate(RollRollMon.gObstacleObj, RollRollMon.transform.position + Vector3.up,Quaternion.identity).GetComponent<Rigidbody>();
                var Force = Vector3.Scale(transform.forward, new Vector3(-1.0f ,0.0f ,0.0f)) + Vector3.up * 5;
                rigidbody.AddForce(Force.normalized * 7,ForceMode.Impulse);
                CurrentObstacle = 0.0f;
            }
        }
    }
}

