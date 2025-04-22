using System;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player;
    [SerializeField]Animator animator;
    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            animator.enabled = false;
        else
        {
            animator.enabled = true;
        }
    }

    private void EnterInvisibleMode() => player.stats.isInvisible = true;
    private void ExitInvisibleMode() => player.stats.isInvisible = false;
    private void DeathExitTrigger() => player.anim.SetBool("Die", false);
    private void RollingExitTrigger() => player.anim.SetBool("Roll", false);
    private void SlashingExitTrigger() => player.anim.SetBool("Attack", false);
    private void AttackDamageTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.coll.transform.position, player.coll.radius);
        {
            foreach (var hit in colliders)
            {
                if (hit.GetComponent<EnemyInterface>() != null)
                {
                    var enemy = hit.GetComponent<EnemyInterface>();
                    if (!enemy.statsI.isInvisible)
                    {
                        enemy.Damage(player.stats.damage.GetValue());                        
                        enemy.SetKnockBack(player.stats.currentVector.x * player.stats.knockVector.x, player.stats.currentVector.y * player.stats.knockVector.y);
                    }
                }

                if (hit.GetComponentInParent<Resource>())
                { 
                    var resource = hit.GetComponentInParent<Resource>();
                    if (!resource.stats.isInvisible) 
                        resource.Damage(player.stats.damage.GetValue());
                }
            }
        }
    }
}
