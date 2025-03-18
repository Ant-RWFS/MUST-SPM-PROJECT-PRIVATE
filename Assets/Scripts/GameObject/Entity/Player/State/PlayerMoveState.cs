using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetMoveVelocity(xInput, yInput);

        if (xInput == 0 && yInput == 0)
            stateMachine.ChangeState(player.idleState);
        
        else if (Input.GetKey(KeyCode.LeftShift))
            stateMachine.ChangeState(player.runState);
    }
}
