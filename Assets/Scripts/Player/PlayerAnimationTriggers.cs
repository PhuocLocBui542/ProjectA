using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigge();
    }

    private void AttackTrigger()
    {

        AudioManager.instance.PlaySFX(2, null);

        Collider2D[] collider2D = Physics2D.OverlapCircleAll(player.atkCheck.position, player.atkRadius);
        foreach (var hit in collider2D)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                    player.stats.DoDamage(_target);

                ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                    weaponData.Effect(_target.transform);
            }
        }
    }

    private void WeaponEffect()
    {
    }
}
