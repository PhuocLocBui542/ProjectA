using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //private => protected
    //virtual => call when need
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(7,12);
    [SerializeField] protected float knockbackDuration = .07f;
    protected bool isKnocked;
    [Header("Collision info")]
    public Transform atkCheck;
    public float atkRadius = 1.2f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance =.8f;
    [SerializeField] protected LayerMask whatIsGround;

    public int knockbackDir { get; private set; }

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnBaseSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact() => StartCoroutine("Hitknockback");

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockbackDir = -1;
        else if (_damageDirection.position.x < transform.position.x)
            knockbackDir = 1;
    }

    public void SetupKnockbackPower(Vector2 _knockbackpower) => knockbackPower = _knockbackpower;
    protected virtual IEnumerator Hitknockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        SetupZeroKnockbackPower();
    }

    protected virtual void SetupZeroKnockbackPower()
    {

    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(atkCheck.position, atkRadius);
    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null)
            onFlipped();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight) Flip();
        else if (_x < 0 && facingRight) Flip();
    }

    public virtual void SetupDefaultFacingDir(int _dir)
    {
        facingDir = _dir;

        if (facingDir == -1)
            facingRight = false;
    }
    #endregion
    #region Velocity
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void ZeroVelocity()
    {
        rb.velocity = new Vector2(0, 0);
    }
    #endregion
    public virtual void Die()
    {
        
    }
}
