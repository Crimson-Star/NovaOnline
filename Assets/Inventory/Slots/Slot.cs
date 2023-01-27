using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Color defaultBackgroundColor;
    public StackObject stack;
    private GameObject contextMenu;
    private Image image;
    private Text amount;

    // Start is called before the first frame update
    void Start()
    {
        if (stack.amount <= 0 || stack.item == null) { stack = null; }

        image = transform.Find("Image").GetComponent<Image>();
        amount = transform.Find("Amount").GetComponent<Text>();
        amount.text = "";

        if (this.name.StartsWith("Hotbar"))
        {
            foreach (char c in this.name)
            {
                try { transform.Find("Index").GetComponent<Text>().text =  int.Parse(c.ToString()).ToString(); } catch { }
            }
        }
    }

    void Update()
    {
        try { if (stack.item == null || stack.amount <= 0) { stack = null; } } catch { stack = null; }

        try
        { 
            image.sprite = stack.item.icon;
            image.color = new Color(1, 1, 1, 1);
            if (stack.amount > 1) { amount.text = stack.amount.ToString(); }
            else { amount.text = ""; }
        }
        catch
        {
            image.color = new Color(0, 0, 0, 0);
            amount.text = "";
        }
    }
}
