using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorState
{
    protected MotorStateMachine stateMachine;
    protected Player player;
    protected Motor motor;
    private string animBoolName;

    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;
    public MotorState(Player _player,Motor _motor, MotorStateMachine _motorStateMachine, string _animBoolName)
    {
        this.player = _player;
        this.motor = _motor;
        this.stateMachine = _motorStateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        motor.anim.SetBool(animBoolName, true);
        rb = motor.rb;
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit()
    {
        motor.anim.SetBool(animBoolName, false);
    }
    public virtual void MotorLockRB() => motor.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    public virtual void MotorUnlockRB() => motor.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
}
