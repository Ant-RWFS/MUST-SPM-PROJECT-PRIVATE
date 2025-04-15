using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }
    private void DeathExitTrigger() => player.anim.SetBool("Die", false);
    private void RollingExitTrigger() => player.anim.SetBool("Roll", false);
    private void SlashingExitTrigger() => player.anim.SetBool("Attack", false);
}
