using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private DeathBringer enemy;
    private Transform player;
    public DeathBringerIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, DeathBringer _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
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

        if (Vector2.Distance(player.transform.position, enemy.transform.position) < 7)
            enemy.bossFightBegin = true;

        if (Input.GetKeyDown(KeyCode.M))
            stateMachine.ChangeState(enemy.teleportState);

        if (stateTimer < 0 && enemy.bossFightBegin)
            stateMachine.ChangeState(enemy.battleState);
    }
}
