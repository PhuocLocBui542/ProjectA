using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundState : EnemyState
{
    protected Transform player;
    protected Slime enemy;

    public SlimeGroundState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Slime _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.transform.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
