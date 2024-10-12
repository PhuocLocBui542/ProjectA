using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float CD;
    public float CDTimer;

    protected Player player;

    protected virtual void Update()
    {
        CDTimer -= Time.deltaTime;
    }

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlock();
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual bool CanUseSkill()
    {
        if (CDTimer < 0)
        {
            UseSkill();
            CDTimer = CD;
            return true;
        }

        player.fx.CreatePopUpText("Cooldown");
        return false;
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Transform FindClosestEnemy(Transform _checkTranform)
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(_checkTranform.position, 25);

        float cloneDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in collider2D)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTranform.position, hit.transform.position);

                if (distanceToEnemy < cloneDistance)
                {
                    cloneDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
