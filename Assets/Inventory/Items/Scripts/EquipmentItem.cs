using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentItem : ItemObject
{
    public int atkBonus;
    public int defBonus;
    public void Awake()
    {
        type = itemType.Equipment;
    }
}
