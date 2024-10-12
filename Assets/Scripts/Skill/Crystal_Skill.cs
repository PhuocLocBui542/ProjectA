using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDur;
    [SerializeField] private GameObject CrystalPrefabs;
    private GameObject currentCrystal;

    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Explosive Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveCrystalButton;
    [SerializeField] private float explisoveCooldown;
    [SerializeField] private bool canExplode;

    [Header("Clone Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockCloneCrystalButton;
    [SerializeField] private bool cloneCrystal;

    [Header("Moving Crystal")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Crystal Stack")]
    [SerializeField] private UI_SkillTreeSlot unlockStackCrystalButton;
    [SerializeField] private bool canStack;
    [SerializeField] private int amountOfStack;
    [SerializeField] private float multiStackCD;
    [SerializeField] private float useTime;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockCloneCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCloneCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockStackCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalStack);
    }

    #region Skill Unlock
    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockExplosiveCrystal();
        UnlockCloneCrystal();
        UnlockMovingCrystal();
        UnlockCrystalStack();
    }

    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }
    
    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveCrystalButton.unlocked)
        {
            canExplode = true;
            CD = explisoveCooldown;
        }
    }

    private void UnlockCloneCrystal()
    {
        if (unlockCloneCrystalButton.unlocked)
            cloneCrystal = true;
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }

    private void UnlockCrystalStack()
    {
        if (unlockStackCrystalButton.unlocked)
            canStack = true;
    }

    #endregion

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseStackCrystal())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = PlayerManager.instance.player.transform.position;
            PlayerManager.instance.player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;
            if (cloneCrystal)
            {
                SkillManager.instance.clone.createClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }

        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(CrystalPrefabs, PlayerManager.instance.player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScirpt = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScirpt.SetupCrystal(crystalDur, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform),player);
    }
    
    public void CurrentCtystalRandom() => currentCrystal.GetComponent<Crystal_Skill_Controller>().RandomEnemyChossen();
    private bool CanUseStackCrystal()
    {
        if (canStack)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStack)
                    Invoke("resetSkill", useTime);

                CD = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, PlayerManager.instance.player.transform.position, Quaternion.identity);
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDur, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalLeft.Count <= 0)
                {
                    CD = multiStackCD;
                    RefillCrystal();
                }
                return true;
            }
        }

        return false;
    }
    private void RefillCrystal()
    {
        int amountToAdd = amountOfStack - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(CrystalPrefabs);
        }
    }

    private void resetSkill()
    {
        if (CDTimer > 0)
            return;

        CDTimer = multiStackCD;
        RefillCrystal();
    }
}
