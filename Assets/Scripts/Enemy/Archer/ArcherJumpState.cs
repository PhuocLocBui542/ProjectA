using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
    private Archer enemy;
    public ArcherJumpState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Archer _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir, enemy.jumpVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.y < 0 && enemy.IsGroundDetected())
            stateMachine.ChangeState(enemy.battleState);
    }
}
