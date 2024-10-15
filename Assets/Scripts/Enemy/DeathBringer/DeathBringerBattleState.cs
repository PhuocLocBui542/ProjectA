using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private DeathBringer enemy;
    private Transform player;
    private int moveDir;
    public DeathBringerBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, DeathBringer _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
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

        //if (player.GetComponent<PlayerStats>().isDead)
        //    stateMachine.ChangeState(enemy.moveState);
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
                else
                    stateMachine.ChangeState(enemy.idleState);
            }
        }
       /* else
        {
            if (enemy.battleTime < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 100)
                stateMachine.ChangeState(enemy.idleState);
        }*/

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.atkDistance - .1f)
            return;

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
