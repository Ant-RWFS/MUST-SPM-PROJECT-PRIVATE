using UnityEditor.Animations;
using UnityEngine;

public class Player : Entity<PlayerStats>
{
    #region Components
    public AnimatorController[] acList;
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerRollState rollState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerRunState runState { get; private set; }
    public PlayerRideState rideState { get; private set; }
    public PlayerSlashState slashState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        rollState = new PlayerRollState(this, stateMachine, "Roll");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        runState = new PlayerRunState(this, stateMachine, "Run");
        rideState = new PlayerRideState(this, stateMachine, "Ride");
        slashState = new PlayerSlashState(this, stateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}