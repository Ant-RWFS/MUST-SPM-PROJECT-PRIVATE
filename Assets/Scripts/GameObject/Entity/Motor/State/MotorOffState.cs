using UnityEngine;

public class MotorOffState : MotorState
{
    public MotorOffState(Player _player, Motor _motor, MotorStateMachine _motorStateMachine, string _animBoolName) : base(_player, _motor, _motorStateMachine, _animBoolName)
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

        MotorShutDown();
        PlayerOnDetect();
    }

    private void PlayerOnDetect()
    {
        Collider2D[] detector = Physics2D.OverlapCircleAll(motor.detector.transform.position, motor.detectRadius);

        foreach (Collider2D hit in detector)
        {
            if (hit.GetComponent<Player>())
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    player.stateMachine.ChangeState(player.rideState);
                    motor.stateMachine.ChangeState(motor.idleState);
                }
            }
        }
    }
    private void MotorShutDown() => motor.SetZeroVelocity();
}
