using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHold_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinSpeed;
    private float bhTimer;

    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotKeys = true;
    private bool canAtkRealeased;
    private bool canDisapear = true;

    private int amountOfAttacks = 4;
    private float cloneAtkCD = .3f;
    private float cloneAtkTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAtkCD, float _bhDur)
    {
        maxSize = _maxSize; growSpeed = _growSpeed; shrinSpeed = _shrinkSpeed; amountOfAttacks = _amountOfAttacks; cloneAtkCD = _cloneAtkCD; bhTimer = _bhDur;

        if (SkillManager.instance.clone.crystalInseadClone)
            canDisapear = false;
    }

    private void Update()
    {
        cloneAtkTimer -= Time.deltaTime;
        bhTimer -= Time.deltaTime;

        if (bhTimer < 0)
        {
            bhTimer = Mathf.Infinity;
            if (targets.Count > 0)
                ReleaseCloneAtk();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ReleaseCloneAtk();
        }

        CloneAtkLogic();

        if (canGrow && !canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAtk()
    {
        if (targets.Count <= 0)
            return;

        DestroyHotKeys();
        canAtkRealeased = true;
        canCreateHotKeys = false;

        if (canDisapear)
        {
            canDisapear = false;
            PlayerManager.instance.player.fx.MakeTransprent(true);
        }
    }

    private void CloneAtkLogic()
    {
        if (cloneAtkTimer < 0 && canAtkRealeased && amountOfAttacks > 0)
        {
            cloneAtkTimer = cloneAtkCD;
            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            if (SkillManager.instance.clone.crystalInseadClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCtystalRandom();
            }
            else
            {
                SkillManager.instance.clone.createClone(targets[randomIndex], new Vector3(xOffset,0));
            }


            SkillManager.instance.clone.createClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility", .5f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        canAtkRealeased = false;
    }

    private void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
            Destroy(createdHotKey[i]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count < 0)
            return;

        if (!canCreateHotKeys)
            return;

        GameObject newHotKey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_Hotkey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();

        newHotKeyScript.SetupHotkey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
