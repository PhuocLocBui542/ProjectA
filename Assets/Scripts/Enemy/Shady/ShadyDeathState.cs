using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadyDeathState : EnemyState
{
    private Shady enemy;
    public ShadyDeathState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Shady enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
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
        if (triggerCalled)
            enemy.SelfDestroy();
    }
}
