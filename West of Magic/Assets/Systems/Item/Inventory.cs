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
        Temp.Stack = 1;

        Items.Add(Temp);

        if (SelectNew)
            Selected = Items.Count - 1;

        return true;
    }

    public bool UseItem(GameObject User = null)
    {
        if (Items[Selected].ItemUses > 0)
        {
            if (Time.time - Items[Selected].LastUse < Items[Selected].Item.GetCoolDown())
            {
                if (Items[Selected].Item.Use(User == null ? gameObject : User))
                {
                    Items[Selected].LastUse = Time.time;

                    if (Items[Selected].Item.GetMaxUses() > 0)
                    {
                        Items[Selected].ItemUses -= 1;

                        if (Items[Selected].ItemUses <= 0 && Items[Selected].Item.GetConsumeOnUse())
                        {
                            Items[Selected].Stack -= 1;

                            if (Items[Selected].Stack <= 0)
                            {
                                Items.RemoveAt(Selected);
                                Selected -= 1;
                                if (Selected < 0)
                                    Selected = Items.Count - 1;
                            }
                            else
                                Items[Selected].ItemUses = Items[Selected].Item.GetMaxUses();
                        }
                    }
                    return true;
                }
            }
        }

        return false;
    }

    public int RestoreItem(int Uses = -1)
    {
        int CurentUses = Items[Selected].ItemUses;

        if(Uses == -1)
        {
            Items[Selected].ItemUses = Items[Selected].Item.GetMaxUses();
        }
        else
        {
            Items[Selected].ItemUses += Uses;

            if (Items[Selected].ItemUses > Items[Selected].Item.GetMaxUses())
                Items[Selected].ItemUses = Items[Selected].Item.GetMaxUses();
        }

        return Items[Selected].ItemUses - CurentUses;
    }

    public ItemData Current()
    {
        if (Items.Count == 0)
            return null;
        else
            return Items[Selected].Item;
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