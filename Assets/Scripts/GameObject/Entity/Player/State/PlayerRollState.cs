using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerState
{
    public PlayerRollState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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

        player.SetRollVelocity();

        if(!player.anim.GetBool("Roll"))
            player.stateMachine.ChangeState(player.idleState);
    }
}