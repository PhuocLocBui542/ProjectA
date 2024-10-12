using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [Header("Plus Stats")]
    public int strengh; //1 point = 1 dmg + 1 %crit dmg
    public int agility; //1 point = 1 Anti Ad Dmg + 1% crit rate
    public int intelligence; // 1 point = 1 Ap + Anti 3 Ap dmg point
    public int vitality; //1 point 3-5 health

    [Header("Offensive stats")]
    public int damage;
    public int critRate;    //base : 5%
    public int critDamage;  //base : 150%

    [Header("Def Stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDmg;
    public int iceDmg;
    public int lightningDmg;

    [Header("Craft requirements")]
    public List<InventoryItem> crafttingMaterial;

    private int descriptionLength;

    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifiers(strengh);
        playerStats.agility.AddModifiers(agility);
        playerStats.intelligence.AddModifiers(intelligence);
        playerStats.vitality.AddModifiers(vitality);

        playerStats.damage.AddModifiers(damage);
        playerStats.critRate.AddModifiers(critRate);
        playerStats.critDamage.AddModifiers(critDamage);

        playerStats.maxHealth.AddModifiers(maxHealth);
        playerStats.armor.AddModifiers(armor);
        playerStats.evasion.AddModifiers(evasion);
        playerStats.magicResistance.AddModifiers(magicResistance);

        playerStats.fireDmg.AddModifiers(fireDmg);
        playerStats.iceDmg.AddModifiers(iceDmg);
        playerStats.lightningDmg.AddModifiers(lightningDmg);
    }

    public void RemoveModifiers() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifiers(strengh);
        playerStats.agility.RemoveModifiers(agility);
        playerStats.intelligence.RemoveModifiers(intelligence);
        playerStats.vitality.RemoveModifiers(vitality);

        playerStats.damage.RemoveModifiers(damage);
        playerStats.critRate.RemoveModifiers(critRate);
        playerStats.critDamage.RemoveModifiers(critDamage);

        playerStats.maxHealth.RemoveModifiers(maxHealth);
        playerStats.armor.RemoveModifiers(armor);
        playerStats.evasion.RemoveModifiers(evasion);
        playerStats.magicResistance.RemoveModifiers(magicResistance);

        playerStats.fireDmg.RemoveModifiers(fireDmg);
        playerStats.iceDmg.RemoveModifiers(iceDmg);
        playerStats.lightningDmg.RemoveModifiers(lightningDmg);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strengh, "Strengh");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critRate, "Crit Rate");
        AddItemDescription(critDamage, "Crit Damage");

        AddItemDescription(maxHealth, "Max Health");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Res");

        AddItemDescription(fireDmg, "Fire Dmg");
        AddItemDescription(iceDmg, "Ice Dmg");
        AddItemDescription(lightningDmg, "Lightning Dmg");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: " + itemEffects[i].effectDescription);
                descriptionLength++;
            }
        }

        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value,string _name)
    {
        if (_value != 0)
        {
            if(sb.Length > 0) 
                sb.AppendLine();

            if(_value > 0)
                sb.Append("+ " + _value + ": " + _name);

            descriptionLength++;
        }
    }
}
