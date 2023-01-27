using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [Header("References:")]
    public Transform destinyHud;
    public Controls control;

    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private Inventory inventory;

    // Update is called once per frame
    void Update()
    {
        // Move o player até o lugar clicado
        navMeshAgent.destination = destinyHud.position;

        if (control.clickedObj != null)
        {
            if (control.clickedObj.layer == 10)
            {
                StackObject drop = control.clickedObj.GetComponent<Stack>().stack;
                if ((transform.position - control.clickedObj.transform.position).magnitude < 2f)
                {
                    inventory.inventory.AddStack(drop);
                }
            }

        }
    }
}
