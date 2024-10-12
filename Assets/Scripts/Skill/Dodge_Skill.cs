using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodge;
    public bool mirageUnlocked;

    protected override void Start()
    {
        base.Start();  
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirage();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifiers(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirage()
    {
        if (unlockMirageDodge.unlocked)
            mirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (mirageUnlocked)
            SkillManager.instance.clone.createClone(player.transform, new Vector3(2 * player.facingDir ,0));
    }
}
