using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Freeze effect", menuName = "Data/Item effect/Freeze enemy")]

public class FreezeEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _tranform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
            return; 

        if (!Inventory.instance.CanUseArmor())
            return;

        Collider2D[] collider2D = Physics2D.OverlapCircleAll(_tranform.position, 2);
        foreach (var hit in collider2D)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
            
        }
    }
}
