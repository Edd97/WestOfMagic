using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [Header("Basic Data")]
    [SerializeField] string ItemName = "New Item"; // Display name for this item
    [SerializeField] Image ItemImage; // Display image for this item
    [SerializeField] int MaxStack = 1; // How many of this item should the inventory put into a single slot
    [SerializeField] int MaxUses = 0; // Number of times an item can be used (0 = no limit)
    [SerializeField] bool ConsumeOnUse = false; // Should the item be removed from the inventory when the use limit is reached
    [SerializeField] float CoolDown = 0f; // minimum time in seconds between item uses

    public string GetItemName() { return ItemName; }
    public Image GetItemImage() { return ItemImage; }
    public int GetMaxStack () { return MaxStack; }
    public int GetMaxUses() { return MaxUses; }
    public bool GetConsumeOnUse() { return ConsumeOnUse; }
    public float GetCoolDown() { return CoolDown; }

    public void OnValidate()
    {
        if (MaxStack < 1)
            MaxStack = 1;
        if (MaxUses < 0)
            MaxUses = 0;
        if (CoolDown < 0)
            CoolDown = 0;
    }

    public virtual bool Use()
    {
        Debug.Log("Item Used: " + ItemName);
        return true;
    }
}
