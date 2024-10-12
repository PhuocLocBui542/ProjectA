using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathState : EnemyState
{
    private Skeleton enemy;
    public SkeletonDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();
        enemy.Die();/*
        enemy.ZeroVelocity();*/
    }
}
