using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public float distanceToPlayer;

    #region State
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }
    public SkeletonDeathState deathState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(stateMachine, this, "Idle",this);
        moveState = new SkeletonMoveState(stateMachine, this, "Move",this);
        battleState = new SkeletonBattleState(stateMachine, this, "Battle", this);
        attackState = new SkeletonAttackState(stateMachine, this, "Attack", this);

        stunnedState = new SkeletonStunnedState(stateMachine, this, "Stunned", this);
        deathState = new SkeletonDeathState(stateMachine, this, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        distanceToPlayer = IsPlayerDetected().distance;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }
}
