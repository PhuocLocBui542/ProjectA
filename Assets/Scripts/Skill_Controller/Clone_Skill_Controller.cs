using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;

    private float cloneTimer;
    private float atkMultiplier;
    [SerializeField] private Transform atkCheck;
    [SerializeField] private float atkRadius = .8f;
    private Transform closeEnemy;
    private int facingDir = -1;

    private float changetoDuplicate;
    private bool canDuplicate;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));
            if (sr.color.a < 0)
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform _newTranform, float cloneDur, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _changetoDuplicate, Player _player, float _atkMultiplier)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }

        atkMultiplier = _atkMultiplier;
        player = _player;
        transform.position = _newTranform.position + _offset;
        cloneTimer = cloneDur;
        closeEnemy = _closestEnemy;
        canDuplicate = _canDuplicate;
        changetoDuplicate = _changetoDuplicate;
        FaceCloneTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(atkCheck.position, atkRadius);
        foreach (var hit in collider2D)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDmg(enemyStats, atkMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }

                if (canDuplicate)
                {
                    if (Random.Range(0, 100) < changetoDuplicate)
                    {
                        SkillManager.instance.clone.createClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceCloneTarget()
    {
        if (closeEnemy != null)
        {
            if (transform.position.x > closeEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
