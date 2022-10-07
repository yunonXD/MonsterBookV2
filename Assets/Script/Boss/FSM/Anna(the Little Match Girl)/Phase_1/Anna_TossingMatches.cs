
namespace UnityEngine
{
    public class Anna_TossingMatches : FSM_State<Anna>
    {
        static readonly Anna_TossingMatches instance = new Anna_TossingMatches();

        public static Anna_TossingMatches Instance
        {
            get { return instance; }
        }

        private UnityEngine.GameObject MatchesYeet; //�ӽ�
        private Vector3 m_MatchesDestination;

        static Anna_TossingMatches() { }
        private Anna_TossingMatches() { }

        public override void EnterState(Anna _Anna)
        {
            //Ÿ�� Ȯ��(�÷��̾�)
            if (_Anna.Anna_Player == null)
            {
                return;
            }

            //�÷��̾� ���� (�ش� �������� ���� ���� �̻��� �� ) 
            m_MatchesDestination = new UnityEngine.Vector3(_Anna.Anna_Player.transform.position.x, _Anna.Anna_Player.transform.position.y, 0);


        }


        public override void UpdateState(Anna _Anna)
        {




        }


        public override void ExitState(Anna _Anna)
        {

        }

    }
}

