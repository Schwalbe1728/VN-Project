using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentScript : MonoBehaviour {

    private static Dictionary<string, ItemScript> ItemsDictionary;

    [SerializeField]
    private EquipmentSerializableIndex Equipment;

    [SerializeField]
    private int money;

    public int AbstractMoneyValue { get { return money; } }
    public bool SubstractMoney(int abstractValue)
    {
        int after = money - abstractValue;
        bool result = after >= 0;

        if(result) { money -= abstractValue; }

        return result;
    }

    public void AddMoney(int abstractValue) { money += abstractValue; }

    public void AddItems(ItemScript item, int quantity = 1)
    {
        if(item.IsWeapon)
        {
            if (Equipment.Weapons == null) Equipment.Weapons = new EquipmentItemToQuantity();
            Equipment.Weapons.Add(item.Name, quantity);
        }

        if(item.IsConsumable)
        {
            if (Equipment.Consumables == null) Equipment.Consumables = new EquipmentItemToQuantity();
            Equipment.Consumables.Add(item.Name, quantity);
        }

        if(item.IsQuestItem)
        {
            if (Equipment.QuestItems == null) Equipment.QuestItems = new EquipmentItemToQuantity();
            Equipment.QuestItems.Add(item.Name, quantity);
        }

        if(item.IsMoney)
        {
            int val = item.AbstractValue * quantity;
            money += val;
        }
    }

    public void GetWeaponsList(out ItemScript[] items, out int[] quantities)
    {
        GetCollectionsList(Equipment.Weapons, out items, out quantities);
    }

    public void GetQuestItemsList(out ItemScript[] items, out int[] quantities)
    {
        GetCollectionsList(Equipment.QuestItems, out items, out quantities);
    }

    public void GetConsumables(out ItemScript[] items, out int[] quantities)
    {
        GetCollectionsList(Equipment.Consumables, out items, out quantities);
    }

    public bool HasItem(ItemScript item, int quantity = 1)
    {
        return Equipment.ContainsItem(item, quantity);                        
    }

    public bool TakeItem(ItemScript item, int quantity = 1, bool forced = false)
    {
        bool result = forced || HasItem(item, quantity);

        if(result)
        {
            Equipment.RemoveItems(item, quantity);
        }

        return result;
    }

    public void SetEquipment(EquipmentSerializableIndex eq = null)
    {
        if(eq == null)
        {
            eq = new EquipmentSerializableIndex();
        }

        Equipment = eq;
    }

    public void UseItem(ItemScript item)
    {
        if(item.IsUsable && this.HasItem(item, 1))
        {
            item.Use();

            if(item.IsConsumable)
            {
                TakeItem(item, 1, true);
            }
        }
    }

    void Awake()
    {
        if (ItemsDictionary == null || ItemsDictionary.Count == 0)
        {
            ItemScript[] allItems = Resources.LoadAll<ItemScript>("Items");
            ItemsDictionary = new Dictionary<string, ItemScript>();

            foreach (ItemScript item in allItems)
            {
                //Debug.Log(item.Name);
                ItemsDictionary.Add(EquipmentItemToQuantity.ItemToDictionaryKey(item), item);
            }
        }        
    }    

    private void GetCollectionsList(EquipmentItemToQuantity collection, out ItemScript[] items, out int[] quantities)
    {
        items = new ItemScript[collection.Count];
        quantities = new int[collection.Count];

        string[] keys = new string[collection.Count];
        collection.Keys.CopyTo(keys, 0);

        int i = 0;
        foreach (string key in keys)
        {
            items[i] = ItemsDictionary[key];
            quantities[i] = collection[key];
            i++;
        }
    }
}

[System.Serializable]
public class EquipmentSerializableIndex
{
    [SerializeField]
    public EquipmentItemToQuantity Weapons;

    [SerializeField]
    public EquipmentItemToQuantity Consumables;

    [SerializeField]
    public EquipmentItemToQuantity QuestItems;

    public EquipmentItemToQuantity ItemTypeToDictionary(ItemScript item)
    {
        EquipmentItemToQuantity result = null;

        if (item.IsWeapon) result = Weapons;
        if (item.IsQuestItem) result = QuestItems;
        if (item.IsConsumable) result = Consumables;

        return result;
    }

    public bool ContainsItem(ItemScript item, int quantity = 1)
    {
        bool result = false;
        EquipmentItemToQuantity collection = ItemTypeToDictionary(item);

        if(collection != null)
        {
            string key = EquipmentItemToQuantity.ItemToDictionaryKey(item);
            result = collection.ContainsKey(key) && collection[key] >= quantity;
        }

        return result;
    }

    public bool RemoveItems(ItemScript item, int quantity = 1)
    {
        bool hadRequestedNumber = ContainsItem(item, quantity);

        EquipmentItemToQuantity collection = ItemTypeToDictionary(item);
        if(collection != null)
        {
            collection[item] -= quantity;
        }

        return hadRequestedNumber;
    }
}
