using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    private DeathBringer enemy;

    private int amountOfSpells;
    private float spellTimer;
    public DeathBringerSpellCastState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, DeathBringer _enemy) : base(_stateMachine, _enemyBase, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpells = enemy.amountOfSpells;
        spellTimer = .5f;
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (CanCast())
            enemy.CastSpell();
        
        if (amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);

    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
    }

    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCd;
            return true;
        }

        return false;
    }
}
