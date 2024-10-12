using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private Player player;
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExitTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatisEnemy;
    public void SetupCrystal(float _crystalDur, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        player = _player;
        crystalExitTimer = _crystalDur;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;

        /*if (closestTarget == null)
        {

            closestTarget = transform;
            canMove = false;
        }
        else
        {
            closestTarget = _closestTarget;
        }*/
    }

    public void RandomEnemyChossen()
    {
        float radius = SkillManager.instance.blackhole.BlackholeRadius();

        Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, radius, whatisEnemy);
        if (collider2D.Length > 0)
            closestTarget = collider2D[Random.Range(0, collider2D.Length)].transform;
    }

    private void Update()
    {
        crystalExitTimer -= Time.deltaTime;

        if (crystalExitTimer < 0)
            FinishCrystal();

        if (canMove)
        {
            if (closestTarget == null)
                return;

            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in collider2D)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                player.stats.DoMagicDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equipedAmulet != null)
                    equipedAmulet.Effect(hit.transform);
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
