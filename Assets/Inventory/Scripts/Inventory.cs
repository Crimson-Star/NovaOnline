using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Initial properties:")]
    public List<StackObject> slots;
    public int space;

    [Header("Current properties (do not edit):")]
    public InventoryObject inventory;

    void Awake()
    {
        if (space < slots.Count) { space = slots.Count; }
        inventory = new InventoryObject(slots, space);     
    }
}
