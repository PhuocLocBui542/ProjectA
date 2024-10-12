using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyIdleState : ShadyGroundState
{
    public ShadyIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Shady _enemy) : base(_stateMachine, _enemyBase, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        enemy.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySFX(24, enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
