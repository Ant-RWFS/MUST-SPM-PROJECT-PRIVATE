using UnityEngine;

public class PlayerRideState : PlayerState
{
    public PlayerRideState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.stats.isBusy = true;
    }

    public override void Exit()
    {
        base.Exit();

        player.stats.isBusy = false;
        player.anim.runtimeAnimatorController = player.acList[0];
        player.transform.position = MotorManager.instance.motorTransform.position;
    }

    public override void Update()
    {
        base.Update();
        player.transform.position = MotorManager.instance.motorTransform.position + new Vector3(0, 0, -.1f);
    }
}
