public class SpecialAttackState : IState {

    public override void OnStateEnter(PlayerController player)  {
        player.invinBool = true;
        player.attackWeapon[0].gameObject.GetComponent<Weapon>().isCutting = true;
        player.attackWeapon[0].SetDamage(player.Special_Damage);
        player.ani.SetTrigger("SAttack");
    }

    public override void OnStateExcute(PlayerController player) {

        if(player.ani.GetCurrentAnimatorStateInfo(0).IsName("Serros_SAttack") &&
        player.ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= .95f)   {
            player.Walk();
            player.ChangeState(PlayerState.WalkState);
        }
    }

    public override void OnStateExit(PlayerController player)   {
        player.invinBool = false;
        player.attackWeapon[0].gameObject.GetComponent<Weapon>().isCutting = false;
        player.attackWeapon[0].SetDamage(player.isDamage);
        player.ui.SP_Using = false;
        player.AttackBoxOff(0);
    }
}