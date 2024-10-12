using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastTimeAttack;
    private float comboReset = 1;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        /*AudioManager.instance.PlaySFX(2);*/

        if (comboCounter > 2 || Time.time >= lastTimeAttack + comboReset)
            comboCounter = 0;
        player.anim.SetInteger("ComboCounter", comboCounter);
        player.SetVelocity(player.moveAtk[comboCounter].x * player.facingDir, player.moveAtk[comboCounter].y);

    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine(nameof(player.BusyFor), .15f);
        comboCounter++;
        lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.ZeroVelocity();
        if (triggerCalled == true)
            stateMachine.ChangeState(player.idleState);
    }
}
