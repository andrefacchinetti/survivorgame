using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;
using TMPro;

[System.Serializable]
public class ItemCash : MonoBehaviour
{
	public LojaCashManager lojaCashManager;
	public Sprite itemImage;
	public string itemId = "BasicChest";
	public int itemCost = 199;
	public string Annotation = "Pack with 100 Coins";
	public string currencyPrefix = "$";

	public Image itemImageField;
	public TMP_Text itemCostTextField, itemDescTextField;

	// Use this for initialization
	void Start()
	{
		if (itemImage == null)
		{
			itemImage = Resources.Load<Sprite>("ItemSprites/DefaultImage");
		}
		itemImageField.sprite = itemImage;
		itemCostTextField.text = currencyPrefix + (float) itemCost/100;
		itemDescTextField.text = Annotation;
	}

	public void buyThisItemButton()
    {
		lojaCashManager.addItemAoCarrinho(this);
	}

}
