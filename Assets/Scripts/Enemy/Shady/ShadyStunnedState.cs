using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyStunnedState : EnemyState
{
    protected Shady enemy;

    public ShadyStunnedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Shady enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
