using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    private DeathBringer enemy;

    public DeathBringerAttackState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, DeathBringer _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.chanceToTeleport += 5;
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
            {
                if (enemy.CanTeleport())
                    stateMachine.ChangeState(enemy.teleportState);
                else 
                    stateMachine.ChangeState(enemy.battleState);
            }
        }
    }
}
