using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private CharacterMenu_UI UI;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null )
            statNameText.text = statName;
    }
    void Start()
    {
        UpdateStatValue();
        UI = GetComponentInParent<CharacterMenu_UI>();
    }

    // Update is called once per frame
    public void UpdateStatValue()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        if( playerStats != null )
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            if(statType == StatType.health )
                statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if(statType == StatType.damage )
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();

            if(statType == StatType.critDamage)
                statValueText.text = (playerStats.critDamage.GetValue() + playerStats.strength.GetValue()).ToString();

            if(statType == StatType.critRate)
                statValueText.text = (playerStats.critRate.GetValue() + playerStats.agility.GetValue()).ToString();

            if(statType == StatType.evasion)
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();

            if(statType == StatType.magicRes)
                statValueText.text = (playerStats.magicResistance.GetValue() + playerStats.intelligence.GetValue() * 3).ToString();

            
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        UI.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI.statToolTip.HideStatToolTip();
    }

}
