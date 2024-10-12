using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
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
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.X) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        if (Input.GetKeyDown(KeyCode.C))
            stateMachine.ChangeState(player.primaryAttack);

        if (Input.GetKeyDown(KeyCode.V) && player.skill.blackhole.blackholeUnlocked /*&& SkillManager.instance.blackhole.CanUseSkill()*/)
        {
            if (player.skill.blackhole.CDTimer > 0)
            {
                player.fx.CreatePopUpText("Cooldown");
                return;
            }

            stateMachine.ChangeState(player.blackHole);
        }
    }
}
