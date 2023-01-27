using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType
{
    Default,
    Equipment,
    Food
}
public abstract class ItemObject : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;
    public itemType type { get; protected set; }
    [TextArea(15,20)]
    public string description;
}
