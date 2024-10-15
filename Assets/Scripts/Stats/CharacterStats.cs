using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelegence,
    vitality,
    damage,
    critRate,
    critDamage,
    health,
    armor,
    evasion,
    magicRes,
    fireDmg,
    iceDmg,
    lightingDmg
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Plus Stats")]
    public Stat strength; //1 point = 1 dmg + 1 %crit dmg
    public Stat agility; //1 point = 1 Anti Ad Dmg + 1% crit rate
    public Stat intelligence; // 1 point = 1 Ap + Anti 3 Ap dmg point
    public Stat vitality; //1 point 3-5 health

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critRate;    //base : 5%
    public Stat critDamage;  //base : 150%

    [Header("Def Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDmg;
    public Stat iceDmg;
    public Stat lightningDmg;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDmgCD = .3f; //deadtime betwwin 2 burn
    private float igniteDmgTimer;
    private int igniteDmg;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDmg;
    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible; 
    protected virtual void Start()
    {
        critRate.SetDefaultValue(5);
        critDamage.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDmgTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;

        if (isIgnited)
            ApplyIgniteDamage();
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCorotuien(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCorotuien(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifiers(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifiers(_modifier);
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (_targetStats.isInvincible)
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDmg = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDmg = CalculateCritDmg(totalDmg);
            criticalStrike = true;
        }

        fx.CreateHitFx(_targetStats.transform, criticalStrike);

        totalDmg = CheckTargetArmor(_targetStats, totalDmg);
        _targetStats.TakeDamage(totalDmg);

        DoMagicDamage(_targetStats);
    }

    #region Magic damage and ailemnts
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDmg = fireDmg.GetValue();
        int _iceDmg = iceDmg.GetValue();
        int _lightningDmg = lightningDmg.GetValue();

        int totalApDmg = _fireDmg + _iceDmg + _lightningDmg + intelligence.GetValue();

        totalApDmg = CheckApResistance(_targetStats, totalApDmg);
        _targetStats.TakeDamage(totalApDmg);

        if (Mathf.Max(_fireDmg, _iceDmg, _lightningDmg) <= 0) //make basic item 0/0/0 can't cause crack of deadscene
            return;
        AttemptyToApplyAilements(_targetStats, _fireDmg, _iceDmg, _lightningDmg);
    }

    private static void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDmg, int _iceDmg, int _lightningDmg)
    {
        bool canIgnite = _fireDmg > _iceDmg && _fireDmg > _lightningDmg;
        bool canChill = _iceDmg > _fireDmg && _iceDmg > _lightningDmg;
        bool canShock = _lightningDmg > _fireDmg && _lightningDmg > _iceDmg;

        while (!canChill && !canShock && !canIgnite)
        {
            if (Random.value < .3f && _fireDmg > 0)
            {
                canIgnite = true;
                _targetStats.ApplyElements(canIgnite, canChill, canShock);
                return;
            }
            if (Random.value < .5f && _iceDmg > 0)
            {
                canChill = true;
                _targetStats.ApplyElements(canIgnite, canChill, canShock);
                return;
            }
            if (Random.value < .5f && _lightningDmg > 0)
            {
                canShock = true;
                _targetStats.ApplyElements(canIgnite, canChill, canShock);
                return;
            }
        }
        if (canIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDmg * .2f));

        if (canShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightningDmg * .1f));

        _targetStats.ApplyElements(canIgnite, canChill, canShock);
    }
    public void ApplyElements(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = _chill;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                HitNearestTargetWithShockStrike();
            }
        }
    }

    private void HitNearestTargetWithShockStrike()
    {
        /*if (GetComponent<Player>() != null)
            return;*/

        Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, 25);

        float cloneDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in collider2D)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < cloneDistance)
                {
                    cloneDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ThunderStrike_Controller>().Setup(shockDmg, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    private void ApplyIgniteDamage()
    {
        if (igniteDmgTimer < 0)
        {
            DecreaseHealthBy(igniteDmg);

            if (currentHealth <= 0 && !isDead)
                Die();

            igniteDmgTimer = igniteDmgCD;
        }
    }

    public void ApplyShock(bool _shock)
    {
        shockedTimer = ailmentsDuration;
        isShocked = _shock;

        fx.ShockFxFor(ailmentsDuration);
    }

    public void SetupIgniteDamage(int _damage) => igniteDmg = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDmg = _damage;
    #endregion
    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");
        if (currentHealth <= 0 && !isDead)
            Die();
    }

    public virtual void IncreaseHealthBy(int _amout)
    {
        currentHealth += _amout;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        

        currentHealth -= _damage;

        if (_damage > 0)
            fx.CreatePopUpText(_damage.ToString());

        if (onHealthChanged != null)
            onHealthChanged();
    }
    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    #region Stat calculations
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDmg)
    {
        if (_targetStats.isChilled)
            totalDmg -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDmg -= _targetStats.armor.GetValue();

        totalDmg = Mathf.Clamp(totalDmg, 0, int.MaxValue);
        return totalDmg;
    }
    private int CheckApResistance(CharacterStats _targetStats, int totalApDmg)
    {
        totalApDmg -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue());
        totalApDmg = Mathf.Clamp(totalApDmg, 0, int.MaxValue);
        return totalApDmg;
    }

    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }
    protected bool CanCrit()
    {
        int totalCritcalChance = critRate.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCritcalChance)
        {
            return true;
        }
        return false;
    }

    protected int CalculateCritDmg(int _damage)
    {
        float totalCD = (critDamage.GetValue() + strength.GetValue()) * .01f;
        float CD = _damage * totalCD;
        return Mathf.RoundToInt(CD);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.intelegence) return intelligence;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critRate) return critRate;
        else if (_statType == StatType.critDamage) return critDamage;
        else if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.armor) return armor;
        else if (_statType == StatType.evasion) return evasion;
        else if (_statType == StatType.magicRes) return magicResistance;
        else if (_statType == StatType.fireDmg) return fireDmg;
        else if (_statType == StatType.iceDmg) return iceDmg;
        else if (_statType == StatType.lightingDmg) return lightningDmg;

        return null;
    }
}
