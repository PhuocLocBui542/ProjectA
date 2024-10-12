using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackHold_Skill_Controller blackHold;

    public void SetupHotkey(KeyCode _myNewHotKey, Transform _myEnemy, BlackHold_Skill_Controller _blackHold)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myEnemy = _myEnemy;
        blackHold = _blackHold;
        myHotkey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            blackHold.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
        }
    }
}
