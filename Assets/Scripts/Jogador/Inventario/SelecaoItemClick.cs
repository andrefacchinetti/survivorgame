using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelecaoItemClick : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            item.SelecionarItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            item.DroparItem();
        }
    }

}
