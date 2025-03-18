using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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

        player.SetRunVelocity(xInput, yInput);

        if (Input.GetKeyUp(KeyCode.LeftShift))
            stateMachine.ChangeState(player.moveState);

        if ((xInput == 0 && yInput == 0))
            stateMachine.ChangeState(player.idleState);
    }
}
