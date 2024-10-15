using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpell : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;

    private CharacterStats Stats;

    public void SetupSpell(CharacterStats _stats) => Stats = _stats;
    private void AnimationTrigger()
    {
        Collider2D[] collider2D = Physics2D.OverlapBoxAll(check.position, boxSize, whatIsPlayer);
        foreach (var hit in collider2D)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                Stats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize);

    private void SelfDestroy() => Destroy(gameObject);
}
