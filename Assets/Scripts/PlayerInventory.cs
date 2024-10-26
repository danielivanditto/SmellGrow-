using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    struct Item
    {
        public int itemCount;
        public NewObject objAttributes;
    }

    private List<Item> inventory = new List<Item>();
    [SerializeField] private NewObject sterliceSeed;
    [SerializeField] private int sterliceSeedStartCount; // admin only

    private void Start()
    {
        AddItem(sterliceSeed, sterliceSeedStartCount);
    }

    public void AddItem(NewObject objectToAdd, int count)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            Item item = inventory[i];

            if (item.objAttributes.ID == objectToAdd.ID) // checks if the object is in the inventory
            {
                item.itemCount += count;
                inventory[i] = item;

                Debug.Log($"Successfully added {count} {objectToAdd.objectFullName} to inventory");
                return;
            }
        }

        Debug.Log($"Successfully added {count} {objectToAdd.objectFullName} to inventory");

        Item newItem = new Item();

        newItem.objAttributes = objectToAdd;
        newItem.itemCount = count;

        inventory.Add(newItem);
    }

    public void RemoveItem(NewObject objectToRemove, int count)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            Item item = inventory[i];

            if (item.objAttributes.ID == objectToRemove.ID)
            {
                item.itemCount -= count;
                inventory[i] = item;

                if (inventory[i].itemCount <= 0)
                {
                    Debug.Log($"{item.objAttributes.objectFullName} has been removed from your inventory");
                    inventory.RemoveAt(i);
                }

                return;
            }
        }

        Debug.Log("The player doesn't have that inventory item, which means you can't delete it");
    }

    public void UpdateInventoryItemCount(TextMeshProUGUI itemCount, NewObject obj)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            Item item = inventory[i];

            if (item.objAttributes.ID == obj.ID)
            {
                itemCount.text = item.itemCount.ToString();
            }
        }
    }

    public bool HasItem(NewObject obj)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            Item item = inventory[i];

            if (item.objAttributes.ID == obj.ID)
            {
                return true;
            }
        }

        return false;
    }

    public int GetItemCount(NewObject obj, out int itemCount)
    {
        itemCount = 0;

        for (int i = 0; i < inventory.Count; i++)
        {
            Item item = inventory[i];

            if (item.objAttributes.ID == obj.ID)
            {
                itemCount = item.itemCount;
                return itemCount;
            }
        }

        return 0;
    }

    private void ClearInventory() // admin only
    {
        inventory.Clear(); 
    }
}
