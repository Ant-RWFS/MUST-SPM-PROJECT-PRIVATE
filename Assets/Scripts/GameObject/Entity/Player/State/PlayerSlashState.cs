using Unity.VisualScripting;
using UnityEngine;

public class PlayerSlashState : PlayerState
{
    public PlayerSlashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.stats.isBusy = true;
    }

    public override void Exit()
    {
        base.Exit();

        player.stats.isBusy = false;
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (!player.anim.GetBool("Attack"))
            player.stateMachine.ChangeState(player.idleState);

        AdjustAttackBox();
    }

    private void AdjustAttackBox()
    {
        Vector2 attackVector = player.stats.currentVector.normalized;
        player.coll.transform.position = new Vector3(player.transform.position.x + attackVector.x / 2, player.transform.position.y + attackVector.y / 2);
    }  
}
