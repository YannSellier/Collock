using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBank", menuName = "Custom/Create Items Bank", order = 1)]
public class ItemBank : ScriptableObject
{
	public List<DDItem> ddItems;


	public int GetDDItemIndex(Item item)
	{
		for (int i = 0; i < ddItems.Count; i++)
			if (item.itemName == ddItems[i].itemName)
				return i;
		return -1;
	}
}
