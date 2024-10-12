using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunedState : EnemyState
{
    private Slime enemy;
    public SlimeStunedState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Slime _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);
        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
