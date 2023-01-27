using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Camera UICamera;

    public StackObject carrying;

    private GameObject previousSlot;

    private GameObject obj;

    // Start is called before the first frame update
    private void Awake()
    { 
        carrying = null;
    }

    // Update is called once per frame
    void Update()
    {
        try { if (carrying.item == null || carrying.amount <= 0) { carrying = null; } } catch { carrying = null; }

        Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 700f, 1 << 5))
        {
            obj = hitInfo.collider.gameObject;

            StackObject stack;

            try { stack = obj.GetComponent<Slot>().stack; } catch { stack = null; }

            if (stack != null && stack.item != null && stack.amount > 0)
            {
                if (Input.GetMouseButtonDown(0) && carrying == null)
                {
                    carrying = new StackObject(stack.item, stack.amount);
                    obj.GetComponent<Slot>().stack = null;
                    previousSlot = obj;
                }
            }
            else if (stack == null && carrying != null)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    obj.GetComponent<Slot>().stack = new StackObject(carrying.item, carrying.amount);
                    carrying = null;
                }
            }
        }
        else { try { obj.transform.Find("Background").GetComponent<Image>().color = obj.GetComponent<Slot>().defaultBackgroundColor; } catch { } }
        if (carrying != null && Input.GetMouseButtonUp(0)) { previousSlot.GetComponent<Slot>().stack = new StackObject(carrying.item, carrying.amount); carrying = null; }
    }
}
