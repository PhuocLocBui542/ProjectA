using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMoveState : ArcherGroundState
{
    public ArcherMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Archer _enemy) : base(_stateMachine, _enemyBase, _animBoolName, _enemy)
    {
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
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);
        if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
        {
            enemy.Flip();
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }
}
