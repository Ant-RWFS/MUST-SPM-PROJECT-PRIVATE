using UnityEngine;

public class MotorOnState : MotorState
{
    protected float xInput;
    protected float yInput;

    protected float xSpeed;
    protected float ySpeed;

    protected Vector2 input;

    protected float rockTime = 0.05f;
    protected float rockTimer = 0;
    protected bool rockUp = false;
    public MotorOnState(Player _player, Motor _motor, MotorStateMachine _motorStateMachine, string _animBoolName) : base(_player, _motor, _motorStateMachine, _animBoolName)
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
        PlayerOffDetect();
        MotorEngineLogic();
        MotorRockEffect();
    }

    private void PlayerOffDetect() 
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.stateMachine.ChangeState(player.idleState);
            motor.stateMachine.ChangeState(motor.offState);
        }
    }

    private void MotorEngineLogic()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        input = new Vector2(xInput, yInput).normalized;

        if (input != Vector2.zero) 
        {
            motor.anim.SetFloat("xInput", xInput);
            motor.anim.SetFloat("yInput", yInput); 
        }
    }

    private void MotorRockEffect() 
    {
        rockTimer -= Time.deltaTime;

        if (rockTimer <= 0)
        {
            if (rockUp)
                motor.anim.transform.position += new Vector3(0, 0, -.02f);
            else
                motor.anim.transform.position -= new Vector3(0, 0, -.02f);

            rockUp = !rockUp;
            rockTimer = rockTime;
        }
    }
}
