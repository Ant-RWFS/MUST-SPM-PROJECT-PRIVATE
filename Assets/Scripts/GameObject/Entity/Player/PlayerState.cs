using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    protected Vector2 input;
    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _playerStateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        InputUpdate();

        RollingLogic();
        SlashLogic();
    }

    private void InputUpdate()
    { 
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        input = new Vector2 (xInput, yInput).normalized;

        if (!player.stats.isArmed)
            PlayerDisarmedAnim();
        else
            PlayerArmedAnim();

        player.AdjustCurrentVector(input.x, input.y);
    }

    private void PlayerDisarmedAnim() 
    {
        if (player.anim.runtimeAnimatorController == player.acList[0])
            player.anim.SetFloat("Speed", 1);

        if (input != Vector2.zero)
        {
            player.anim.SetFloat("xInput", xInput);
            player.anim.SetFloat("yInput", yInput);
        }
    }

    private void PlayerArmedAnim() 
    {
        player.anim.runtimeAnimatorController = player.acList[0];

        player.anim.SetFloat("xInput", Input.mousePosition.x - Screen.width / 2);
        player.anim.SetFloat("yInput", Input.mousePosition.y - Screen.height / 2);

        if ((Input.mousePosition.x - Screen.width / 2) * xInput >= 0 && (Input.mousePosition.y - Screen.height / 2) * yInput >= 0)
            player.anim.SetFloat("Speed", 1);
        else
            player.anim.SetFloat("Speed", -1);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public virtual void RollingLogic() 
    {
        if (!player.stats.isBusy && Input.GetKeyDown(KeyCode.Space)) 
            player.stateMachine.ChangeState(player.rollState);
    }

    public virtual void SlashLogic()
    {
        if (!player.stats.isArmed)
        {
            if (!player.stats.isBusy && Input.GetKeyDown(KeyCode.Mouse0))
            {
                player.anim.runtimeAnimatorController = player.acList[1];
                player.stateMachine.ChangeState(player.slashState);
            }
        }
    }
}
