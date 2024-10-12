using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private float atkMultiplier;
    [SerializeField] private GameObject clonePrefabs;
    [SerializeField] private float cloneDur;
    [Space]

    [Header("Clone Atk")]
    [SerializeField] private UI_SkillTreeSlot cloneAtkUnlockButton;
    [SerializeField] private float cloneAtkMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggressive clone")]
    [SerializeField] private UI_SkillTreeSlot aggresiveCloneUnlockButton;
    [SerializeField] private float aggresiveCloneAtkMultiplier;
    public bool canApplyOnHitEffect {  get; private set; }

    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multiCloneUnlockButton;
    [SerializeField] private float multiCloneAtkMultiplier;
    [SerializeField] private bool canDuplicate;
    [SerializeField] private float changetoDuplicate;

    [Header("Crystal Change To Clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInseadClone;

    protected override void Start()
    {
        base.Start();
        cloneAtkUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAtk);
        aggresiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multiCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    #region Unlock region

    protected override void CheckUnlock()
    {
        UnlockCloneAtk();
        UnlockAggresiveClone();
        UnlockMultiClone();
        UnlockCrystalInstead();
    }

    private void UnlockCloneAtk()
    {
        if (cloneAtkUnlockButton.unlocked)
        {
            canAttack = true;
            atkMultiplier = cloneAtkMultiplier;
        }
    }

    private void UnlockAggresiveClone()
    {
        if (aggresiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            atkMultiplier = aggresiveCloneAtkMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (multiCloneUnlockButton.unlocked)
        {
            canDuplicate = true;
            atkMultiplier = multiCloneAtkMultiplier;
        }
    }

    private void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockButton.unlocked)
        {
            crystalInseadClone = true;
        }
    }

    #endregion

    public void createClone(Transform _clonePosition, Vector3 _offset)
    {

        if (crystalInseadClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefabs);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDur, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicate,changetoDuplicate,player,atkMultiplier);
    }
}
