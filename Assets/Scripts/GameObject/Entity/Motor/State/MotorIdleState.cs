using UnityEngine;

public class MotorIdleState : MotorOnState
{
    public MotorIdleState(Player _player, Motor _motor, MotorStateMachine _motorStateMachine, string _animBoolName) : base(_player, _motor, _motorStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        motor.LockRB();
    }

    public override void Exit()
    {
        base.Exit();
        motor.UnlockRB();
    }

    public override void Update()
    {
        base.Update();

        if (input != Vector2.zero)
            stateMachine.ChangeState(motor.moveState);
    }
}
