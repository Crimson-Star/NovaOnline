using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [Header("Initial stack's properties:")]
    public ItemObject item;
    [Range(0, 999)]
    public int amount;

    [Header("Current stack's properties (do not edit):")]
    public StackObject stack;

    // Start is called before the first frame update
    void Awake()
    {
        stack = new StackObject(item, amount);
    }

    private void Update()
    {
        if (stack.amount <= 0) { Destroy(this.gameObject); }
    }
}
