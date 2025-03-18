using UnityEngine;

public class MotorMoveState : MotorOnState
{
    public MotorMoveState(Player _player, Motor _motor, MotorStateMachine _motorStateMachine, string _animBoolName) : base(_player, _motor, _motorStateMachine, _animBoolName)
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

        motor.SetMoveVelocity(input.x, input.y);

        if (input == Vector2.zero)
            stateMachine.ChangeState(motor.idleState);
    }
}
