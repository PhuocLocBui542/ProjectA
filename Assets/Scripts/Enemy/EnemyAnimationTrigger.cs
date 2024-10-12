using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();
    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }

    private void SpecialAttackTrigger()
    {
        enemy.AnamtionSpecialAttackTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] collider2d = Physics2D.OverlapCircleAll(enemy.atkCheck.position, enemy.atkDistance);
        foreach (var hit  in collider2d)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(_target);
            }
        }
    }
}
