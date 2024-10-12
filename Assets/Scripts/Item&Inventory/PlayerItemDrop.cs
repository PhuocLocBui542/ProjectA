using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Drop")]
    [SerializeField] private float chanceToLooseItem;
    [SerializeField] private float chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materulsToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipment())
        {
            if (Random.Range(0, 100) <= chanceToLooseItem)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if(Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                DropItem(item.data);
                materulsToLoose.Add(item);
            }
        }

        for (int i = 0; i < materulsToLoose.Count; i++)
        {
            inventory.RemoveItem(materulsToLoose[i].data);
        }
    }
}
