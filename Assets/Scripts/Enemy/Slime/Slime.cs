using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { big, medium, small }
public class Slime : Enemy
{
    [Header("Slime spesific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimeToCreate;
    [SerializeField] private GameObject slimePrefabs;
    [SerializeField] private Vector2 minCreationVelocity;
    [SerializeField] private Vector2 maxCreationVelocity;

    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunedState stunnedState { get; private set; }
    public SlimeDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new SlimeIdleState(stateMachine, this, "Idle", this);
        moveState = new SlimeMoveState(stateMachine, this, "Move", this);
        battleState = new SlimeBattleState(stateMachine, this, "Move", this);
        attackState = new SlimeAttackState(stateMachine, this, "Attack", this);

        stunnedState = new SlimeStunedState(stateMachine,this, "Stunned", this);
        deathState = new SlimeDeathState(stateMachine, this, "Die", this);

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

       /* if (slimeType == SlimeType.small)
            return;

        CreateSlime(slimeToCreate, slimePrefabs);*/
    }

    /*private void CreateSlime(int _amountOfSlimes, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Slime>().SetupSlime(facingDir);
        }
    }*/

    /*public void SetupSlime(int _facingDir)
    {
        if (_facingDir != facingDir)
            Flip();

        float xVelocity = Random.Range(minCreationVelocity.x , maxCreationVelocity.x);
        float yVelocity = Random.Range(minCreationVelocity.y , maxCreationVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDir, yVelocity);

        Invoke("CancelKnockbck", 1.5f);
    }

    private void CancelKnockback() => isKnocked = false;*/
}
 