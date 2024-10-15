using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerDeathState : EnemyState
{
    private DeathBringer enemy;
    public DeathBringerDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, DeathBringer _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
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
