using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] bool AllowDuplicateItem = false;
    [SerializeField] List<InventorySlot> Items;
    [SerializeField] int Selected;

    public bool AddItem(ItemData NewItem, bool SelectNew = true)
    {
        if (NewItem == null)
            return false;
        if (Items == null)
            Items = new List<InventorySlot>();

        //checks if item is alredy in inventory
        for (int i = 0; i < Items.Count; i++)
        {
            if(Items[i].Item == NewItem)
            {
                if (Items[i].Stack < NewItem.GetMaxStack())
                {
                    if (SelectNew)
                        Selected = i;

                    Items[i].Stack++;
                    return true;
                }
                else if (AllowDuplicateItem)
                    break;
                else
                    return false;

            }
        }

        // adds item if there is no stack or stack if full

        InventorySlot Temp = new InventorySlot();
        Temp.Item = NewItem;
        Temp.ItemUses = NewItem.GetMaxUses();
        Temp.LastUse = Time.time;

        Items.Add(Temp);

        if (SelectNew)
            Selected = Items.Count - 1;

        return true;
    }

    public bool UseItem()
    {
        if (Items[Selected].ItemUses > 0)
        {
            if (Items[Selected].Item.Use(gameObject))
            {
                Items[Selected].ItemUses -= 1;
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    public ItemData Next()
    {
        if (Items.Count == 0)
            return null;

        Selected++;

        if (Selected >= Items.Count)
            Selected = 0;

        return Items[Selected].Item;
    }

    public ItemData Previous()
    {
        if (Items.Count == 0)
            return null;

        Selected--;

        if (Selected < 0)
            Selected = Items.Count - 1;

        return Items[Selected].Item;
    }
}

class InventorySlot
{
    public ItemData Item;
    public int ItemUses;
    public int Stack;
    public float LastUse;
}