using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastarItensInventario : MonoBehaviour
{
    private Item item;
    private GameObject placeHolder, slot1;
    private int slot2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DragStartItemInventario(Item itemDrag, GameObject go1){
        Debug.Log("Pegou item " + go1.name);
        item = itemDrag;
        slot1 = go1;
    }

    public void DragEndItemInventario(SlotHotbar slotHotbar){
        slotHotbar.SetupSlotHotbar(item);
        item = null;
    }

    public void DragEndItemInventario(ItemArmadura slotArmadura)
    {
        slotArmadura.ColocarItemNoSlot(item);
        item = null;
    }

    public void TrocarLugarInventario(GameObject go2){
        Debug.Log("trocou " + go2.name + " por " + slot1.name);
        slot2 = go2.transform.GetSiblingIndex();
        go2.transform.SetSiblingIndex(slot1.transform.GetSiblingIndex());
        slot1.transform.SetSiblingIndex(slot2);
    }

    public void StopDrag(){
        Debug.Log("Soltou item");
        item = null;
    }
}
