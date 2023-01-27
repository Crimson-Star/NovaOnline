using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarScript : MonoBehaviour
{
    private List<GameObject> slots;
    private GameObject previousSlot;

    public GameObject selectedSlot;

    [Space]

    [Header("Colors:")]
    public Color defaultBackgroundColor;
    public Color selectedBackgroundColor;
    void Awake()
    {
        slots = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            slots.Add(transform.GetChild(i).transform.gameObject);
        }

        selectedSlot = slots[0];

        previousSlot = null;
        selectedSlot.transform.Find("Background").GetComponent<Image>().color = selectedBackgroundColor;
        selectedSlot.GetComponent<Slot>().defaultBackgroundColor = selectedBackgroundColor;
    }
    private void OnGUI()
    {
        Event e = Event.current;
        KeyCode key = e.keyCode;
        int index;

        if (e.isKey && key.ToString().StartsWith("Alpha"))
        {
            if (int.TryParse(key.ToString().Substring(5), out index))
            {
                selectSlot(index);
            }

        }
    }
    public void selectSlot(int slotIndex)
    {
        previousSlot = selectedSlot;
        if (slotIndex == 0) { slotIndex = 10; }
        selectedSlot = slots[--slotIndex];

        previousSlot.transform.Find("Background").GetComponent<Image>().color = defaultBackgroundColor;
        previousSlot.GetComponent<Slot>().defaultBackgroundColor = defaultBackgroundColor;

        selectedSlot.transform.Find("Background").GetComponent<Image>().color = selectedBackgroundColor;
        selectedSlot.GetComponent<Slot>().defaultBackgroundColor = selectedBackgroundColor;
    }
}
