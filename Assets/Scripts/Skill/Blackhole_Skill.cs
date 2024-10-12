using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{


    [SerializeField] private UI_SkillTreeSlot blackHoleUnlockButton;
    public bool blackholeUnlocked {  get; private set; }
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCD;
    [SerializeField] private float blackholeDur;
    [Space]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    BlackHold_Skill_Controller BlackHoleCurrent;

    private void UnlockBlackHole()
    {
        if (blackHoleUnlockButton.unlocked)
            blackholeUnlocked = true;
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();



        GameObject newBlackHole = Instantiate(blackholePrefab, player.transform.position,Quaternion.identity);

        BlackHoleCurrent = newBlackHole.GetComponent<BlackHold_Skill_Controller>();

        BlackHoleCurrent.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCD, blackholeDur);

        AudioManager.instance.PlaySFX(3 , player.transform);
        AudioManager.instance.PlaySFX(6 , player.transform);
    }

    protected override void Start()
    {
        base.Start();
        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool Finish()
    {
        if (!BlackHoleCurrent)
            return false;

        if (BlackHoleCurrent.playerCanExitState)
        {
            BlackHoleCurrent = null;
            return true;
        }
        return false;
    }

    public float BlackholeRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockBlackHole();
    }
}
