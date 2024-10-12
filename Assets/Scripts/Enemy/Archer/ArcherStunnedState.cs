using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStunnedState : EnemyState
{
    private Archer enemy;
    public ArcherStunnedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Archer _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
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
