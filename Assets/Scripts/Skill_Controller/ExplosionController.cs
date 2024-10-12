using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ExplosionController : MonoBehaviour
{
    private Animator anim;
    private CharacterStats myStats;
    private float growSpeed = 15;
    private float maxSize = 6;
    private float explosionRadius;

    private bool canGrow = true;

    private void Update()
    {
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (maxSize - transform.localScale.x < 5f)
        {
            canGrow = false;
            anim.SetTrigger("Explode");
        }
    }


    public void SetupExplosive(CharacterStats _myStats, float _growSpeed, float _maxSize, float _explosionRadius)
    {
        anim = GetComponent<Animator>();

        _myStats = myStats;
        _growSpeed = growSpeed;
        _maxSize = maxSize;
        _explosionRadius = explosionRadius;
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in collider2D)
        {
            if (hit.GetComponent<CharacterStats>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}
