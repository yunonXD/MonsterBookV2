public class PatrolState : IState   {

    private int m_CountPointer = 0;

    public override void OnStateEnter(PlayerController player)  {
        player.input.SetInputAction(false);
        player.invinBool = true;
         m_CountPointer = 0;
    }     

    public override void OnStateExcute(PlayerController player) {
        if (m_CountPointer != player.playerPatrolLocaiton.Length)   {
                #region Movement to m_CountPointer
                player.transform.position = UnityEngine.Vector3.MoveTowards(
                player.transform.position, player.playerPatrolLocaiton[m_CountPointer].transform.position,
                 player.walkSpeed * UnityEngine.Time.fixedDeltaTime);
                 player.ani.SetFloat("WalkSpeed", 1.0f);
                #endregion

                #region LookRotation

                var m_lookatVec = (new UnityEngine.Vector3(player.playerPatrolLocaiton[m_CountPointer].transform.position.x, 0, 0)).normalized;

                player.transform.LookAt(m_lookatVec);

                #endregion

                if (player.transform.position == player.playerPatrolLocaiton[m_CountPointer].transform.position)
                    m_CountPointer++;
                else    return;
        }
        else    {
            player.ani.SetFloat("WalkSpeed", 0.0f);
            player.ChangeState(PlayerState.IdleState);
        }
    }

    public override void OnStateExit(PlayerController player)   {
        player.input.SetInputAction(true);
        player.invinBool = false;
    }
}
