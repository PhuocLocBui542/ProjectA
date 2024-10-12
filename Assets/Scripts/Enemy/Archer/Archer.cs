using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    [Header("Archer spisifc info")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private int arrowDmg;

    public Vector2 jumpVelocity;
    public float jumpCd;
    public float safeDistance;
    [HideInInspector] public float lastTimeJumped;

    [Header("Additional collision check")]
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;

    #region State
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherStunnedState stunnedState { get; private set; }
    public ArcherDeathState deathState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new ArcherIdleState(stateMachine, this, "Idle", this);
        moveState = new ArcherMoveState(stateMachine, this, "Move", this);
        battleState = new ArcherBattleState(stateMachine, this, "Idle ", this);
        attackState = new ArcherAttackState(stateMachine, this, "Attack", this);
        jumpState = new ArcherJumpState(stateMachine, this, "Jump", this);

        stunnedState = new ArcherStunnedState(stateMachine, this, "Stunned", this);
        deathState = new ArcherDeathState(stateMachine, this, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

    public override void AnamtionSpecialAttackTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, atkCheck.position, Quaternion.identity);

        newArrow.GetComponent<ArrowController>().SetupArrow(arrowSpeed * facingDir, stats);
    }

    public bool GroundBehind() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, whatIsGround);
    public bool WallBehind() => Physics2D.Raycast(wallCheck.position, Vector2.right * -facingDir, wallCheckDistance + 2, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize); 
    }
}
