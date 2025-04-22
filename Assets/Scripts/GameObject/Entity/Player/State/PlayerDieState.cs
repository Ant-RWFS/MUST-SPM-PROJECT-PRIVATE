using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.LockRB();
    }

    public override void Exit()
    {
        base.Exit();
        player.UnlockRB();
        player.stats.InitCurrentHealthValue();
    }

    public override void Update()
    {
        base.Update();

        if(!player.anim.GetBool("Die"))
            PlayerRespawn();
    }

    private void PlayerRespawn() 
    {
        player.transform.position = new Vector3(10, 10, 0);//respawn position
        player.stateMachine.ChangeState(player.idleState);
    }
}
