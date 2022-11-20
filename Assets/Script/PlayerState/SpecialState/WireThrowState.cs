using UnityEngine;

public class WireThrowState : IState    {

    public override void OnStateEnter(PlayerController player)  {     
        player.line.enabled = true;
        if (player.CheckDamage) {
            player._CheckDamage();
            return;
        }
  
        player.line.GetComponent<Renderer>().material = player.WireMaterial[0];
        player.SaveMonDetect = player.isMonsterCheck;
        player.LockLookTartget = true;
        player.ani.SetTrigger("WireThrow");
        player.ParticlePlay("WireThrow");
    }

    public override void OnStateExcute(PlayerController player) {
        player.line.SetPosition(0, player.wireStart.position);
        player.line.SetPosition(1, player.wirePos.position);

        if(player.ani.GetCurrentAnimatorStateInfo(0).IsName("WireThrow") &&
        player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95f)   {
            if (player.CheckDamage) player._CheckDamage();
            else    {
                player.SoundShot("Player_Wire_Attach");
                player.ChangeState(PlayerState.WireState);
            }
        }

#region HitInter
    IWireEffect IWE = GetComponent<IWireEffect>();
    if(IWE != null && !player.CheckDamage){
        IWE.Hit(true);
    }
#endregion
    }

    public override void OnStateExit(PlayerController player)   {
        if(!player.CheckDamage) {
            player.invinBool = true;
            player.line.GetComponent<Renderer>().material = null;
            player.line.GetComponent<Renderer>().material = player.WireMaterial[1];
        }
        else{
            return;
        }          
    }
}
