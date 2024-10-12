using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyMoveState : ShadyGroundState
{
    public ShadyMoveState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Shady _enemy) : base(_stateMachine, _enemyBase, _animBoolName, _enemy)
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
