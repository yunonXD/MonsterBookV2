using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterFSM
{



    public abstract class MonsterFSMBase : MonoBehaviour, MonsterFSMInterface
    {
        public abstract void FixedExecute(Rigidbody rigid);
        public abstract MonsterFSMBase Transition();
        public abstract void UpdateExecute();

        protected MonsterBase Monster;

        protected bool ObstacleCheck(Transform trans, Vector3 Point, float MaxHeigh, out RaycastHit HitinfoPoint)
        {
            if (Physics.Raycast(Point + Vector3.up * MaxHeigh, Vector3.down, out HitinfoPoint))
            {
                if (HitinfoPoint.point.y < 0.1f || trans == HitinfoPoint.transform)
                {
                    return false;
                }

                return true;
            }

            return false;
        }



        protected virtual Vector3 JumpForce(float MaxForward)
        {
            RaycastHit hitinfo;
            Transform Trans = Monster.transform;
            float MaxHeigh = Monster.gMaxJumpForce;


            float Dist = 0;
            if (Physics.Raycast(Trans.position, Trans.forward, out hitinfo, MaxForward))
            {
                Dist = hitinfo.distance;
                if (hitinfo.normal.y != 0)
                {
                    return Vector3.zero;
                }


                if (ObstacleCheck(Trans, hitinfo.point, MaxHeigh, out hitinfo))
                {
                    if (Dist < MaxForward * 0.5f)
                    {
                        var Force = hitinfo.point - Trans.position;
                        Force.y += 1.7f;
                        return Force;
                    }
                }
            }
            return Vector3.zero;
        }


        protected virtual void AngleTurn()
        {
            var v = Monster.gPatrolPoint;

            Monster.CurrentPatrol = Monster.CurrentPatrol % v.Length;
            Monster.transform.LookAt(v[Monster.CurrentPatrol]);
            var rot = Monster.transform.rotation;
            Monster.transform.rotation = Quaternion.Euler(0.0f, rot.eulerAngles.y, 0.0f);



            if (XZDistanceCheck(Monster.transform.position, v[Monster.CurrentPatrol].position))
            {
                Monster.CurrentPatrol++;
            }
            if (Monster.CapsuleCastCheck() && Monster.gStopType == MonsterBase.EStopType.OppsiteDirInCaseOfCollision)
                Monster.CurrentPatrol++;
        }


        protected void Move(Rigidbody rigid, Vector3 Dir, float Speed)
        {
            if (Monster.gStopType != MonsterBase.EStopType.StopOnCrash && Monster.CapsuleCastCheck())
                return;

            var velocity = rigid.velocity;
            var forward = Vector3.Scale(Dir, (new Vector3(1.0f, 0.0f, 1.0f)));

            float velocityY = rigid.velocity.y;

            velocity = forward.normalized * Speed;
            velocity.y = velocityY;

            rigid.velocity = velocity;
        }

        protected bool XZDistanceCheck(Vector3 Pos1, Vector3 Pos2, float Dist = 0.5f)
        {
            var ArrivalXZ = new Vector2(Pos1.x, Pos1.z);
            var MonsterXZ = new Vector2(Pos2.x, Pos2.z);

            return Vector2.Distance(ArrivalXZ, MonsterXZ) < Dist;
        }

    }
}

