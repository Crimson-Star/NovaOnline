using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryObject
{
    [SerializeField]
    private StackObject[] slots;
    public int space;

    public InventoryObject(List<StackObject> slots, int space = 50)
    {
        if (space < slots.Count) { space = slots.Count; }

        this.slots = new StackObject[space];

        for (int i = 0; i < slots.Count; i++)
        {
            this.slots[i] = new StackObject(slots[i].item, slots[i].amount);
        }

        this.space = space;
    }

    public void AddStack(StackObject item)
    {
        // Precisamos adicionar ao inventário (NeedToAdd) todos os itens que estamos tentando pegar
        int NeedToAdd = item.amount;

        while (NeedToAdd > 0)
        {
            // Será o valor que efetivamente iremos adicionar à pilha (Stack) de itens
            int ValueToAdd;

            // Pega um Slot que já exista e que tem espaço sobrando na pilha
            StackObject desiredSlot = null;

            foreach (StackObject slot in slots)
            {
                if (slot.item.name == item.item.name && item.remainStack > 0)
                {
                    desiredSlot = slot;
                    break;
                }
            }
            for (int i = 0;i < slots.Length; i++)
            {
                if (slots[i] == null) { desiredSlot = slots[i]; }
            }

            if (desiredSlot != null)
            {
                // Enche o máximo que puder caso o valor seja muito alto ao ponto de superar o espaço sobrando
                if (NeedToAdd > desiredSlot.remainStack)
                {
                    ValueToAdd = desiredSlot.remainStack;
                }
                // Caso não ultrapasse o limite, apenas adicione o valor normalmente
                else
                {
                    ValueToAdd = NeedToAdd;
                }
                NeedToAdd -= ValueToAdd;

                desiredSlot.addItem(ValueToAdd);
            }
            else
            {
                // Se não, confere se tem vaga pra um novo slot e se aloja lá
                if (slots.Length < space)
                {
                    //slots.Append(new StackObject(item.item, NeedToAdd));
                    NeedToAdd = 0;
                }
                // Não tem mais espaço sobrando no inventário, logo, não pegue nada
                else break;
            }
        }

        // Botar o que sobrou de volta na pilha
        item.setAmount(NeedToAdd);
    }
}
