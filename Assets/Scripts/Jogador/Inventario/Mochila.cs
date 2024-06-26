using UnityEngine;
using UnityEngine.EventSystems;

public class Mochila : MonoBehaviour
{

    public ArrastarItensInventario arrastarItensInventario;

    private void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drop;
        entry.callback.AddListener((data) => { OnDropDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnDropDelegate(PointerEventData data)
    {
        Debug.Log("OnDropDelegate mochila: ");
        arrastarItensInventario.DragEndItemMochila();
    }

}
