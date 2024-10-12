using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Shady : Enemy
{
    [Header("Shady spisifc info")]
    public float battleSpeed;

    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;

    #region States
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new ShadyIdleState(stateMachine, this, "Idle", this);
        moveState = new ShadyMoveState(stateMachine, this, "Move", this);
        battleState = new ShadyBattleState(stateMachine, this, "MoveFast", this);

        stunnedState = new ShadyStunnedState(stateMachine, this, "Stunned", this);
        deathState = new ShadyDeathState(stateMachine, this, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }   
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

    public override void AnamtionSpecialAttackTrigger()
    {
        GameObject newExplosive = Instantiate(explosivePrefab, atkCheck.position, Quaternion.identity);
        newExplosive.GetComponent<ExplosionController>().SetupExplosive(stats,growSpeed,maxSize,atkRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
    }

    public void SelfDestroy() => Destroy(gameObject);
}
