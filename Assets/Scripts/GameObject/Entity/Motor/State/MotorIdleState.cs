using UnityEngine;

public class MotorIdleState : MotorOnState
{
    public MotorIdleState(Player _player, Motor _motor, MotorStateMachine _motorStateMachine, string _animBoolName) : base(_player, _motor, _motorStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MotorLockRB();
    }

    public override void Exit()
    {
        base.Exit();
        MotorUnlockRB();
    }

    public override void Update()
    {
        base.Update();

        if (input != Vector2.zero)
            stateMachine.ChangeState(motor.moveState);
    }
}
