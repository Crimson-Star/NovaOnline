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
        // Precisamos adicionar ao invent�rio (NeedToAdd) todos os itens que estamos tentando pegar
        int NeedToAdd = item.amount;

        while (NeedToAdd > 0)
        {
            // Ser� o valor que efetivamente iremos adicionar � pilha (Stack) de itens
            int ValueToAdd;

            // Pega um Slot que j� exista e que tem espa�o sobrando na pilha
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
                // Enche o m�ximo que puder caso o valor seja muito alto ao ponto de superar o espa�o sobrando
                if (NeedToAdd > desiredSlot.remainStack)
                {
                    ValueToAdd = desiredSlot.remainStack;
                }
                // Caso n�o ultrapasse o limite, apenas adicione o valor normalmente
                else
                {
                    ValueToAdd = NeedToAdd;
                }
                NeedToAdd -= ValueToAdd;

                desiredSlot.addItem(ValueToAdd);
            }
            else
            {
                // Se n�o, confere se tem vaga pra um novo slot e se aloja l�
                if (slots.Length < space)
                {
                    //slots.Append(new StackObject(item.item, NeedToAdd));
                    NeedToAdd = 0;
                }
                // N�o tem mais espa�o sobrando no invent�rio, logo, n�o pegue nada
                else break;
            }
        }

        // Botar o que sobrou de volta na pilha
        item.setAmount(NeedToAdd);
    }
}
