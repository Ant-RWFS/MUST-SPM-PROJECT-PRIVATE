using UnityEngine;

public class Motor : Entity<MotorStats>
{
    #region Componets
    public Player player;
    public Transform detector;
    public float detectRadius;
    #endregion
    #region States
    public MotorStateMachine stateMachine { get; private set; }
    public MotorOffState offState { get; private set; }
    public MotorIdleState idleState { get; private set; }
    public MotorMoveState moveState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new MotorStateMachine();
        offState = new MotorOffState(player, this, stateMachine, "Off");
        idleState = new MotorIdleState(player, this, stateMachine, "Idle");
        moveState = new MotorMoveState(player, this, stateMachine, "Move");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(offState);
    }

    protected override void Update()
    {
        base.Update();
        HandleVisualRange();
        stateMachine.currentState.Update();
    }

    private void HandleVisualRange()
    {
        if (Mathf.Abs(Vector3.Distance(transform.position, PlayerManager.instance.playerTransform.position)) >= MapGenerator.instance.radius - 2.5f)
            MotorOutRange();
        else
            MotorInRange();
    }

    private void MotorOutRange() => sr.enabled = false;
    private void MotorInRange() => sr.enabled = true;
}
