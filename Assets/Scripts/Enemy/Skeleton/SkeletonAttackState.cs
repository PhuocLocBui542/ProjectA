using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Skeleton enemy;
    public SkeletonAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Skeleton _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lasttimeAtk = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsGroundDetected())
        {
            enemy.ZeroVelocity();
            if (triggerCalled)
                stateMachine.ChangeState(enemy.battleState);
        }
    }
}
