using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleState : EnemyState
{
    private Slime enemy;
    private Transform player;
    private int moveDir;
    public SlimeBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Slime _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
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

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.atkDistance)
            {
                if (canAtk())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            if (enemy.battleTime < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;
        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool canAtk()
    {
        if (Time.time > enemy.lasttimeAtk + enemy.atkCD)
        {
            enemy.atkCD = Random.Range(enemy.minAtkCD, enemy.maxAtkCD);

            enemy.lasttimeAtk = Time.time;
            return true;
        }
        return false;
    }

}
