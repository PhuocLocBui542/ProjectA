using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{

    #region State
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDeathState deathState { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerBlackholeState blackHole { get; private set; }
    #endregion
    public bool isBusy { get; private set; }
    [Header("Move Attack")]
    public Vector2[] moveAtk;
    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    private float basemoveSpeed;
    private float basejumpForce;
    [Header("Dash Info")]
    public float dashSpeed = 25f;
    public float dashDuration;
    private float basedashSpeed;
    public float dashDir { get; private set; }
    public SkillManager skill { get; private set; }
    public PlayerFX fx { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        deathState = new PlayerDeathState(this, stateMachine, "Die");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        blackHole = new PlayerBlackholeState(this, stateMachine, "Jump");
    }

    protected override void Start()
    {
        //Start trong stateMachine
        /*stateMachine.Intialize( hanh vi dau tien khi khoi dong);*/
        base.Start();

        fx = GetComponent<PlayerFX>();

        skill = SkillManager.instance;

        stateMachine.Intialize(idleState);

        basemoveSpeed = moveSpeed;
        basejumpForce = jumpForce;
        basedashSpeed = dashSpeed;
    }
    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();

        stateMachine.currentState.Update();

        checkforDashInput();

        /*StartCoroutine("BusyFor", .1f);*/

        if (Input.GetKeyDown(KeyCode.A) && cd.enabled == true && skill.crystal.crystalUnlocked)
        {
            skill.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
           Inventory.instance.UseFlask();
            
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnBaseSpeed", _slowDuration);
    }

    protected override void ReturnBaseSpeed()
    {
        base.ReturnBaseSpeed();

        moveSpeed = basemoveSpeed;
        jumpForce = basejumpForce;
        dashSpeed = basedashSpeed;

    }

    public void AnimationTrigge() => stateMachine.currentState.AnimationFinishTrigger();

    //delay code
    public IEnumerable BusyFor(float _time)
    {
        isBusy = true;
        yield return new WaitForSeconds(_time);
        isBusy = false;
    }
    private void checkforDashInput()
    {
        if (IsWallDetected())
            return;

        if (skill.dash.dashUnlocked == false)
            return;
        
        if (Input.GetKeyDown(KeyCode.Z) && SkillManager.instance.dash.CanUseSkill() && cd.enabled == true)
        {
            
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
        /*====*/
        cd.enabled = false;
        rb.gravityScale = 0;
        
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
