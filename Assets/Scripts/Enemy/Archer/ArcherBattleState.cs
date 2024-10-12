using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private Archer enemy;
    private Transform player;
    private int moveDir;
    public ArcherBattleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Archer _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
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

            if (enemy.IsPlayerDetected().distance < enemy.safeDistance)
            {
                if (CanJump())
                    stateMachine.ChangeState(enemy.jumpState);
            }

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

        BattleStateFlipController();
    }

    private void BattleStateFlipController()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            enemy.Flip();
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

    private bool CanJump()
    {
        if (enemy.GroundBehind() == false || enemy.WallBehind() == true) 
            return false;

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCd)
        {

            enemy.lastTimeJumped = Time.time;
            return true;
        }
        return false;
    }
}
